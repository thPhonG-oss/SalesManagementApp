package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.responses.*;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.services.ReportService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.TextStyle;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class ReportServiceImpl implements ReportService {
    OrderRepository orderRepository;


    @Override
    public List<DailyRevenueResponse> getDailyRevenue(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getDailyRevenue(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        // Map kết quả từ DB
        Map<LocalDate, DailyRevenueResponse> dataMap = results.stream()
                .collect(Collectors.toMap(
                        obj -> convertToLocalDate(obj[0]),
                        obj -> new DailyRevenueResponse(
                                convertToLocalDate(obj[0]),
                                obj[1] != null ? ((Number) obj[1]).doubleValue() : 0.0
                        )
                ));

        // Tạo danh sách đầy đủ các ngày (bao gồm ngày không có dữ liệu)
        List<DailyRevenueResponse> result = new ArrayList<>();
        LocalDate current = startDate;
        while (!current.isAfter(endDate)) {
            result.add(dataMap.getOrDefault(current,
                    new DailyRevenueResponse(current, 0.0)));
            current = current.plusDays(1);
        }

        return result;
    }

    @Override
    public List<WeeklyRevenueResponse> getWeeklyRevenue(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getWeeklyRevenue(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        return results.stream()
                .map(obj -> new WeeklyRevenueResponse(
                        ((Number) obj[0]).intValue(),  // year
                        ((Number) obj[1]).intValue(),  // week_number
                        convertToLocalDate(obj[2]),    // week_start
                        convertToLocalDate(obj[3]),    // week_end
                        obj[4] != null ? ((Number) obj[4]).doubleValue() : 0.0  // revenue
                ))
                .collect(Collectors.toList());
    }

    @Override
    public List<MonthlyRevenueResponse> getMonthlyRevenue(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getMonthlyRevenue(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        return results.stream()
                .map(obj -> {
                    int year = ((Number) obj[0]).intValue();
                    int month = ((Number) obj[1]).intValue();
                    String monthName = LocalDate.of(year, month, 1)
                            .getMonth()
                            .getDisplayName(TextStyle.FULL, Locale.getDefault());

                    return new MonthlyRevenueResponse(
                            year,
                            month,
                            monthName,
                            obj[2] != null ? ((Number) obj[2]).doubleValue() : 0.0
                    );
                })
                .collect(Collectors.toList());
    }

    @Override
    public List<YearlyRevenueResponse> getYearlyRevenue(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getYearlyRevenue(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        return results.stream()
                .map(obj -> new YearlyRevenueResponse(
                        ((Number) obj[0]).intValue(),
                        obj[1] != null ? ((Number) obj[1]).doubleValue() : 0.0
                ))
                .collect(Collectors.toList());
    }

    @Override
    public List<ProductSalesResponse> getProductSales(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getProductSales(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        return results.stream()
                .map(obj -> new ProductSalesResponse(
                        ((Number) obj[0]).longValue(),     // product_id
                        (String) obj[1],                   // product_name
                        ((Number) obj[2]).intValue(),      // quantity_sold
                        obj[3] != null ? ((Number) obj[3]).doubleValue() : 0.0  // revenue
                ))
                .collect(Collectors.toList());
    }

    @Override
    public List<DailyProductSalesResponse> getDailyProductSales(LocalDate startDate, LocalDate endDate) {
        LocalDateTime start = startDate.atStartOfDay();
        LocalDateTime end = endDate.atTime(LocalTime.MAX);

        List<Object[]> results = orderRepository.getDailyProductSales(
                Timestamp.valueOf(start),
                Timestamp.valueOf(end)
        );

        return results.stream()
                .map(obj -> new DailyProductSalesResponse(
                        convertToLocalDate(obj[0]),        // date
                        ((Number) obj[1]).longValue(),     // product_id
                        (String) obj[2],                   // product_name
                        ((Number) obj[3]).intValue()       // quantity_sold
                ))
                .collect(Collectors.toList());
    }

    @Override
    public ReportResponse getCurrentMonthDashboard() {
        LocalDate today = LocalDate.now();
        LocalDate firstDay = today.withDayOfMonth(1);
        LocalDate lastDay = today.withDayOfMonth(today.lengthOfMonth());

        return ReportResponse.builder()
                .dailyRevenue(getDailyRevenue(firstDay, lastDay))
                .topProducts(getProductSales(firstDay, lastDay))
                .dailyProductSales(getDailyProductSales(firstDay, lastDay))
                .build();
    }

    @Override
    public List<WeeklyProductSalesResponse> getWeeklyProductSales(LocalDate startDate, LocalDate endDate) {
        List<Object[]> results = orderRepository.getWeeklyProductSales(
                Timestamp.valueOf(startDate.atStartOfDay()),
                Timestamp.valueOf(endDate.atTime(LocalTime.MAX))
        );

        return results.stream()
                .map(obj -> new WeeklyProductSalesResponse(
                        ((Number) obj[0]).intValue(), // year
                        ((Number) obj[1]).intValue(), // week
                        ((Number) obj[2]).longValue(), // product_id
                        (String) obj[3],               // product_name
                        ((Number) obj[4]).intValue(),  // quantity
                        obj[5] != null ? ((Number) obj[5]).doubleValue() : 0.0
                ))
                .collect(Collectors.toList());
    }

    @Override
    public List<MonthlyProductSalesResponse> getMonthlyProductSales(LocalDate startDate, LocalDate endDate) {
        List<Object[]> results = orderRepository.getMonthlyProductSales(
                Timestamp.valueOf(startDate.atStartOfDay()),
                Timestamp.valueOf(endDate.atTime(LocalTime.MAX))
        );

        return results.stream()
                .map(obj -> {
                    int year = ((Number) obj[0]).intValue();
                    int month = ((Number) obj[1]).intValue();
                    String monthName = LocalDate.of(year, month, 1)
                            .getMonth()
                            .getDisplayName(TextStyle.FULL, Locale.getDefault());

                    return new MonthlyProductSalesResponse(
                            year,
                            month,
                            monthName,
                            ((Number) obj[2]).longValue(),
                            (String) obj[3],
                            ((Number) obj[4]).intValue(),
                            obj[5] != null ? ((Number) obj[5]).doubleValue() : 0.0
                    );
                })
                .collect(Collectors.toList());
    }

    @Override
    public List<YearlyProductSalesResponse> getYearlyProductSales(LocalDate startDate, LocalDate endDate) {
        List<Object[]> results = orderRepository.getYearlyProductSales(
                Timestamp.valueOf(startDate.atStartOfDay()),
                Timestamp.valueOf(endDate.atTime(LocalTime.MAX))
        );

        return results.stream()
                .map(obj -> new YearlyProductSalesResponse(
                        ((Number) obj[0]).intValue(),
                        ((Number) obj[1]).longValue(),
                        (String) obj[2],
                        ((Number) obj[3]).intValue(),
                        obj[4] != null ? ((Number) obj[4]).doubleValue() : 0.0
                ))
                .collect(Collectors.toList());
    }

    // ===== HELPER METHOD =====

    private LocalDate convertToLocalDate(Object dateObj) {
        if (dateObj instanceof java.sql.Timestamp) {
            return ((java.sql.Timestamp) dateObj).toLocalDateTime().toLocalDate();
        } else if (dateObj instanceof java.sql.Date) {
            return ((java.sql.Date) dateObj).toLocalDate();
        } else {
            throw new RuntimeException("Unknown date type: " + dateObj.getClass());
        }
    }
}
