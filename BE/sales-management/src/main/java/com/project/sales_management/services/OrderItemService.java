package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.models.Order;
import com.project.sales_management.models.OrderItem;
import java.util.List;

public interface OrderItemService {
    OrderItem createOrderItem(OrderItemRequest orderItemRequest, Order order);
   void updateOrderItems(Order order, List<OrderItemUpdateRequest> itemRequests);
}
