package com.project.sales_management.dtos.requests;

import com.project.sales_management.models.DiscountType;
import jakarta.validation.constraints.NotNull;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class PromotionCreationRequestDTO {

    @NotNull(message = "Promotion code cannot be null")
    String promotionName;

    String description;

    @NotNull(message = "Discount type cannot be null")
    DiscountType discountType;

    @NotNull(message = "Discount value cannot be null")
    Double discountValue;

    Double minOrderValue;

    Double maxDiscountValue;

    @NotNull(message = "Start date cannot be null")
    LocalDate startDate;

    @NotNull(message = "End date cannot be null")
    LocalDate endDate;

    Integer usageLimit;
}
