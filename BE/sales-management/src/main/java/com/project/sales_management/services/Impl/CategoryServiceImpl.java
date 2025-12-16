package com.project.sales_management.services.Impl;

import com.project.sales_management.mappers.CategoryMapper;
import com.project.sales_management.repositories.CategoryRepository;
import com.project.sales_management.services.CategoryService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class CategoryServiceImpl implements CategoryService {
    CategoryRepository categoryRepository;
    CategoryMapper categoryMapper;
}
