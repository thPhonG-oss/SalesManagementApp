package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.requests.ProductUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.responses.ListProductResponseDTO;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.services.ProductService;
import jakarta.validation.Valid;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/products")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class ProductController {
    ProductService productService;

    @PostMapping
    public ResponseEntity<ApiResponse<ProductResponse>> createProduct(@RequestBody @Valid ProductCreationRequestDTO productCreationRequestDTO) {
        // Implementation goes here
        return new ResponseEntity<>(
                ApiResponse.<ProductResponse>builder()
                        .success(true)
                        .message("Product created successfully")
                        .data(productService.createProduct(productCreationRequestDTO))
                        .build(),
                org.springframework.http.HttpStatus.CREATED
        );
    }

    @GetMapping("/{productId}")
    public ResponseEntity<ApiResponse<ProductResponse>> getProductById(@PathVariable Long productId) {
        // Implementation goes here
        return ResponseEntity.ok(
                ApiResponse.<ProductResponse>builder()
                        .success(true)
                        .message("Product retrieved successfully")
                        .data(productService.getProductById(productId))
                        .build()
        );
    }

    @GetMapping
    public ResponseEntity<ApiResponse<ListProductResponseDTO>> getAllProducts(
            @RequestParam(defaultValue = "1", required = false) int page,
            @RequestParam(defaultValue = "20", required = false) int size,
            @RequestParam(defaultValue = "productName", required = false) String sortBy,
            @RequestParam(defaultValue = "asc", required = false) String sortDir
    ) {
        return ResponseEntity.ok(
                ApiResponse.<ListProductResponseDTO>builder()
                        .success(true)
                        .message("Products retrieved successfully")
                        .data(productService.getAllProducts(page, size, sortBy, sortDir))
                        .build()
        );
    }

    @GetMapping("/search")
    public ResponseEntity<ApiResponse<ListProductResponseDTO>> searchProducts(
            @RequestParam(required = false, defaultValue = "") String keyword,
            @RequestParam(required = false, defaultValue = "10000") double maxPrice,
            @RequestParam(required = false, defaultValue = "0") double minPrice,
            @RequestParam(defaultValue = "1", required = false) int page,
            @RequestParam(defaultValue = "20", required = false) int size,
            @RequestParam(defaultValue = "productName", required = false) String sortBy,
            @RequestParam(defaultValue = "asc", required = false) String sortDir
    ) {
        return ResponseEntity.ok(
                ApiResponse.<ListProductResponseDTO>builder()
                        .success(true)
                        .message("Products retrieved successfully")
                        .data(productService.searchProducts(keyword, maxPrice, minPrice, page, size, sortBy, sortDir))
                        .build()
        );
    }

    @PostMapping("/{productId}/images")
    public ResponseEntity<ApiResponse<?>> uploadProductImage(@PathVariable Long productId,@RequestParam("file") MultipartFile file) throws IOException {
        return ResponseEntity.ok(
                ApiResponse.builder()
                        .success(true)
                        .message("Product image uploaded successfully")
                        .data(productService.updateProductImages(productId, file))
                        .build()
        );
    }

    @PutMapping("/{productId}")
    public ResponseEntity<ApiResponse<ProductResponse>> updateProduct(
            @PathVariable Long productId,
            @RequestBody @Valid ProductUpdateRequestDTO productUpdateRequestDTO
    ) {
        return ResponseEntity.ok(
                ApiResponse.<ProductResponse>builder()
                        .success(true)
                        .message("Product updated successfully")
                        .data(productService.updateProduct(productId, productUpdateRequestDTO))
                        .build()
        );
    }

    @PatchMapping("/{productId}" )
    public ResponseEntity<ApiResponse<?>> deleteProduct(@PathVariable Long productId) {
        productService.deleteProduct(productId);
        return ResponseEntity.ok(
                ApiResponse.builder()
                        .success(true)
                        .message("Product deleted successfully")
                        .build()
        );
    }

}
