package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.CategoryRequest;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.responses.CategoryResponse;
import com.project.sales_management.services.CategoryService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/categories")
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CategoryController {
    CategoryService categoryService;
    @PostMapping
    public ResponseEntity<ApiResponse<CategoryResponse>> createCategory(@RequestBody CategoryRequest categoryRequest){
        return ResponseEntity.ok(ApiResponse.<CategoryResponse>builder()
                        .success(true)
                        .message("create category successfully")
                        .data(categoryService.createCategory(categoryRequest))
                .build());
    }
    @GetMapping
    public ResponseEntity<ApiResponse<List<CategoryResponse>>> getAllCategory(){
        return ResponseEntity.ok(ApiResponse.<List<CategoryResponse>>builder()
                        .success(true)
                        .message("get all successfully")
                        .data(categoryService.getAllCategory())
                .build());
    }
    @GetMapping("/{id}")
    public ResponseEntity<ApiResponse<CategoryResponse>> findOneCategory(@PathVariable Long id){
        return ResponseEntity.ok(ApiResponse.<CategoryResponse>builder()
                        .success(true)
                        .message("findOneCategory successfully")
                        .data(categoryService.findOne(id))
                .build());
    }
    @PutMapping("/{id}")
    public ResponseEntity<ApiResponse<CategoryResponse>> updateCategory(@RequestBody CategoryRequest categoryRequest,@PathVariable Long id){
        return ResponseEntity.ok(ApiResponse.<CategoryResponse>builder()
                        .success(true)
                        .message("update category successfully")
                        .data(categoryService.updateCategory(categoryRequest,id))
                .build());
    }
    @DeleteMapping("/{id}")
    public ResponseEntity<ApiResponse<String>> deleteCategory(@PathVariable Long id){

        return ResponseEntity.ok(ApiResponse.<String>builder()
                        .success(true)
                        .message("delete successfully")
                        .data(categoryService.deleteCategory(id))
                        .build());
    }

}
