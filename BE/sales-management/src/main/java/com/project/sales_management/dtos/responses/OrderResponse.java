package com.project.sales_management.dtos.responses;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.project.sales_management.models.OrderStatus;
import com.project.sales_management.models.PaymentMethod;
import lombok.*;
import lombok.experimental.FieldDefaults;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class OrderResponse {
    Long orderId;
    String orderCode;
    LocalDateTime orderDate;
    OrderStatus status;
    Double subTotal;
    Double discountAmount;
    Double totalAmount;
    String notes;
    String shippingAddress;
    PaymentMethod paymentMethod;
    LocalDateTime createdAt;
    LocalDateTime updatedAt;
    List<OrderItemResponse> orderItems = new ArrayList<>();
}
