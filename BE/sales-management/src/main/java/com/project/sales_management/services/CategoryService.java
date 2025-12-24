package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.CategoryRequest;
import com.project.sales_management.dtos.responses.CategoryResponse;

import java.util.List;

public interface CategoryService {
    CategoryResponse createCategory(CategoryRequest categoryRequest);
    CategoryResponse updateCategory(CategoryRequest categoryRequest,Long id);
    List<CategoryResponse> getAllCategory();
    CategoryResponse findOne(Long id);
    String deleteCategory(Long id);
}
