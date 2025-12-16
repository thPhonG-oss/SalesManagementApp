package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.OrderItemUpdateRequest;
import com.project.sales_management.mappers.OrderItemMapper;
import com.project.sales_management.models.Order;
import com.project.sales_management.models.OrderItem;
import com.project.sales_management.models.Product;
import com.project.sales_management.repositories.OrderItemRepository;
import com.project.sales_management.repositories.ProductRepository;
import com.project.sales_management.services.OrderItemService;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Objects;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class OrderItemServiceImpl implements OrderItemService {
    OrderItemRepository orderItemRepository;
    OrderItemMapper orderItemMapper;

    ProductRepository productRepository;


    @Transactional
    @Override
    public void updateOrderItems(Order order, List<OrderItemUpdateRequest> itemRequests) {

        for (OrderItemUpdateRequest itemRequest : itemRequests) {

            // CASE 1: DELETE
            if (Boolean.TRUE.equals(itemRequest.getIsDeleted())
                    && itemRequest.getOrderItemId() != null) {


                OrderItem itemToDelete = orderItemRepository.findById(itemRequest.getOrderItemId())
                        .filter(item -> item.getOrder().getOrderId().equals(order.getOrderId()))
                        .orElseThrow(() -> new IllegalArgumentException("Order item not found in this order"));

                Product product = itemToDelete.getProduct();
                product.setStockQuantity(product.getStockQuantity() + itemToDelete.getQuantity());

                order.getOrderItems().remove(itemToDelete);
                orderItemRepository.delete(itemToDelete);

                continue;
            }

            // CASE 2: UPDATE
            if (itemRequest.getOrderItemId() != null) {

//

                OrderItem existingItem = orderItemRepository.findById(itemRequest.getOrderItemId())
                        .filter(item -> item.getOrder().getOrderId().equals(order.getOrderId()))
                        .orElseThrow(() -> new IllegalArgumentException("Order item not found in this order"));

                Product product = existingItem.getProduct();
                int stockDiff = itemRequest.getQuantity() - existingItem.getQuantity();

                if (stockDiff > 0 && product.getStockQuantity() < stockDiff) {
                    throw new IllegalArgumentException("Insufficient stock");
                }

                product.setStockQuantity(product.getStockQuantity() - stockDiff);
                productRepository.save(product);
                orderItemMapper.updateOrderItem(existingItem, itemRequest);

                continue;
            }

            // CASE 3: ADD
            if (itemRequest.getProductId() != null) {

                Product product = productRepository.findById(itemRequest.getProductId())
                        .orElseThrow(() -> new IllegalArgumentException("Product not found"));

                if (product.getStockQuantity() < itemRequest.getQuantity()) {
                    throw new IllegalArgumentException("Insufficient stock");
                }

                OrderItem newItem = orderItemMapper.toOrderItem2(itemRequest);
                newItem.setOrder(order);
                newItem.setProduct(product);

                order.getOrderItems().add(newItem);
                product.setStockQuantity(product.getStockQuantity() - itemRequest.getQuantity());
                productRepository.save(product);
            }
        }
    }

}
