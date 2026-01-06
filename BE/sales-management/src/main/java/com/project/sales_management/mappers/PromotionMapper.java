package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.PromotionCreationRequestDTO;
import com.project.sales_management.dtos.requests.PromotionUpdateRequestDTO;
import com.project.sales_management.dtos.responses.PromotionResponse;
import com.project.sales_management.models.Promotion;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

@Mapper(componentModel = "spring")
public interface PromotionMapper {

    @Mapping(target = "minOrderValue", source = "minOrderValue")
    PromotionResponse toPromotionResponse(Promotion promotion);

    @Mapping(target = "startDate", ignore = true)
    @Mapping(target = "endDate", ignore = true)
    Promotion toPromotion(PromotionCreationRequestDTO promotionCreationRequestDTO);

    @Mapping(target = "isActive", source = "isActive")
    Promotion mapToPromotion(PromotionUpdateRequestDTO promotionUpdateRequestDTO);
}
