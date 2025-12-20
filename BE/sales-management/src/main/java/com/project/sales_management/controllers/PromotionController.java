package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.PromotionCreationRequestDTO;
import com.project.sales_management.dtos.requests.PromotionUpdateRequestDTO;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.responses.ListPromotionResponse;
import com.project.sales_management.dtos.responses.ProductResponse;
import com.project.sales_management.dtos.responses.PromotionResponse;
import com.project.sales_management.models.DiscountType;
import com.project.sales_management.services.PromotionService;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.util.Dictionary;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/promotions")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class PromotionController {
    PromotionService promotionService;

    @PostMapping
    public ResponseEntity<ApiResponse<PromotionResponse>> createPromotion(@RequestBody @Valid PromotionCreationRequestDTO promotionCreationRequestDTO) {
        // Implementation for creating a promotion will go here
        return new ResponseEntity<>(
                ApiResponse.<PromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotion created successfully")
                        .data(promotionService.createPromotion(promotionCreationRequestDTO))
                        .build(),
                HttpStatus.CREATED
        );
    }

    @GetMapping
    public ResponseEntity<ApiResponse<ListPromotionResponse>> getAllPromotion(
            @RequestParam(value = "page", defaultValue = "0") int page,
            @RequestParam(value = "size", defaultValue = "10") int size,
            @RequestParam(value = "sortBy", defaultValue = "createdAt") String sortBy,
            @RequestParam(value = "sortDir", defaultValue = "desc") String sortDir
    ) {
        // Implementation for retrieving all promotions will go here
        return new ResponseEntity<>(
                ApiResponse.<ListPromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotions retrieved successfully")
                        .data(promotionService.getAllPromotions(page, size, sortBy, sortDir))
                        .build(),
                HttpStatus.OK
        );
    }

    @PatchMapping("/{promotionId}/deactivate")
    public ResponseEntity<ApiResponse<PromotionResponse>> deactivatePromotion(@PathVariable Long promotionId) {
        // Implementation for deactivating a promotion will go here
        return new ResponseEntity<>(
                ApiResponse.<PromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotion deactivated successfully")
                        .data(promotionService.deactivatePromotion(promotionId))
                        .build(),
                HttpStatus.OK
        );
    }

    @GetMapping("/{promotionId}")
    public ResponseEntity<ApiResponse<PromotionResponse>> getPromotionById(@PathVariable Long promotionId) {
        // Implementation for retrieving a promotion by ID will go here
        return new ResponseEntity<>(
                ApiResponse.<PromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotion retrieved successfully")
                        .data(promotionService.getPromotionById(promotionId))
                        .build(),
                HttpStatus.OK
        );
    }

    @PutMapping("/{promotionId}")
    public ResponseEntity<ApiResponse<PromotionResponse>> updatePromotion(@PathVariable Long promotionId, @RequestBody @Valid PromotionUpdateRequestDTO promotionUpdateRequestDTO){
        // Implementation for updating a promotion will go here
        return new ResponseEntity<>(
                ApiResponse.<PromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotion updated successfully")
                        .data(promotionService.updatePromotion(promotionId, promotionUpdateRequestDTO))
                        .build(),
                HttpStatus.OK
        );
    }

    @GetMapping("/search")
    public ResponseEntity<ApiResponse<ListPromotionResponse>> searchPromotions(
            @RequestParam(value = "discountType", required = false) DiscountType discountType,
            @RequestParam(value = "startDate", required = false) LocalDate startDate,
            @RequestParam(value = "endDate", required = false) LocalDate endDate,
            @RequestParam(value = "isActive", required = false, defaultValue = "true") Boolean isActive,
            @RequestParam(value = "page", defaultValue = "0") int page,
            @RequestParam(value = "size", defaultValue = "10") int size,
            @RequestParam(value = "sortBy", defaultValue = "createdAt") String sortBy,
            @RequestParam(value = "sortDir", defaultValue = "desc") String sortDir
    ) {
        // Implementation for searching promotions will go here
        return new ResponseEntity<>(
                ApiResponse.<ListPromotionResponse>builder()
                        .code("1000")
                        .success(true)
                        .message("Promotions searched successfully")
                        .data(promotionService.searchPromotions(discountType, startDate, endDate, isActive, page, size, sortBy, sortDir))
                        .build(),
                HttpStatus.OK
        );
    }
}
