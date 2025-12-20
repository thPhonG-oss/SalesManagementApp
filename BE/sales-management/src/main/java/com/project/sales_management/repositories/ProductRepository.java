package com.project.sales_management.repositories;

import com.project.sales_management.models.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ProductRepository extends JpaRepository<Product, Long> {
    boolean existsByProductName(String productName);

    @Query(
            value = "SELECT p FROM Product p WHERE " +
                    "LOWER(p.productName) LIKE LOWER(CONCAT('%', :keyword, '%')) " +
                    "AND p.price BETWEEN :minPrice AND :maxPrice"
    )
    Page<Product> findByProductNameContainingIgnoreCase(String keyword,Double maxPrice, Double minPrice, Pageable pageable);

    long countByIsActiveTrue();

    // 2.Top 5 sản phẩm sắp hết hàng (stock < )
    @Query("""
    SELECT p FROM Product p
    WHERE p.stockQuantity < 15
      AND p.isActive = true
    ORDER BY p.stockQuantity ASC
    """)
    List<Product> findLowStockProducts(Pageable pageable);

    // 3.Top 5 sản phẩm bán chạy
    @Query(value = """
    SELECT p.*, COALESCE(SUM(oi.quantity), 0) as total_sold
    FROM products p
    LEFT JOIN order_items oi ON p.product_id = oi.product_id
    WHERE p.is_active = true
    GROUP BY p.product_id
    ORDER BY total_sold DESC
    LIMIT 5
    """, nativeQuery = true)
    List<Product> findTop5SellingProducts();

    @Query(
            value = "SELECT p FROM Product p WHERE " +
                    "p.category.categoryId = :categoryId " +
                    "AND p.price BETWEEN :minPrice AND :maxPrice"
    )
    Page<Product> findByCategory_CategoryIdAndPriceBetween(Long categoryId, int minPrice, int maxPrice, Pageable pageable);

    @Query(
            value = "SELECT p FROM Product p WHERE " +
                    "p.category.categoryId = :categoryId " +
                    "AND p.price BETWEEN :minPrice AND :maxPrice " +
                    "AND LOWER(p.productName) LIKE LOWER(CONCAT('%', :keyword, '%'))"
    )
    Page<Product> findByCategory_CategoryIdAndPriceBetweenAndProductNameContainingIgnoreCase(Long categoryId, int minPrice, int maxPrice, String keyword, Pageable pageable);
}
