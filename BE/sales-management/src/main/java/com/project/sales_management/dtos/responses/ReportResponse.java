package com.project.sales_management.dtos.responses;

import lombok.*;

import java.util.List;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ReportResponse {
    // Revenue reports
    private List<DailyRevenueResponse> dailyRevenue;
    private List<WeeklyRevenueResponse> weeklyRevenue;
    private List<MonthlyRevenueResponse> monthlyRevenue;
    private List<YearlyRevenueResponse> yearlyRevenue;

    // Product sales reports
    private List<ProductSalesResponse> topProducts;
    private List<DailyProductSalesResponse> dailyProductSales;
}
