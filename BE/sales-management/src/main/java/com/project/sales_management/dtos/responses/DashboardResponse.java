package com.project.sales_management.dtos.responses;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class DashboardResponse {

    Long totalProducts;
    List<ProductResponse> lowStockProducts;
    List<ProductResponse> topSellingProducts;
    Long todayOrderCount;
    Double todayRevenue;
    List<OrderResponse> recentOrders;
    List<DailyRevenueResponse> dailyRevenue;
}
