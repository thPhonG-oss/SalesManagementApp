package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.OrderItemUpdateRequest;
import com.project.sales_management.models.Order;

import java.util.List;

public interface OrderItemService {
    void updateOrderItems(Order order, List<OrderItemUpdateRequest> itemRequests);
}
