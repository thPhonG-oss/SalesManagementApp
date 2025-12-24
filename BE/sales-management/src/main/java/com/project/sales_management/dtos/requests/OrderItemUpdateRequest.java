package com.project.sales_management.dtos.requests;

import jakarta.validation.constraints.Min;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class OrderItemUpdateRequest {
    private Long orderItemId;
    private Long productId;
    @Min(value = 1, message = "Quantity must be at least 1")
    private Integer quantity;
    private Double unitPrice;
    private Double discount;
    private Double totalPrice;
    private Boolean isDeleted;
}
