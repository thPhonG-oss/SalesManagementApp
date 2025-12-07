package com.project.sales_management.dtos.responses;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;
import java.util.List;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class ProductResponse {
    Long productId;
    Long categoryId;
    String categoryName;
    String productName;
    String description;
    String author;
    String publisher;
    Integer publicationYear;
    Double price;
    Integer stockQuantity;
    Integer minStockQuantity;
    Integer soldQuantity;
    Boolean isActive;
    Double discountPercentage;
    Boolean isDiscounted;
    Double specialPrice;
    LocalDateTime createdAt;
    LocalDateTime updatedAt;
    List<ProductImageResponse> images;
}
