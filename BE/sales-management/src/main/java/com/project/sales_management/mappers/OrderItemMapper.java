package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.dtos.requests.OrderItemUpdateRequest;
import com.project.sales_management.dtos.responses.OrderItemResponse;
import com.project.sales_management.models.Order;
import com.project.sales_management.models.OrderItem;
import org.mapstruct.*;

@Mapper(componentModel = "spring", uses = {ProductMapper.class})
public interface OrderItemMapper {
    OrderItemResponse toOrderItemResponse(OrderItem orderItem);
    OrderItem toOrderItem(OrderItemRequest orderItemRequest);



    @Mapping(target = "orderItemId", ignore = true)
    @Mapping(target = "order", ignore = true) // Set manual
    @Mapping(target = "product", ignore = true) // Set manual
    @Mapping(target = "discount", expression = "java(itemRequest.getDiscount() != null ? itemRequest.getDiscount() : 0.0)")
    OrderItem toOrderItem2(OrderItemUpdateRequest itemRequest);

    @BeanMapping(nullValuePropertyMappingStrategy = NullValuePropertyMappingStrategy.IGNORE)
    @Mapping(target = "orderItemId", ignore = true)
    @Mapping(target = "order", ignore = true)
    @Mapping(target = "product", ignore = true)
    void updateOrderItem(@MappingTarget OrderItem orderItem, OrderItemUpdateRequest itemRequest);
}
