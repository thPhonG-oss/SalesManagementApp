package com.project.sales_management.dtos.responses;

import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ProductSalesResponse {
    private Long productId;
    private String productName;
    private Integer quantitySold;
    private Double revenue;
}
