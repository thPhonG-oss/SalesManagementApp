package com.project.sales_management.services.Impl;

import com.project.sales_management.mappers.OrderMapper;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.services.OrderService;
import jakarta.persistence.PreUpdate;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class OrderServiceImpl implements OrderService {
    OrderRepository orderRepository;
    OrderMapper orderMapper;
}
