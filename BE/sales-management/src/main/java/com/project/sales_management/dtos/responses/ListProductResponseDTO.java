package com.project.sales_management.dtos.responses;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.ArrayList;
import java.util.List;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class ListProductResponseDTO {
    List<ProductResponse> products = new ArrayList<>();
    int page;
    int size;
    int totalPages;
    long totalElements;
    boolean isLastPage;
}
