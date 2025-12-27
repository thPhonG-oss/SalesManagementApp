package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.OrderItemRequest;
import com.project.sales_management.dtos.requests.OrderRequest;
import com.project.sales_management.dtos.requests.OrderStatusUpdateRequest;
import com.project.sales_management.dtos.requests.OrderUpdateRequest;
import com.project.sales_management.dtos.responses.OrderResponse;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.OrderMapper;
import com.project.sales_management.models.Order;
import com.project.sales_management.repositories.*;
import com.project.sales_management.services.CustomerService;
import com.project.sales_management.services.OrderItemService;
import com.project.sales_management.models.*;
import com.project.sales_management.repositories.OrderRepository;
import com.project.sales_management.repositories.PromotionRepository;
import com.project.sales_management.services.OrderService;
import jakarta.persistence.criteria.Predicate;
import jakarta.transaction.Transactional;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.stereotype.Service;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class OrderServiceImpl implements OrderService {
    OrderRepository orderRepository;
    OrderMapper orderMapper;
    CustomerRepository customerRepository;
    PromotionRepository promotionRepository;
    OrderItemService orderItemService;
    ProductRepository productRepository;

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
            Product product=productRepository.findById(i.getProductId()).orElseThrow(()->new AppException(ErrorCode.PRODUCT_NOT_EXIST));
            Integer stockQuantity=product.getStockQuantity()- i.getQuantity();
            Integer soldQuantity= product.getSoldQuantity()+i.getQuantity();
            if(stockQuantity<0)
                throw new AppException(ErrorCode.OUT_OF_STOCK);
            product.setStockQuantity(stockQuantity);
            product.setSoldQuantity(soldQuantity);
            productRepository.save(product);
        });

        return orderMapper.toOrderResponse(newOrder);
    }
    public String generateOrderCode() {
        long count = orderRepository.count() + 1;
        return String.format("ORD%05d", count);
    }
    

    @Override
    public Page<OrderResponse> getAllOrders(Integer pageNumber, Integer pageSize,
                                            OrderStatus status, LocalDateTime fromDate, LocalDateTime toDate) {
        Pageable pageable = PageRequest.of(pageNumber, pageSize, Sort.by("createdAt").descending());

        // Build Specification dynamically
        Specification<Order> spec = (root, query, criteriaBuilder) -> {
            List<Predicate> predicates = new ArrayList<>();

            // Filter by status
            if (status != null) {
                predicates.add(criteriaBuilder.equal(root.get("status"), status));
            }

            // Filter by fromDate (createdAt >= fromDate)
            if (fromDate != null) {
                predicates.add(criteriaBuilder.greaterThanOrEqualTo(root.get("orderDate"), fromDate));
            }

            // Filter by toDate (createdAt <= toDate)
            if (toDate != null) {
                predicates.add(criteriaBuilder.lessThanOrEqualTo(root.get("orderDate"), toDate));
            }

            return criteriaBuilder.and(predicates.toArray(new Predicate[0]));
        };

        Page<Order> orderPage = orderRepository.findAll(spec, pageable);
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

        // Chỉ cho phép update order có status
        if (order.getStatus() != OrderStatus.CREATED && order.getStatus() != OrderStatus.PAID) {
            throw new AppException(ErrorCode.INVALID_ORDER_STATUS);
        }

        orderMapper.updateOrder(order, orderUpdateRequest);

        // 4. Set updatedAt
        order.setUpdatedAt(LocalDateTime.now());

        // 5. Save order
        Order updatedOrder = orderRepository.save(order);
        return orderMapper.toOrderResponse(updatedOrder);
    }


    @Override
    public OrderResponse deleteOrder(Long orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));

        // Chỉ cho phép xóa order CREATED vaf PAID
        if (order.getStatus() != OrderStatus.CREATED && order.getStatus() != OrderStatus.PAID) {

            throw new AppException(ErrorCode.INVALID_DELETE_ORDER_STATUS);
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
