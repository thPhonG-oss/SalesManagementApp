package com.project.sales_management.services.Impl;

import com.project.sales_management.repositories.ProductImageRepository;
import com.project.sales_management.services.ProductImageService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class ProductImageServiceImpl implements ProductImageService {
    ProductImageRepository productImageRepository;
}
