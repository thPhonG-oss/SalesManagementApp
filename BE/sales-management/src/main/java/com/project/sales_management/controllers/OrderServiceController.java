package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.services.OrderService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/orders")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class OrderServiceController {
    OrderService orderService;
    @PostMapping
    public ResponseEntity<ApiResponse<OrderResponse>>createOrder(@RequestBody OrderRequest orderRequest){
        return ResponseEntity.ok(ApiResponse.<OrderResponse>builder()
                        .message("create order successfully")
                        .data(orderService.createOrder(orderRequest))
                .build());
    }
}
