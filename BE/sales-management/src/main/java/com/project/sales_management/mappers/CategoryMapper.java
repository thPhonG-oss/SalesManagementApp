package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.CategoryRequest;
import com.project.sales_management.dtos.responses.CategoryResponse;
import com.project.sales_management.models.Category;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;
import org.mapstruct.MappingTarget;

import java.util.List;
import java.time.LocalDateTime;

@Mapper(componentModel = "spring", imports = LocalDateTime.class)
public interface CategoryMapper {
    CategoryResponse toCategoryResponse(Category category);

    @Mapping(target = "createdAt", expression = "java(LocalDateTime.now())")
    @Mapping(target = "updatedAt", expression = "java(LocalDateTime.now())")
    @Mapping(target = "categoryId", ignore = true)
    @Mapping(target = "products", ignore = true)
    Category toCategory(CategoryRequest categoryRequest);

    List<CategoryResponse> toListCategoryResponse(List<Category> categories);

    @Mapping(target = "updatedAt", expression = "java(LocalDateTime.now())")
    @Mapping(target = "createdAt", ignore = true)
    @Mapping(target = "categoryId", ignore = true)
    @Mapping(target = "products", ignore = true)
    void updateCategoryFromRequest(
            CategoryRequest request,
            @MappingTarget Category category
    );
}
