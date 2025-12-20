package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.CategoryRequest;
import com.project.sales_management.dtos.responses.CategoryResponse;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.CategoryMapper;
import com.project.sales_management.models.Category;
import com.project.sales_management.repositories.CategoryRepository;
import com.project.sales_management.services.CategoryService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class CategoryServiceImpl implements CategoryService {
    CategoryRepository categoryRepository;
    CategoryMapper categoryMapper;

    @Override
    public CategoryResponse createCategory(CategoryRequest request) {

        if (categoryRepository.existsByCategoryName(request.getCategoryName())) {
            if(categoryRepository.findByCategoryName(request.getCategoryName()).getIsActive()==false)
                throw new AppException(ErrorCode.CATEDORY_DELETED);
            throw new AppException(ErrorCode.CATEGORY_EXISTED);
        }

        Category category = categoryMapper.toCategory(request);
        return categoryMapper.toCategoryResponse(
                categoryRepository.save(category)
        );
    }

    @Override
    public List<CategoryResponse> getAllCategory() {
        return categoryMapper.toListCategoryResponse(categoryRepository.findAllByIsActiveTrue());
    }

    @Override
    public CategoryResponse updateCategory(CategoryRequest categoryRequest,Long id) {
        Category category=categoryRepository.findById(id).orElseThrow(()-> new AppException(ErrorCode.CATEGORY_NOT_EXIST));
        if (categoryRepository.existsByCategoryName(categoryRequest.getCategoryName()) && categoryRepository.findByCategoryName(categoryRequest.getCategoryName()).getIsActive()==true)
            throw new AppException(ErrorCode.CATEGORY_EXISTED);
        categoryMapper.updateCategoryFromRequest(categoryRequest,category);
        return categoryMapper.toCategoryResponse(categoryRepository.save(category));
    }

    @Override
    public CategoryResponse findOne(Long id) {
        Category category=categoryRepository.findById(id).orElseThrow(()-> new AppException(ErrorCode.CATEGORY_NOT_EXIST));
        if(category.getIsActive()==false)
            throw new AppException(ErrorCode.CATEDORY_DELETED);
        return categoryMapper.toCategoryResponse(category);
    }

    @Override
    public String deleteCategory(Long id) {
        Category category=categoryRepository.findById(id).orElseThrow(()-> new AppException(ErrorCode.CATEGORY_NOT_EXIST));
        if(category.getIsActive()==false)
            throw new AppException(ErrorCode.CATEDORY_DELETED);
        category.setIsActive(false);
        categoryRepository.save(category);
        return "delete successfully";
    }
}
