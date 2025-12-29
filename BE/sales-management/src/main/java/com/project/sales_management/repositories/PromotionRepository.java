package com.project.sales_management.repositories;

import com.project.sales_management.models.DiscountType;
import com.project.sales_management.models.Promotion;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.time.LocalDate;

@Repository
public interface PromotionRepository extends JpaRepository<Promotion, Long> {
    boolean existsByPromotionCode(String promotionCode);

    @Query(
            value = "SELECT p FROM Promotion p WHERE " +
                    "(:discountType IS NULL OR p.discountType = :discountType) AND " +
                    "(:startDate IS NULL OR p.startDate >= :startDate) AND " +
                    "(:endDate IS NULL OR p.endDate <= :endDate) AND " +
                    "(:isActive IS NULL OR p.isActive = :isActive)"
    )
    Page<Promotion> findByCriteria(DiscountType discountType, LocalDate startDate, LocalDate endDate, Boolean isActive, Pageable pageable);
}
