package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.OrderItemUpdateRequest;
import com.project.sales_management.dtos.requests.OrderUpdateRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.models.Order;
import com.project.sales_management.models.OrderItem;
import org.mapstruct.*;

@Mapper(componentModel = "spring", uses = {CustomerMapper.class, OrderItemMapper.class, PromotionMapper.class})
public interface OrderMapper {
    OrderResponse toOrderResponse(Order order);

    @BeanMapping(nullValuePropertyMappingStrategy = NullValuePropertyMappingStrategy.IGNORE)
    @Mapping(target = "orderItems", ignore = true)
    void updateOrder(@MappingTarget Order order, OrderUpdateRequest orderUpdateRequest);
}
