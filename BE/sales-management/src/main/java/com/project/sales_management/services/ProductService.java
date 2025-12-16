package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.responses.ListProductResponseDTO;
import com.project.sales_management.dtos.responses.ProductResponse;
import jakarta.transaction.Transactional;

public interface ProductService {
    // CRUD methods to be implemented
    @Transactional
    ProductResponse createProduct(ProductCreationRequestDTO productCreationRequestDTO);

    ProductResponse getProductById(Long productId);

    ListProductResponseDTO getAllProducts(int page, int size, String sortBy, String sortDir);

    ListProductResponseDTO searchProducts(String keyword, Double maxPrice, Double minPrice, int page, int size, String sortBy, String sortDir);
}
