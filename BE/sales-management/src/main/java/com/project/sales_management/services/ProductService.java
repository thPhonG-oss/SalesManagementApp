package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.requests.ProductUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ListProductResponseDTO;
import com.project.sales_management.dtos.responses.ProductImageResponse;
import com.project.sales_management.dtos.responses.ProductResponse;
import jakarta.transaction.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.util.List;

public interface ProductService {
    // CRUD methods to be implemented
    @Transactional
    ProductResponse createProduct(ProductCreationRequestDTO productCreationRequestDTO);

    ProductResponse getProductById(Long productId);

    ListProductResponseDTO getAllProducts(int page, int size, String sortBy, String sortDir);

    ListProductResponseDTO searchProducts(String keyword, Double maxPrice, Double minPrice, int page, int size, String sortBy, String sortDir);

    void deleteProduct(Long productId);

    ProductResponse updateProduct(Long productId, ProductUpdateRequestDTO productUpdateRequestDTO);

    ProductImageResponse updateProductImages(Long productId, MultipartFile file) throws IOException;
}
