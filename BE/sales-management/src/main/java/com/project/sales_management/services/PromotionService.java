package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.PromotionCreationRequestDTO;
import com.project.sales_management.dtos.requests.PromotionUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ListPromotionResponse;
import com.project.sales_management.dtos.responses.PromotionResponse;
import com.project.sales_management.models.DiscountType;
import jakarta.transaction.Transactional;

import java.time.LocalDate;

public interface PromotionService {
    @Transactional
    PromotionResponse createPromotion(PromotionCreationRequestDTO promotionCreationRequestDTO);

    ListPromotionResponse getAllPromotions(int page, int size, String sortBy, String sortDir);

    PromotionResponse deactivatePromotion(Long promotionId);

    PromotionResponse getPromotionById(Long promotionId);

    PromotionResponse updatePromotion(Long promotionId, PromotionUpdateRequestDTO promotionUpdateRequestDTO);

    ListPromotionResponse searchPromotions(
            DiscountType discountType,
            LocalDate startDate,
            LocalDate endDate,
            Boolean isActive,
            int page,
            int size,
            String sortBy,
            String sortDir
    );
}
