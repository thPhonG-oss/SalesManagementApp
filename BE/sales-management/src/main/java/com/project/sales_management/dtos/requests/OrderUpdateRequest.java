package com.project.sales_management.dtos.requests;

import com.project.sales_management.models.OrderStatus;
import com.project.sales_management.models.PaymentMethod;
import jakarta.validation.Valid;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class OrderUpdateRequest {
    private Long promotionId;
    private OrderStatus status;
    private String shippingAddress;
    private String notes;
    private PaymentMethod paymentMethod;

    @Valid
    private List<OrderItemUpdateRequest> orderItems;
}
