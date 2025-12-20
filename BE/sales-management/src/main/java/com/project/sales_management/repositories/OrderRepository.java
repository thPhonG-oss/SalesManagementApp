package com.project.sales_management.repositories;

import com.project.sales_management.dtos.responses.DailyRevenueResponse;
import com.project.sales_management.models.Order;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface OrderRepository extends JpaRepository<Order, Long> {

    // Tổng số đơn trong ngày
    long countByOrderDateBetween(LocalDateTime start, LocalDateTime end);

    // Tổng doanh thu trong ngày
    @Query("""
        SELECT COALESCE(SUM(o.totalAmount), 0)
        FROM Order o
        WHERE o.orderDate BETWEEN :start AND :end
          AND o.status = 'PAID'
    """)
    Double getTodayRevenue(@Param("start") LocalDateTime start,
                           @Param("end") LocalDateTime end);

    // 3 đơn hàng gần nhất
    List<Order> findTop3ByOrderByCreatedAtDesc();


    // Doanh thu theo ngày trong tháng hiện tại
    @Query(value = """
    SELECT DATE(order_date) as day,
           COALESCE(SUM(total_amount), 0) as total
    FROM orders
    WHERE order_date BETWEEN :startDate AND :endDate
    GROUP BY DATE(order_date)
    ORDER BY DATE(order_date)
    """, nativeQuery = true)
    List<Object[]> getDailyRevenueInMonthNative(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate
    );



}
