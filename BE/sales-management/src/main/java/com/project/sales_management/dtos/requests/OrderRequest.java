package com.project.sales_management.dtos.requests;

import com.project.sales_management.models.*;

import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDateTime;
import java.util.List;
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class OrderRequest {
    String email;
    Long promotionId;
    String notes;
    String shippingAddress;
    PaymentMethod paymentMethod;
    List<OrderItemRequest> orderItems;
}
