package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.models.Order;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring", uses = {CustomerMapper.class, OrderItemMapper.class, PromotionMapper.class})
public interface OrderMapper {
    OrderResponse toOrderResponse(Order order);
}
