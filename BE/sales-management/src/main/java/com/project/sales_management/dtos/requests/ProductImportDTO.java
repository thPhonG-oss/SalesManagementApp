package com.project.sales_management.dtos.requests;


import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ProductImportDTO {
    String categoryName;
    String productName;
    String description;
    String author;
    String publisher;
    Integer publicationYear;
    Double price;
    Integer stockQuantity;
    Integer minStockQuantity;
    Double discountPercentage;
}
