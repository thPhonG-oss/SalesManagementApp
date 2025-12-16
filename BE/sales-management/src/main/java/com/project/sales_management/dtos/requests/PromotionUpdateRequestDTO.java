package com.project.sales_management.dtos.requests;

import com.project.sales_management.models.DiscountType;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class PromotionUpdateRequestDTO {
    String promotionName;
    String description;
    DiscountType discountType;
    double discountValue;
    double minOrderValue;
    double maxDiscountValue;
    Integer usageLimit;
    boolean isActive;
}
