package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.CategoryResponse;
import com.project.sales_management.models.Category;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface CategoryMapper {
    CategoryResponse toCategoryResponse(Category category);
}
