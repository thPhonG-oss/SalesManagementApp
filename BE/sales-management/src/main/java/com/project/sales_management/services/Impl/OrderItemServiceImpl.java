package com.project.sales_management.services.Impl;

import com.project.sales_management.models.OrderItem;
import com.project.sales_management.repositories.OrderItemRepository;
import com.project.sales_management.services.OrderItemService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class OrderItemServiceImpl implements OrderItemService {
    OrderItemRepository orderItemRepository;
}
