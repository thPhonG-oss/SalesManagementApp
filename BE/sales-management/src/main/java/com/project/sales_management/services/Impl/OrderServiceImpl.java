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

        // Chỉ cho phép update order có status PENDING
        if (order.getStatus() != OrderStatus.CREATED) {
            throw new IllegalArgumentException("Can only update orders with CREATED status. Current status: " + order.getStatus());
        }

        // 1. Update Promotion (nếu có)
        if (orderUpdateRequest.getPromotionId() != null) {
            Promotion promotion = promotionRepository.findById(orderUpdateRequest.getPromotionId())
                    .orElseThrow(() -> new IllegalArgumentException("Promotion not found with id: " + orderUpdateRequest.getPromotionId()));

            LocalDate today = LocalDate.now();
            if (!promotion.getIsActive()
                    || promotion.getStartDate().isAfter(today)
                    || promotion.getEndDate().isBefore(today)) {
                throw new IllegalArgumentException("Promotion is not valid or expired");
            }


            order.setPromotion(promotion);
        }

        orderMapper.updateOrder(order, orderUpdateRequest);

        // 3. Update Order Items (nếu có)
        if (orderUpdateRequest.getOrderItems() != null && !orderUpdateRequest.getOrderItems().isEmpty()) {
            orderItemService.updateOrderItems(order, orderUpdateRequest.getOrderItems());
        }

        // Recalculate totals => tính toán lại
        recalculateOrderTotals(order);

        // 4. Set updatedAt
        order.setUpdatedAt(LocalDateTime.now());

        // 5. Save order
        Order updatedOrder = orderRepository.save(order);

        return orderMapper.toOrderResponse(updatedOrder);
    }

    private void recalculateOrderTotals(Order order) {
        double subtotal = order.getOrderItems().stream()
                .mapToDouble(OrderItem::getTotalPrice)
                .sum();

        double discountAmount = 0.0;
        Promotion promotion = order.getPromotion();
        if (promotion != null) {
            if ("PERCENTAGE".equals(promotion.getDiscountType())) {
                discountAmount = subtotal * (promotion.getDiscountValue() / 100.0);
            } else if ("FIXED".equals(promotion.getDiscountType())) {
                discountAmount = promotion.getDiscountValue();
            }
        }

        double totalAmount = subtotal - discountAmount;

        order.setSubTotal(subtotal);
        order.setDiscountAmount(discountAmount);
        order.setTotalAmount(totalAmount);
    }

    @Override
    public OrderResponse deleteOrder(Long orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));

        // Chỉ cho phép xóa order CREATED
        if (order.getStatus() != OrderStatus.CREATED) {
            throw new IllegalArgumentException(
                    "Can only delete orders with CREATED status. Current status: " + order.getStatus()
            );
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
