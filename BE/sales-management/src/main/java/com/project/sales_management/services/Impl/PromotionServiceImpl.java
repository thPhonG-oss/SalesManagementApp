package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.PromotionCreationRequestDTO;
import com.project.sales_management.dtos.requests.PromotionUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ListPromotionResponse;
import com.project.sales_management.dtos.responses.PromotionResponse;
import com.project.sales_management.mappers.PromotionMapper;
import com.project.sales_management.models.DiscountType;
import com.project.sales_management.models.Promotion;
import com.project.sales_management.repositories.PromotionRepository;
import com.project.sales_management.services.PromotionService;
import jakarta.transaction.Transactional;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class PromotionServiceImpl implements PromotionService {
    PromotionRepository promotionRepository;
    PromotionMapper promotionMapper;

    @Transactional
    @Override
    public PromotionResponse createPromotion(PromotionCreationRequestDTO promotionCreationRequestDTO) {
        Promotion promotion = promotionMapper.toPromotion(promotionCreationRequestDTO);

        promotion.setPromotionCode(generateUniquePromotionCode());
        promotion.setCreatedAt(LocalDateTime.now());
        promotion.setIsActive(true);

        Promotion savedPromotion = promotionRepository.save(promotion);
        return promotionMapper.toPromotionResponse(savedPromotion);
    }

    protected String generateUniquePromotionCode() {
        String code;
        do {
            code = "PROMO" + System.currentTimeMillis(); // Simple example, can be improved
        } while (promotionRepository.existsByPromotionCode(code));
        return code;
    }

    @Override
    public ListPromotionResponse getAllPromotions(int page, int size, String sortBy, String sortDir) {
        // Implementation for retrieving all promotions with pagination and sorting
        Pageable pageable = PageRequest.of(page, size,
                sortDir.equalsIgnoreCase("asc") ? org.springframework.data.domain.Sort.by(sortBy).ascending()
                        : org.springframework.data.domain.Sort.by(sortBy).descending());

        Page<Promotion> promotionPage = promotionRepository.findAll(pageable);
        List<Promotion> promotions = promotionPage.getContent();

        List<PromotionResponse> promotionResponses = promotions.stream()
                .map(promotionMapper::toPromotionResponse)
                .toList();
        // This is a placeholder implementation
        return ListPromotionResponse.builder()
                .promotions(promotionResponses)
                .page(promotionPage.getNumber())
                .size(promotionPage.getSize())
                .totalElements(promotionPage.getTotalElements())
                .totalPages(promotionPage.getTotalPages())
                .isLastPage(promotionPage.isLast())
                .build();
    }

    @Override
    public PromotionResponse deactivatePromotion(Long promotionId) {
        Promotion promotion = promotionRepository.findById(promotionId)
                .orElseThrow(() -> new RuntimeException("Promotion not found"));
        promotion.setIsActive(false);
        Promotion updatedPromotion = promotionRepository.save(promotion);
        return promotionMapper.toPromotionResponse(updatedPromotion);
    }

    @Override
    public PromotionResponse getPromotionById(Long promotionId) {
        Promotion promotion = promotionRepository.findById(promotionId)
                .orElseThrow(() -> new RuntimeException("Promotion not found"));
        return promotionMapper.toPromotionResponse(promotion);
    }

    @Override
    public PromotionResponse updatePromotion(Long promotionId, PromotionUpdateRequestDTO promotionUpdateRequestDTO) {
        Promotion promotion = promotionRepository.findById(promotionId)
                .orElseThrow(() -> new RuntimeException("Promotion not found"));

        promotion.setPromotionName(promotionUpdateRequestDTO.getPromotionName());
        promotion.setDescription(promotionUpdateRequestDTO.getDescription());
        promotion.setDiscountType(promotionUpdateRequestDTO.getDiscountType());
        promotion.setDiscountValue(promotionUpdateRequestDTO.getDiscountValue());
        promotion.setMinOrderValue(promotionUpdateRequestDTO.getMinOrderValue());
        promotion.setMaxDiscountValue(promotionUpdateRequestDTO.getMaxDiscountValue());
        promotion.setUsageLimit(promotionUpdateRequestDTO.getUsageLimit());
        promotion.setIsActive(promotionUpdateRequestDTO.getIsActive());
        promotion.setUpdatedAt(LocalDateTime.now());

        Promotion updatedPromotion = promotionRepository.save(promotion);

        return promotionMapper.toPromotionResponse(updatedPromotion);
    }

    @Override
    public ListPromotionResponse searchPromotions(
            DiscountType discountType,
            LocalDate startDate,
            LocalDate endDate,
            Boolean isActive,
            int page,
            int size,
            String sortBy,
            String sortDir
    ) {
        // Implementation for searching promotions based on criteria
        Pageable pageable = PageRequest.of(page, size,
                sortDir.equalsIgnoreCase("asc") ? org.springframework.data.domain.Sort.by(sortBy).ascending()
                        : org.springframework.data.domain.Sort.by(sortBy).descending());
        Page<Promotion> promotionPage = promotionRepository.findByCriteria(discountType, startDate, endDate, isActive, pageable);
        List<Promotion> promotions = promotionPage.getContent();
        List<PromotionResponse> promotionResponses = promotions.stream()
                .map(promotionMapper::toPromotionResponse)
                .toList();
        // This is a placeholder implementation
        return ListPromotionResponse.builder()
                .promotions(promotionResponses)
                .page(promotionPage.getNumber())
                .size(promotionPage.getSize())
                .totalElements(promotionPage.getTotalElements())
                .totalPages(promotionPage.getTotalPages())
                .isLastPage(promotionPage.isLast())
                .build();
    }
}