package com.project.sales_management.services.Impl;


import com.project.sales_management.dtos.responses.DailyRevenueResponse;
import com.project.sales_management.dtos.responses.DashboardResponse;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.mappers.OrderMapper;
import com.project.sales_management.mappers.ProductMapper;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.repositories.ProductRepository;
import com.project.sales_management.services.DashboardService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class DashboardServiceImpl implements DashboardService {

    ProductRepository productRepository;
    OrderRepository orderRepository;
    ProductMapper productMapper;
    OrderMapper orderMapper;

    @Override
    public DashboardResponse getDashboard() {
        LocalDate today = LocalDate.now();
        LocalDateTime startOfDay = today.atStartOfDay();
        LocalDateTime endOfDay = today.atTime(LocalTime.MAX);

        // Tổng số sản phẩm
        long totalProducts = productRepository.countByIsActiveTrue();

        // Top 5 sắp hết hàng
        Pageable top5 = PageRequest.of(0, 5, Sort.by("stockQuantity").ascending());
        List<ProductResponse> lowStockProducts = productRepository
                .findLowStockProducts(top5)
                .stream()
                .map(productMapper::toProductResponse)
                .toList();

        // Top 5 bán chạy
        List<ProductResponse> topSellingProducts = productRepository
                .findTop5SellingProducts()
                .stream()
                .map(productMapper::toProductResponse)
                .toList();

        // Tổng đơn hôm nay
        long todayOrderCount = orderRepository
                .countByOrderDateBetween(startOfDay, endOfDay);

        // Doanh thu hôm nay
        double todayRevenue = orderRepository
                .getTodayRevenue(startOfDay, endOfDay);

        // 3 đơn gần nhất
        List<OrderResponse> recentOrders = orderRepository
                .findTop3ByOrderByCreatedAtDesc()
                .stream()
                .map(orderMapper::toOrderResponse)
                .toList();


        // ========== DOANH THU THEO NGÀY TRONG THÁNG ==========
        LocalDate firstDay = today.withDayOfMonth(1);
        LocalDate lastDay = today.withDayOfMonth(today.lengthOfMonth());

        Timestamp startDate = Timestamp.valueOf(firstDay.atStartOfDay());
        Timestamp endDate = Timestamp.valueOf(lastDay.atTime(23, 59, 59));

        System.out.println("Getting daily revenue from " + firstDay + " to " + lastDay);

        List<Object[]> dbResults = orderRepository.getDailyRevenueInMonthNative(startDate, endDate);

        System.out.println("Query returned " + dbResults.size() + " rows");

        // Debug: In ra vài dòng đầu để kiểm tra
        if (!dbResults.isEmpty()) {
            for (int i = 0; i < Math.min(3, dbResults.size()); i++) {
                Object[] row = dbResults.get(i);
                System.out.println("Row " + i + ": date=" + row[0] +
                        " (type: " + (row[0] != null ? row[0].getClass().getSimpleName() : "null") +
                        "), revenue=" + row[1] +
                        " (type: " + (row[1] != null ? row[1].getClass().getSimpleName() : "null") + ")");
            }
        }

        // Map kết quả từ DB
        Map<LocalDate, Double> revenueMap = new HashMap<>();
        for (Object[] row : dbResults) {
            try {
                LocalDate date = null;

                // Xử lý date
                if (row[0] instanceof java.sql.Date) {
                    date = ((java.sql.Date) row[0]).toLocalDate();
                } else if (row[0] instanceof java.sql.Timestamp) {
                    date = ((java.sql.Timestamp) row[0]).toLocalDateTime().toLocalDate();
                } else if (row[0] instanceof LocalDate) {
                    date = (LocalDate) row[0];
                } else if (row[0] != null) {
                    System.out.println("WARNING: Unknown date type: " + row[0].getClass().getName());
                    continue;
                }

                // Xử lý revenue
                Double revenue = 0.0;
                if (row[1] != null) {
                    if (row[1] instanceof Number) {
                        revenue = ((Number) row[1]).doubleValue();
                    } else {
                        System.out.println("WARNING: Revenue is not a Number: " + row[1].getClass().getName());
                    }
                }

                if (date != null) {
                    revenueMap.put(date, revenue);
                    System.out.println("Mapped: " + date + " -> " + revenue);
                }

            } catch (Exception e) {
                System.err.println("Error processing row: " + e.getMessage());
                e.printStackTrace();
            }
        }

        System.out.println("Revenue map size: " + revenueMap.size());

        // Tạo danh sách đầy đủ các ngày trong tháng
        List<DailyRevenueResponse> dailyRevenue = new ArrayList<>();
        for (int day = 1; day <= today.lengthOfMonth(); day++) {
            LocalDate date = LocalDate.of(today.getYear(), today.getMonth(), day);
            Double revenue = revenueMap.getOrDefault(date, 0.0);

            DailyRevenueResponse response = DailyRevenueResponse.builder()
                    .date(date)
                    .revenue(revenue)
                    .build();

            dailyRevenue.add(response);

            // In vài ngày đầu và cuối
            if (day <= 3 || day >= today.lengthOfMonth() - 2) {
                System.out.println("Day " + day + ": " + date + " -> " + revenue);
            }
        }

        System.out.println("Daily revenue list size: " + dailyRevenue.size());

        double totalRevenue = dailyRevenue.stream()
                .mapToDouble(DailyRevenueResponse::getRevenue)
                .sum();
        System.out.println("Total revenue in month: " + totalRevenue);


        return DashboardResponse.builder()
                .totalProducts(totalProducts)
                .lowStockProducts(lowStockProducts)
                .topSellingProducts(topSellingProducts)
                .todayOrderCount(todayOrderCount)
                .todayRevenue(todayRevenue)
                .recentOrders(recentOrders)
                .dailyRevenue(dailyRevenue)
                .build();


    }
}
