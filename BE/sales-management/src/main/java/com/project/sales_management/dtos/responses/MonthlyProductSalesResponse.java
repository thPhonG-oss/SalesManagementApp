package com.project.sales_management.dtos.responses;

import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class MonthlyProductSalesResponse {
    private int year;
    private int month;
    private String monthName;
    private Long productId;
    private String productName;
    private int quantitySold;
    private double revenue;

}
