package com.project.sales_management.services;

import com.project.sales_management.dtos.responses.*;

import java.time.LocalDate;
import java.util.List;

public interface ReportService {
    List<DailyRevenueResponse> getDailyRevenue(LocalDate startDate, LocalDate endDate);

    List<WeeklyRevenueResponse> getWeeklyRevenue(LocalDate startDate, LocalDate endDate);

    List<MonthlyRevenueResponse> getMonthlyRevenue(LocalDate startDate, LocalDate endDate);

    List<YearlyRevenueResponse> getYearlyRevenue(LocalDate startDate, LocalDate endDate);

    List<ProductSalesResponse> getProductSales(LocalDate startDate, LocalDate endDate);
    List<DailyProductSalesResponse> getDailyProductSales(LocalDate startDate, LocalDate endDate);
    ReportResponse getCurrentMonthDashboard();
    List<WeeklyProductSalesResponse> getWeeklyProductSales(LocalDate startDate, LocalDate endDate);
    List<MonthlyProductSalesResponse> getMonthlyProductSales(LocalDate startDate, LocalDate endDate);
    List<YearlyProductSalesResponse> getYearlyProductSales(LocalDate startDate, LocalDate endDate);


}