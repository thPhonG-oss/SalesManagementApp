package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.OrderItemResponse;
import com.project.sales_management.models.OrderItem;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring", uses = {ProductMapper.class})
public interface OrderItemMapper {
    OrderItemResponse toOrderItemResponse(OrderItem orderItem);
}
