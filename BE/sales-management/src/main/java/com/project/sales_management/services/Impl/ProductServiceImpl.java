package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.requests.ProductUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ListProductResponseDTO;
import com.project.sales_management.dtos.responses.ProductImageResponse;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.mappers.ProductImageMapper;
import com.project.sales_management.mappers.ProductMapper;
import com.project.sales_management.models.Category;
import com.project.sales_management.models.Product;
import com.project.sales_management.models.ProductImage;
import com.project.sales_management.repositories.CategoryRepository;
import com.project.sales_management.repositories.ProductImageRepository;
import com.project.sales_management.repositories.ProductRepository;
import com.project.sales_management.services.ProductService;
import jakarta.transaction.Transactional;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.web.multipart.MultipartFile;

import java.io.IOException;
import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Slf4j
@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class ProductServiceImpl implements ProductService {
    ProductRepository productRepository;
    ProductMapper productMapper;
    CategoryRepository categoryRepository;
    ProductImageRepository productImageRepository;
    CloudinaryService cloudinaryService;
    ProductImageMapper productImageMapper;

    // CRUD methods to be implemented
    @Transactional
    @Override
    public ProductResponse createProduct(ProductCreationRequestDTO productCreationRequestDTO) {
        // Implementation goes here
        if(productRepository.existsByProductName(productCreationRequestDTO.getProductName())) {
            throw new RuntimeException("Product name already exists");
        }
        Product product = productMapper.toProduct(productCreationRequestDTO);

        Category category = categoryRepository.findById(productCreationRequestDTO.getCategoryId())
                .orElseThrow(() -> new RuntimeException("Category not found"));
        product.setCategory(category);

        Product savedProduct = productRepository.save(product);
        return productMapper.toProductResponse(savedProduct);
    }

    @Override
    public ProductResponse getProductById(Long productId) {
        // Implementation goes here
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Product not found"));
        log.info("Fetched product: {}", product.getProductName());
        return productMapper.toProductResponse(product);
    }

    @Override
    public ListProductResponseDTO getAllProducts(int page, int size, String sortBy, String sortDir) {
        // Implementation goes here
        // Pagination and sorting logic to be implemented
        Pageable pageable = PageRequest.of(page -1, size, sortDir.equalsIgnoreCase("asc") ?
                org.springframework.data.domain.Sort.by(sortBy).ascending() :
                org.springframework.data.domain.Sort.by(sortBy).descending());

        Page<Product> productPage = productRepository.findAll(pageable);

        List<Product>  products = productPage.getContent();

        List<ProductResponse> listProductResponseDTO = products.stream().map(productMapper::toProductResponse).collect(Collectors.toList());

        return ListProductResponseDTO.builder()
                .products(listProductResponseDTO)
                .page(productPage.getNumber())
                .size(productPage.getSize())
                .totalElements(productPage.getTotalElements())
                .totalPages(productPage.getTotalPages())
                .isLastPage(productPage.isLast())
                .build();
    }

    @Override
    public ListProductResponseDTO searchProducts(String keyword, Double maxPrice, Double minPrice, int page, int size, String sortBy, String sortDir) {
        log.info("Searching products for keyword: {}", keyword);
        // Implementation goes here
        Pageable pageable = PageRequest.of(page - 1, size, sortDir.equalsIgnoreCase("asc") ?
                org.springframework.data.domain.Sort.by(sortBy).ascending() :
                org.springframework.data.domain.Sort.by(sortBy).descending());
        if(keyword.isEmpty()) {
            return getAllProducts(page, size, sortBy, sortDir);
        }

        else {
            Page<Product> productPage = productRepository.findByProductNameContainingIgnoreCase(keyword, maxPrice, minPrice, pageable);
            List<Product> products = productPage.getContent();
            List<ProductResponse> listProductResponseDTO = products.stream().map(productMapper::toProductResponse).collect(Collectors.toList());
            return ListProductResponseDTO.builder()
                    .products(listProductResponseDTO)
                    .page(productPage.getNumber())
                    .size(productPage.getSize())
                    .totalElements(productPage.getTotalElements())
                    .totalPages(productPage.getTotalPages())
                    .isLastPage(productPage.isLast())
                    .build();
        }
    }

    @Override
    public void deleteProduct(Long productId) {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Product not found"));

        product.setIsActive(false);
        productRepository.save(product);
        log.info("Deleted product: {}", product.getProductName());
    }

    @Override
    public ProductResponse updateProduct(Long productId, ProductUpdateRequestDTO productUpdateRequestDTO) {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Product not found"));
        Category category = categoryRepository.findById(productUpdateRequestDTO.getCategoryId())
                .orElseThrow(() -> new RuntimeException("Category not found"));

        product.setCategory(category);
        product.setProductName(productUpdateRequestDTO.getProductName());
        product.setDescription(productUpdateRequestDTO.getDescription());
        product.setAuthor(productUpdateRequestDTO.getAuthor());
        product.setPublisher(productUpdateRequestDTO.getPublisher());
        product.setPublicationYear(productUpdateRequestDTO.getPublicationYear());
        product.setPrice(productUpdateRequestDTO.getPrice());
        product.setStockQuantity(productUpdateRequestDTO.getStockQuantity());
        product.setMinStockQuantity(productUpdateRequestDTO.getMinStockQuantity());
        Product updatedProduct = productRepository.save(product);
        log.info("Updated product: {}", updatedProduct.getProductName());
        return productMapper.toProductResponse(updatedProduct);
    }

    @Transactional
    @Override
    public ProductImageResponse updateProductImages(Long productId, MultipartFile file) throws IOException {
        Product product = productRepository.findById(productId)
                .orElseThrow(() -> new RuntimeException("Product not found"));

        String image_url =  cloudinaryService.uploadFileWithoutFolder(file);

        ProductImage productImage = ProductImage.builder()
                .imageUrl(image_url)
                .product(product)
                .createdAt(LocalDateTime.now())
                .build();

        ProductImage savedImage = productImageRepository.save(productImage);

        return productImageMapper.toProductImageResponse(savedImage);
    }
}
