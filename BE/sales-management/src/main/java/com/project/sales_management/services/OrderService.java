package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.OrderResponse;

public interface OrderService {
    OrderResponse createOrder(OrderRequest orderRequest);

}
