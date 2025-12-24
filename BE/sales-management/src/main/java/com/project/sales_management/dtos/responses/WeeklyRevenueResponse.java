package com.project.sales_management.dtos.responses;

import lombok.*;

import java.time.LocalDate;


@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class WeeklyRevenueResponse {
    private Integer year;
    private Integer weekNumber;
    private LocalDate weekStart;
    private LocalDate weekEnd;
    private Double revenue;
}
