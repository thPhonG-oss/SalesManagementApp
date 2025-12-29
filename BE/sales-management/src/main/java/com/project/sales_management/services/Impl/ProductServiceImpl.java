package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.ProductCreationRequestDTO;
import com.project.sales_management.dtos.requests.ProductImportDTO;
import com.project.sales_management.dtos.responses.ImportError;
import com.project.sales_management.dtos.responses.ListProductResponseDTO;
import com.project.sales_management.dtos.responses.ProductImportResponse;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.helpers.ExcelHelpers;
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
    ExcelHelpers excelHelpers;

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

    @Transactional
    @Override
    public ProductImportResponse importProducts(MultipartFile file) {
        // Implementation goes here
        ProductImportResponse response = new ProductImportResponse();
        List<Product> products;

        if (file.isEmpty()) {
            response.setSuccess(false);
            response.setMessage("File không được để trống");
            return response;
        }

        if (!ExcelHelpers.hasExcelFormat(file)) {
            response.setSuccess(false);
            response.setMessage("File phải có định dạng Excel (.xlsx hoặc .xls)");
            return response;
        }

        try{
            List<ProductImportDTO> productImportDTOS = excelHelpers.parseExcelFile(
                    file.getInputStream(),
                    response
            );

            response.setTotalRows(productImportDTOS.size());
            int totalRows = productImportDTOS.size();
            int successCount = 0;
            int failureCount = 0;
            for (ProductImportDTO productImportDTO : productImportDTOS) {
                try{
                    Category category = categoryRepository.findByCategoryName(productImportDTO.getCategoryName());
                    if(category == null) {
                        failureCount++;
                        response.getErrors().add(
                                ImportError.builder()
                                        .rowNumber(response.getTotalRows() - productImportDTOS.indexOf(productImportDTO))
                                        .errorMessage("Danh mục không tồn tại")
                                        .build()
                        );
                        continue;
                    }

                    if(productRepository.existsByProductName(productImportDTO.getProductName())) {
                        failureCount++;
                        response.getErrors().add(
                                ImportError.builder()
                                        .rowNumber(response.getTotalRows() - productImportDTOS.indexOf(productImportDTO))
                                        .errorMessage("Tên sản phẩm đã tồn tại")
                                        .build()
                        );
                        continue;
                    }
                    else {
                        Product product = Product.builder()
                                .category(category)
                                .productName(productImportDTO.getProductName())
                                .description(productImportDTO.getDescription())
                                .author(productImportDTO.getAuthor())
                                .publisher(productImportDTO.getPublisher())
                                .publicationYear(productImportDTO.getPublicationYear())
                                .price(productImportDTO.getPrice())
                                .discountPercentage(productImportDTO.getDiscountPercentage())
                                .stockQuantity(productImportDTO.getStockQuantity())
                                .minStockQuantity(productImportDTO.getMinStockQuantity())
                                .soldQuantity(0)
                                .isActive(true)
                                .isDiscounted(productImportDTO.getDiscountPercentage() != null && productImportDTO.getDiscountPercentage() > 0)
                                .specialPrice(productImportDTO.getDiscountPercentage() != null && productImportDTO.getDiscountPercentage() > 0 ?
                                        productImportDTO.getPrice() * (1 - productImportDTO.getDiscountPercentage() / 100) : productImportDTO.getPrice())
                                .build();

                        productRepository.save(product);
                        successCount++;
                    }
                }
                catch (Exception e){
                    response.getErrors().add(new ImportError(
                            -1,
                            "save",
                            "Lỗi lưu sản phẩm: " + productImportDTO.getProductName() + " - " + e.getMessage()
                    ));
                }
            }

            response.setTotalRows(totalRows);
            response.setImportedRows(successCount);
            response.setSkippedRows(failureCount);

            if(successCount > 0) {
                response.setSuccess(true);
                response.setMessage("Import thành công " + successCount + " sản phẩm, bỏ qua " + failureCount + " sản phẩm do lỗi.");
                return response;
            }else {
                response.setSuccess(false);
                response.setMessage("Không có sản phẩm nào được import. Vui lòng kiểm tra lại file.");
                return response;
            }
        }
        catch (Exception e){
            log.info("Error during import: {}", e.getMessage());
            response.setSuccess(false);
            response.setMessage("Đã xảy ra lỗi trong quá trình import: " + e.getMessage());
        }

        return response;
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

    @Override
    public ListProductResponseDTO getProductsByCategory(Long categoryId, int minPrice, int maxPrice, String keyword, int page, int size, String sortBy, String sortDir) {
        Pageable pageable = PageRequest.of(page -1, size, sortDir.equalsIgnoreCase("asc") ?
                org.springframework.data.domain.Sort.by(sortBy).ascending() :
                org.springframework.data.domain.Sort.by(sortBy).descending());

        Page<Product> productPage;
        if(keyword.isEmpty()) {
            productPage = productRepository.findByCategory_CategoryIdAndPriceBetween(categoryId, minPrice, maxPrice, pageable);
        } else {
            productPage = productRepository.findByCategory_CategoryIdAndPriceBetweenAndProductNameContainingIgnoreCase(categoryId, minPrice, maxPrice, keyword, pageable);
        }

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
}
