package com.project.sales_management.dtos.requests;

import com.project.sales_management.models.Product;
import jakarta.persistence.Column;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class OrderItemRequest {
    Long productId;
    Integer quantity;
    Double unitPrice;
    Double discount;
    Double totalPrice;
}
