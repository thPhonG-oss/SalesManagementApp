package com.project.sales_management.dtos.responses;

import lombok.*;



@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class YearlyRevenueResponse {
    private Integer year;
    private Double revenue;
}
