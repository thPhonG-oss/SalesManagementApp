package com.project.sales_management.dtos.responses;

import lombok.*;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class MonthlyRevenueResponse {
    private Integer year;
    private Integer month;
    private String monthName;
    private Double revenue;
}
