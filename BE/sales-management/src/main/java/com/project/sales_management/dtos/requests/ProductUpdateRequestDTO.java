package com.project.sales_management.dtos.requests;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ProductUpdateRequestDTO {
    @NotNull(message = "Category ID cannot be null")
    Long categoryId;

    @NotBlank(message = "Product name cannot be blank")
    String productName;

    String description;
    String author;
    String publisher;
    Integer publicationYear;

    @NotNull(message = "Price cannot be null")
    Double price;
    Integer stockQuantity;
    Integer minStockQuantity;
}
