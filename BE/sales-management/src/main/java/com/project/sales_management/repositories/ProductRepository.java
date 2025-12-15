package com.project.sales_management.repositories;

import com.project.sales_management.models.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

@Repository
public interface ProductRepository extends JpaRepository<Product, Long> {
    boolean existsByProductName(String productName);

    @Query(
            value = "SELECT p FROM Product p WHERE " +
                    "LOWER(p.productName) LIKE LOWER(CONCAT('%', :keyword, '%')) " +
                    "AND p.price BETWEEN :minPrice AND :maxPrice"
    )
    Page<Product> findByProductNameContainingIgnoreCase(String keyword,Double maxPrice, Double minPrice, Pageable pageable);
}
