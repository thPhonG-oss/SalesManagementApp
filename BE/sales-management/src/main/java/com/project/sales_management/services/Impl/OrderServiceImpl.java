package com.project.sales_management.services.Impl;

import com.lowagie.text.*;
import com.lowagie.text.pdf.PdfPTable;
import com.lowagie.text.pdf.PdfWriter;
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

import java.io.ByteArrayOutputStream;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
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
    OrderItemRepository orderItemRepository;
    CustomerService customerService;
    @Transactional
    @Override
    public OrderResponse createOrder(OrderRequest orderRequest) {
        Order order = orderMapper.toOrder(orderRequest);
        Customer customer=customerRepository.findByEmail(orderRequest.getEmail()).orElseThrow(()->new AppException(ErrorCode.CUSTOMER_NOT_EXIST));
        order.setCustomer(customer);
        Promotion promotion=promotionRepository.findById(orderRequest.getPromotionId())
                .orElseThrow(() -> new AppException(ErrorCode.PROMOTION_NOT_EXIST));
        order.setPromotion(promotion);
        order.setOrderCode(generateOrderCode());
        double orderSubTotal = 0.0;

        for (OrderItemRequest i : orderRequest.getOrderItems()) {
            Product product = productRepository.findById(i.getProductId())
                    .orElseThrow(() -> new AppException(ErrorCode.PRODUCT_NOT_EXIST));
            if(product.getIsDiscounted()==false){
                orderSubTotal += product.getPrice() * i.getQuantity();
            }else{
                orderSubTotal+=product.getSpecialPrice()*i.getQuantity();
            }

        }

        if (orderSubTotal < promotion.getMinOrderValue() ) {
            throw new AppException(ErrorCode.DISCOUNT_NOT_SUITABLE);
        }

        order.setSubTotal(0.0);
        order.setDiscountAmount(0.0);
        order.setTotalAmount(0.0);
        Order newOrder = orderRepository.save(order);
        orderRequest.getOrderItems().forEach(i -> {
            Product product=productRepository.findById(i.getProductId()).orElseThrow(()->new AppException(ErrorCode.PRODUCT_NOT_EXIST));
            Integer stockQuantity=product.getStockQuantity()- i.getQuantity();
            Integer soldQuantity= product.getSoldQuantity()+i.getQuantity();
            if(stockQuantity<0)
                throw new AppException(ErrorCode.OUT_OF_STOCK);
            product.setStockQuantity(stockQuantity);
            product.setSoldQuantity(soldQuantity);
            productRepository.save(product);
            Double unitPrice;
            Double discount;
            Double totalPrice;
            if(product.getIsDiscounted()==false){
                unitPrice=product.getPrice()*i.getQuantity();
                discount=0.0;
                totalPrice=unitPrice;
            }else{
                unitPrice=product.getPrice()*i.getQuantity();
                discount=(product.getPrice()-product.getSpecialPrice())*i.getQuantity();
                totalPrice=unitPrice-discount;
            }
            i.setTotalPrice(totalPrice);
            i.setDiscount(discount);
            i.setUnitPrice(unitPrice);

            OrderItem orderItem = orderItemService.createOrderItem(i, newOrder);
            orderItem.setOrder(newOrder);
            newOrder.getOrderItems().add(orderItem);
        });
        recalculateOrderTotals(newOrder);
        promotion.setUsedCount(promotion.getUsedCount()+1);
        promotionRepository.save(promotion);
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

    private void recalculateOrderTotals(Order order) {
        double subtotal = order.getOrderItems().stream()
                .mapToDouble(OrderItem::getTotalPrice)
                .sum();

        double discountAmount = 0.0;
        Promotion promotion = order.getPromotion();
        if (promotion != null) {
            if (DiscountType.PERCENTAGE.equals(promotion.getDiscountType())) {
                discountAmount = subtotal * (promotion.getDiscountValue() / 100.0);
            } else {
                discountAmount = promotion.getDiscountValue();
            }
        }


        double totalAmount = subtotal - discountAmount;

        order.setSubTotal(subtotal);
        order.setDiscountAmount(discountAmount);
        order.setTotalAmount(totalAmount);
        orderRepository.save(order);

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




    public byte[] generateInvoice(Long orderId) {

        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new AppException(ErrorCode.ORDER_NOT_FOUND));

        List<OrderItem> items = orderItemRepository.findByOrder_OrderId(orderId);

        ByteArrayOutputStream out = new ByteArrayOutputStream();
        Document document = new Document(PageSize.A4);
        PdfWriter.getInstance(document, out);

        document.open();

        // ===== FORMAT DATE =====
        DateTimeFormatter dateFormatter = DateTimeFormatter.ofPattern("dd/MM/yyyy");
        String orderDate = order.getOrderDate().format(dateFormatter);

        // ===== TIÊU ĐỀ =====
        Font titleFont = new Font(Font.HELVETICA, 18, Font.BOLD);
        Paragraph title = new Paragraph("HÓA ĐƠN BÁN HÀNG", titleFont);
        title.setAlignment(Element.ALIGN_CENTER);
        document.add(title);

        document.add(new Paragraph(" "));

        // ===== THÔNG TIN ĐƠN =====
        document.add(new Paragraph("Mã đơn: " + order.getOrderCode()));
        document.add(new Paragraph("Ngày: " + orderDate));
        document.add(new Paragraph("Khách hàng: " + order.getCustomer().getCustomerName()));
        document.add(new Paragraph("Địa chỉ giao hàng: " + order.getShippingAddress()));

        document.add(new Paragraph(" "));

        // ===== BẢNG SẢN PHẨM =====
        PdfPTable table = new PdfPTable(5);
        table.setWidthPercentage(100);

        table.addCell("Sản phẩm");
        table.addCell("SL");
        table.addCell("Đơn giá");
        table.addCell("Giảm");
        table.addCell("Thành tiền");

        for (OrderItem item : items) {
            table.addCell(item.getProduct().getProductName());
            table.addCell(String.valueOf(item.getQuantity()));
            table.addCell(String.valueOf(item.getUnitPrice()));
            table.addCell(String.valueOf(item.getDiscount()));
            table.addCell(String.valueOf(item.getTotalPrice()));
        }

        document.add(table);

        document.add(new Paragraph(" "));
        document.add(new Paragraph("Tổng tiền: " + order.getTotalAmount()));

        document.close();

        return out.toByteArray();
    }


}
