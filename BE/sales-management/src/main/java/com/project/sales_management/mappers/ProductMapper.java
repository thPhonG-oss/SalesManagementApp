package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.models.Product;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring", uses = {ProductImageMapper.class})
public interface ProductMapper {
    ProductResponse toProductResponse(Product product);
}
