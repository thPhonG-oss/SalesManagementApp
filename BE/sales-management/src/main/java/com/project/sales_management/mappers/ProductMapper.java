package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.models.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

@Mapper(componentModel = "spring", uses = {ProductImageMapper.class, CategoryMapper.class})
public interface ProductMapper {
    Product toProduct(ProductCreationRequestDTO productCreationRequestDTO);

    @Mapping(target = "images", source = "product.productImages")
    @Mapping(target = "category", source = "category")
    ProductResponse toProductResponse(Product product);

}
