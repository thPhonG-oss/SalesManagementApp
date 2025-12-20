package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.PromotionCreationRequestDTO;
import com.project.sales_management.dtos.requests.PromotionUpdateRequestDTO;
import com.project.sales_management.dtos.responses.PromotionResponse;
import com.project.sales_management.models.Promotion;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface PromotionMapper {
    PromotionResponse toPromotionResponse(Promotion promotion);
    Promotion toPromotion(PromotionCreationRequestDTO promotionCreationRequestDTO);
    Promotion mapToPromotion(PromotionUpdateRequestDTO promotionUpdateRequestDTO);
}
