package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.OrderItemMapper;
import com.project.sales_management.models.Order;
import com.project.sales_management.models.OrderItem;
import com.project.sales_management.models.Product;
import com.project.sales_management.repositories.OrderItemRepository;
import com.project.sales_management.repositories.ProductRepository;
import com.project.sales_management.services.OrderItemService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class OrderItemServiceImpl implements OrderItemService {
    OrderItemRepository orderItemRepository;
    OrderItemMapper orderItemMapper;
    ProductRepository productRepository;
    public OrderItem createOrderItem(OrderItemRequest orderItemRequest, Order order){
        OrderItem orderItem=orderItemMapper.toOrderItem(orderItemRequest);
        Product product=productRepository.findById(orderItemRequest.getProductId()).orElseThrow(()->new AppException(ErrorCode.PRODUCT_NOT_EXIST));
        orderItem.setProduct(product);
        orderItem.setOrder(order);
        return orderItemRepository.save(orderItem);
    }
}
