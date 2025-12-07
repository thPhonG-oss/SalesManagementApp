package com.project.sales_management.controllers;

import com.project.sales_management.services.PromotionService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/promotions")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class PromotionController {
    PromotionService promotionService;
}
