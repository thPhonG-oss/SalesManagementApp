package com.project.sales_management.dtos.responses;

import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class WeeklyProductSalesResponse {
    private int year;
    private int week;
    private Long productId;
    private String productName;
    private int quantitySold;
    private double revenue;
}
