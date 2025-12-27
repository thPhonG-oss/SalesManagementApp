package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.dtos.requests.OrderStatusUpdateRequest;
import com.project.sales_management.dtos.requests.OrderUpdateRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.models.OrderStatus;
import org.springframework.data.domain.Page;

import java.time.LocalDateTime;

public interface OrderService {
    OrderResponse createOrder(OrderRequest orderRequest);
    public Page<OrderResponse> getAllOrders(Integer pageNumber, Integer pageSize,
                                            OrderStatus status, LocalDateTime fromDate, LocalDateTime toDate);
    OrderResponse getOrderById(Long orderId);
    OrderResponse updateOrder(Long orderId, OrderUpdateRequest orderUpdateRequest);
    OrderResponse deleteOrder(Long orderId);
    OrderResponse updateOrderStatus(Long orderId, OrderStatusUpdateRequest request);
}