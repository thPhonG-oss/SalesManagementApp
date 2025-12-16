package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.OrderMapper;
import com.project.sales_management.models.Order;
import com.project.sales_management.repositories.CustomerRepository;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.repositories.PromotionRepository;
import com.project.sales_management.services.CustomerService;
import com.project.sales_management.services.OrderItemService;
import com.project.sales_management.services.OrderService;
import jakarta.persistence.PreUpdate;
import jakarta.transaction.Transactional;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
@Slf4j
@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class OrderServiceImpl implements OrderService {
    OrderRepository orderRepository;
    OrderMapper orderMapper;
    CustomerRepository customerRepository;
    PromotionRepository promotionRepository;
    OrderItemService orderItemService;
    @Override
    public OrderResponse createOrder(OrderRequest orderRequest) {
        Order order = orderMapper.toOrder(orderRequest);
        order.setCustomer(customerRepository.findById(orderRequest.getCustomerId())
                .orElseThrow(() -> new AppException(ErrorCode.CUSTOMER_NOT_EXIST)));
        order.setPromotion(promotionRepository.findById(orderRequest.getPromotionId())
                .orElseThrow(() -> new AppException(ErrorCode.PROMOTION_NOT_EXIST)));
        order.setOrderCode(generateOrderCode());
        Order newOrder = orderRepository.save(order);
        orderRequest.getOrderItems().forEach(i -> {
            orderItemService.createOrderItem(i,newOrder);
        });

        return orderMapper.toOrderResponse(newOrder);
    }
    public String generateOrderCode() {
        long count = orderRepository.count() + 1;
        return String.format("ORD%05d", count);
    }
}
