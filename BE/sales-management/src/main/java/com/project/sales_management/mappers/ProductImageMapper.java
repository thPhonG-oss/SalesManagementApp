package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.ProductImageResponse;
import com.project.sales_management.models.ProductImage;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface ProductImageMapper {
    ProductImageResponse toProductImageResponse(ProductImage productImage);
}
