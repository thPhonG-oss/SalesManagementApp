package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.models.Order;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

@Mapper(componentModel = "spring", uses = {CustomerMapper.class, OrderItemMapper.class, PromotionMapper.class})
public interface OrderMapper {

    OrderResponse toOrderResponse(Order order);

    @Mapping(target = "createdAt", expression = "java(java.time.LocalDateTime.now())")
    @Mapping(target = "orderDate", expression = "java(java.time.LocalDateTime.now())")
    @Mapping(target = "updatedAt", expression = "java(java.time.LocalDateTime.now())")
    @Mapping(target = "orderItems", ignore = true)
    @Mapping(target = "status", expression = "java(com.project.sales_management.models.OrderStatus.CREATED)")
    Order toOrder(OrderRequest orderRequest);
}