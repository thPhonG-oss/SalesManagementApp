package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.OrderStatusUpdateRequest;
import com.project.sales_management.dtos.requests.OrderUpdateRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.OrderMapper;
import com.project.sales_management.models.*;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.repositories.PromotionRepository;
import com.project.sales_management.services.OrderService;
import jakarta.persistence.PreUpdate;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class OrderServiceImpl implements OrderService {
    OrderRepository orderRepository;
    OrderMapper orderMapper;
    PromotionRepository promotionRepository; //
    OrderItemServiceImpl orderItemService;

    @Override
    public Page<OrderResponse> getAllOrders(Integer pageNumber, Integer pageSize) {
        Pageable pageable = PageRequest.of(pageNumber, pageSize, Sort.by("orderDate").descending());
        Page<Order> orderPage = orderRepository.findAll(pageable);
        return orderPage.map(orderMapper::toOrderResponse);
    }

    @Override
    public OrderResponse getOrderById(Long orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));
        return orderMapper.toOrderResponse(order);
    }

    @Override
    public OrderResponse updateOrder(Long orderId, OrderUpdateRequest orderUpdateRequest) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() ->new AppException(ErrorCode.ORDER_NOT_FOUND));

        // Chỉ cho phép update order có status PENDING
        if (order.getStatus() != OrderStatus.CREATED) {
            throw new IllegalArgumentException("Can only update orders with CREATED status. Current status: " + order.getStatus());
        }

        // 1. Update Promotion (nếu có)
        if (orderUpdateRequest.getPromotionId() != null) {
            Promotion promotion = promotionRepository.findById(orderUpdateRequest.getPromotionId())
                    .orElseThrow(() -> new IllegalArgumentException("Promotion not found with id: " + orderUpdateRequest.getPromotionId()));

            LocalDate today = LocalDate.now();
            if (!promotion.getIsActive()
                    || promotion.getStartDate().isAfter(today)
                    || promotion.getEndDate().isBefore(today)) {
                throw new IllegalArgumentException("Promotion is not valid or expired");
            }


            order.setPromotion(promotion);
        }

        orderMapper.updateOrder(order, orderUpdateRequest);

        // 3. Update Order Items (nếu có)
        if (orderUpdateRequest.getOrderItems() != null && !orderUpdateRequest.getOrderItems().isEmpty()) {
            orderItemService.updateOrderItems(order, orderUpdateRequest.getOrderItems());
        }

        // Recalculate totals => tính toán lại
        recalculateOrderTotals(order);

        // 4. Set updatedAt
        order.setUpdatedAt(LocalDateTime.now());

        // 5. Save order
        Order updatedOrder = orderRepository.save(order);

        return orderMapper.toOrderResponse(updatedOrder);
    }

    private void recalculateOrderTotals(Order order) {
        double subtotal = order.getOrderItems().stream()
                .mapToDouble(OrderItem::getTotalPrice)
                .sum();

        double discountAmount = 0.0;
        Promotion promotion = order.getPromotion();
        if (promotion != null) {
            if ("PERCENTAGE".equals(promotion.getDiscountType())) {
                discountAmount = subtotal * (promotion.getDiscountValue() / 100.0);
            } else if ("FIXED".equals(promotion.getDiscountType())) {
                discountAmount = promotion.getDiscountValue();
            }
        }

        double totalAmount = subtotal - discountAmount;

        order.setSubTotal(subtotal);
        order.setDiscountAmount(discountAmount);
        order.setTotalAmount(totalAmount);
    }

    @Override
    public OrderResponse deleteOrder(Long orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));

        // Chỉ cho phép xóa order CREATED
        if (order.getStatus() != OrderStatus.CREATED) {
            throw new IllegalArgumentException(
                    "Can only delete orders with CREATED status. Current status: " + order.getStatus()
            );
        }

        // Hoàn lại stock cho product
        order.getOrderItems().forEach(item -> {
            Product product = item.getProduct();
            product.setStockQuantity(
                    product.getStockQuantity() + item.getQuantity()
            );
        });

        // Map response trước khi xóa (để trả về)
        OrderResponse response = orderMapper.toOrderResponse(order);

        // Xóa order (cascade sẽ xóa orderItems)
        orderRepository.delete(order);

        return response;
    }

    @Override
    public OrderResponse updateOrderStatus(Long orderId, OrderStatusUpdateRequest request) {

        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));

        order.setStatus(request.getStatus());

        return orderMapper.toOrderResponse(order);
    }
}
