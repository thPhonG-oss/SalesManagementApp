package com.project.sales_management.models;

import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "customers")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Customer {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "customer_id")
    Long customerId;

    @Column(name = "customer_name", nullable = false)
    String customerName;

    @Column(name = "phone", nullable = false, unique = true)
    String phone;

    @Column(name = "email", nullable = false, unique = true)
    String email;

    @Column(name = "address")
    String address;

    @Column(name = "total_orders")
    Integer totalOrder;

    @Column(name = "total_spent")
    Double totalSpent;

    @Column(name = "last_order_date")
    LocalDate lastOrderDate;

    @Column(name = "created_at")
    LocalDateTime createdAt;

    @Column(name = "updated_at")
    LocalDateTime updatedAt;

    @OneToMany(
        mappedBy = "customer",
        cascade = {CascadeType.PERSIST, CascadeType.MERGE},
        orphanRemoval = true
    )
    List<Order> orders = new ArrayList<>();
}
