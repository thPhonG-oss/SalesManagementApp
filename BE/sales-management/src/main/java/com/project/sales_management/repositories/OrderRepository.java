package com.project.sales_management.repositories;

import com.project.sales_management.dtos.responses.DailyRevenueResponse;
import com.project.sales_management.models.Order;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.JpaSpecificationExecutor;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface OrderRepository extends JpaRepository<Order, Long>, JpaSpecificationExecutor<Order> {

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
     AND status = 'PAID'
    GROUP BY DATE(order_date)
    ORDER BY DATE(order_date)
    """, nativeQuery = true)
    List<Object[]> getDailyRevenueInMonthNative(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate
    );

    @Query(value = """
        SELECT DATE(order_date) as date,
               SUM(total_amount) as revenue
        FROM orders
        WHERE order_date BETWEEN :startDate AND :endDate
         AND status = 'PAID'
        GROUP BY DATE(order_date)
        ORDER BY DATE(order_date)
        """, nativeQuery = true)
    List<Object[]> getDailyRevenue(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);



    // ===== REVENUE BY WEEK =====

    @Query(value = """
        SELECT YEAR(order_date) as year,
               WEEK(order_date, 1) as week_number,
               MIN(DATE(order_date)) as week_start,
               MAX(DATE(order_date)) as week_end,
               SUM(total_amount) as revenue
        FROM orders
        WHERE order_date BETWEEN :startDate AND :endDate
         AND status = 'PAID'
        GROUP BY YEAR(order_date), WEEK(order_date, 1)
        ORDER BY year, week_number
        """, nativeQuery = true)
    List<Object[]> getWeeklyRevenue(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);

    // ===== REVENUE BY MONTH =====

    @Query(value = """
        SELECT YEAR(order_date) as year,
               MONTH(order_date) as month,
               SUM(total_amount) as revenue
        FROM orders
        WHERE order_date BETWEEN :startDate AND :endDate
         AND status = 'PAID'
        GROUP BY YEAR(order_date), MONTH(order_date)
        ORDER BY year, month
        """, nativeQuery = true)
    List<Object[]> getMonthlyRevenue(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);

    // ===== REVENUE BY YEAR =====

    @Query(value = """
        SELECT YEAR(order_date) as year,
               SUM(total_amount) as revenue
        FROM orders
        WHERE order_date BETWEEN :startDate AND :endDate
         AND status = 'PAID'
        GROUP BY YEAR(order_date)
        ORDER BY year
        """, nativeQuery = true)
    List<Object[]> getYearlyRevenue(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);

    // ===== PRODUCT SALES BY DATE RANGE =====

    @Query(value = """
        SELECT p.product_id,
               p.product_name,
               SUM(oi.quantity) as total_quantity,
               SUM(oi.quantity * oi.unit_price) as total_revenue
        FROM order_items oi
        JOIN orders o ON oi.order_id = o.order_id
        JOIN products p ON oi.product_id = p.product_id
        WHERE o.order_date BETWEEN :startDate AND :endDate
         AND o.status = 'PAID'
        GROUP BY p.product_id, p.product_name
        ORDER BY total_quantity DESC
        """, nativeQuery = true)
    List<Object[]> getProductSales(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);

    // ===== DAILY PRODUCT SALES =====

    @Query(value = """
        SELECT DATE(o.order_date) as date,
               p.product_id,
               p.product_name,
               SUM(oi.quantity) as quantity_sold
        FROM order_items oi
        JOIN orders o ON oi.order_id = o.order_id
        JOIN products p ON oi.product_id = p.product_id
        WHERE o.order_date BETWEEN :startDate AND :endDate
          AND o.status = 'PAID'
        GROUP BY DATE(o.order_date), p.product_id, p.product_name
        ORDER BY date, quantity_sold DESC
        """, nativeQuery = true)
    List<Object[]> getDailyProductSales(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate);


    // ===== WEEK PRODUCT SALES =====

    @Query(value = """
    SELECT 
        YEAR(o.order_date) as year,
        WEEK(o.order_date, 1) as week_number,
        p.product_id,
        p.product_name,
        SUM(oi.quantity) as quantity_sold,
        SUM(oi.quantity * oi.unit_price) as revenue
    FROM order_items oi
    JOIN orders o ON oi.order_id = o.order_id
    JOIN products p ON oi.product_id = p.product_id
    WHERE o.order_date BETWEEN :startDate AND :endDate
      AND o.status = 'PAID'
    GROUP BY 
        YEAR(o.order_date),
        WEEK(o.order_date, 1),
        p.product_id,
        p.product_name
    ORDER BY year, week_number, quantity_sold DESC
    """, nativeQuery = true)
    List<Object[]> getWeeklyProductSales(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate
    );

    // ===== MONTH PRODUCT SALES =====

    @Query(value = """
    SELECT 
        YEAR(o.order_date) as year,
        MONTH(o.order_date) as month,
        p.product_id,
        p.product_name,
        SUM(oi.quantity) as quantity_sold,
        SUM(oi.quantity * oi.unit_price) as revenue
    FROM order_items oi
    JOIN orders o ON oi.order_id = o.order_id
    JOIN products p ON oi.product_id = p.product_id
    WHERE o.order_date BETWEEN :startDate AND :endDate
      AND o.status = 'PAID'
    GROUP BY 
        YEAR(o.order_date),
        MONTH(o.order_date),
        p.product_id,
        p.product_name
    ORDER BY year, month, quantity_sold DESC
    """, nativeQuery = true)
    List<Object[]> getMonthlyProductSales(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate
    );


    // ===== YAER PRODUCT SALES =====

    @Query(value = """
    SELECT 
        YEAR(o.order_date) as year,
        p.product_id,
        p.product_name,
        SUM(oi.quantity) as quantity_sold,
        SUM(oi.quantity * oi.unit_price) as revenue
    FROM order_items oi
    JOIN orders o ON oi.order_id = o.order_id
    JOIN products p ON oi.product_id = p.product_id
    WHERE o.order_date BETWEEN :startDate AND :endDate
      AND o.status = 'PAID'
    GROUP BY 
        YEAR(o.order_date),
        p.product_id,
        p.product_name
    ORDER BY year, quantity_sold DESC
    """, nativeQuery = true)
    List<Object[]> getYearlyProductSales(
            @Param("startDate") Timestamp startDate,
            @Param("endDate") Timestamp endDate
    );




}
