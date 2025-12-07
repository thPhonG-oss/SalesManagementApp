package com.project.sales_management.models;

import jakarta.persistence.*;
import lombok.*;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "products")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "product_id")
    Long productId;

    @ManyToOne(
            fetch = FetchType.EAGER
    )
    @JoinColumn(name = "category_id", nullable = false)
    Category category;

    @Column(name = "product_name", nullable = false)
    String productName;

    @Column(name = "description")
    String description;

    @Column(name = "author")
    String author;

    @Column(name = "publisher")
    String publisher;

    @Column(name = "publication_year")
    Integer publicationYear;

    @Column(name = "price", nullable = false)
    Double price;

    @Column(name = "stock_quantity", nullable = false)
    Integer stockQuantity;

    @Column(name = "min_stock_quantity", nullable = false)
    Integer minStockQuantity;

    @Column(name = "sold_quantity", nullable = false)
    Integer soldQuantity;

    @Column(name = "is_active")
    Boolean isActive;

    @Column(name = "discount_percentage")
    Double discountPercentage;

    @Column(name = "is_discounted")
    Boolean isDiscounted;

    @Column(name = "special_price")
    Double specialPrice;

    @Column(name = "created_at")
    LocalDateTime createdAt;

    @Column(name = "updated_at")
    LocalDateTime updatedAt;

    @OneToMany(
        mappedBy = "product",
        cascade = {CascadeType.PERSIST, CascadeType.MERGE},
        orphanRemoval = true
    )
    List<ProductImage> productImages = new ArrayList<>();
}
