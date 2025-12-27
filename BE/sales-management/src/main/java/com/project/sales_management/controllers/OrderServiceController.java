package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.requests.OrderStatusUpdateRequest;
import com.project.sales_management.dtos.requests.OrderUpdateRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.models.OrderStatus;
import com.project.sales_management.services.OrderService;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

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


    @GetMapping
    public ResponseEntity<Page<OrderResponse>> getAllOrders(
            @RequestParam(defaultValue = "0") Integer pageNumber,
            @RequestParam(defaultValue = "10") Integer pageSize,
            @RequestParam(required = false) OrderStatus status,
            @RequestParam(required = false)
            @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate fromDate,
            @RequestParam(required = false)
            @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate toDate) {

        LocalDateTime fromDateTime = fromDate != null ? fromDate.atStartOfDay() : null;
        LocalDateTime toDateTime = toDate != null ? toDate.atTime(23, 59, 59) : null;
        Page<OrderResponse> orderResponses = orderService.getAllOrders(pageNumber, pageSize, status, fromDateTime, toDateTime);
        return ResponseEntity.ok(orderResponses);
    }

    @GetMapping("/{orderId}")
    public ResponseEntity<OrderResponse> getOrderById(@PathVariable Long orderId) {
        OrderResponse orderResponse = orderService.getOrderById(orderId);
        return ResponseEntity.ok(orderResponse);
    }

    @PutMapping("/{orderId}")
    public ResponseEntity<OrderResponse> updateOrder(
            @PathVariable Long orderId,
            @Valid @RequestBody OrderUpdateRequest orderUpdateRequest) {
        OrderResponse orderResponse = orderService.updateOrder(orderId, orderUpdateRequest);
        return ResponseEntity.ok(orderResponse);
    }

    @DeleteMapping("/{orderId}")
    public ResponseEntity<OrderResponse> deleteOrder(@PathVariable Long orderId) {
        OrderResponse orderResponse = orderService.deleteOrder(orderId);
        return ResponseEntity.ok(orderResponse);
    }

    @PatchMapping("/{orderId}/status")
    public ResponseEntity<OrderResponse> updateOrderStatus(
            @PathVariable Long orderId,
            @Valid @RequestBody OrderStatusUpdateRequest request) {

        OrderResponse orderResponse = orderService.updateOrderStatus(orderId, request);
        return ResponseEntity.ok(orderResponse);
    }
}
