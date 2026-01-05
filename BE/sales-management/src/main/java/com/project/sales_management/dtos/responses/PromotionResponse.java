package com.project.sales_management.dtos.responses;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.project.sales_management.models.DiscountType;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class PromotionResponse {
    Long promotionId;
    String promotionCode;
    String promotionName;
    String description;
    DiscountType discountType;
    Double discountPercentage;
    Double discountValue;
    Double minOrderValue;
    Double maxDiscountValue;
    String startDate;
    String endDate;
    Integer usageLimit;
    Integer usedCount;
    Boolean isActive;
    LocalDateTime createdAt;
    LocalDateTime updatedAt;
}
