package com.project.sales_management.dtos.responses;

import lombok.*;

import java.time.LocalDate;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class DailyProductSalesResponse {
    private LocalDate date;
    private Long productId;
    private String productName;
    private Integer quantitySold;
}
