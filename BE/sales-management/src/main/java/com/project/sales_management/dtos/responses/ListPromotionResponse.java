package com.project.sales_management.dtos.responses;

import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.ArrayList;
import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ListPromotionResponse {
    List<PromotionResponse> promotions = new ArrayList<>();
    int page;
    int size;
    int totalPages;
    long totalElements;
    boolean isLastPage;
}
