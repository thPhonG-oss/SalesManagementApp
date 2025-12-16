package com.project.sales_management.dtos.responses;


import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class YearlyProductSalesResponse {


    private int year;
    private Long productId;
    private String productName;
    private int quantitySold;
    private double revenue;
}
