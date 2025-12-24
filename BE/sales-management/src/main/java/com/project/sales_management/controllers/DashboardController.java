package com.project.sales_management.controllers;

import com.project.sales_management.dtos.responses.DashboardResponse;
import com.project.sales_management.services.DashboardService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/admin/dashboard")
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class DashboardController {

    DashboardService dashboardService;
    @GetMapping("")
    public ResponseEntity<DashboardResponse> getDashboard() {
        return ResponseEntity.ok(dashboardService.getDashboard());
    }
}
