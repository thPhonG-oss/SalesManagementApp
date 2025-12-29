package com.project.sales_management.controllers;


import com.project.sales_management.dtos.responses.*;
import com.project.sales_management.services.ReportService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.time.LocalDate;
import java.util.List;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/reports")
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class ReportController {

    ReportService reportService;
    @GetMapping("/revenue/daily")
    public ResponseEntity<List<DailyRevenueResponse>> getDashboardDailyRevenue(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getDailyRevenue(startDate, endDate));
    }

    @GetMapping("/revenue/weekly")
    public ResponseEntity<List<WeeklyRevenueResponse>> getDashboardWeeklyRevenue(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getWeeklyRevenue(startDate, endDate));
    }

    @GetMapping("revenue/month")
    public ResponseEntity<List<MonthlyRevenueResponse>> getDashboardMonthlyRevenue(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getMonthlyRevenue(startDate, endDate));
    }

    @GetMapping("/revenue/year")
    public ResponseEntity<List<YearlyRevenueResponse>> getDashboardYearlyRevenue(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getYearlyRevenue(startDate, endDate));
    }

    @GetMapping("/products/sales")
    public ResponseEntity<List<ProductSalesResponse>> getDashboardProductSales(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getProductSales(startDate, endDate));
    }

    @GetMapping("/products/weekly")
    public ResponseEntity<List<WeeklyProductSalesResponse>> getDashboarWeeklyProductSales(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getWeeklyProductSales(startDate, endDate));
    }

    @GetMapping("/products/month")
    public ResponseEntity<List<MonthlyProductSalesResponse>> getDashboarMonthlyProductSales(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getMonthlyProductSales(startDate, endDate));
    }

    @GetMapping("/products/year")
    public ResponseEntity<List<YearlyProductSalesResponse>> getDashboarYearlyProductSales(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getYearlyProductSales(startDate, endDate));
    }

    @GetMapping("/products/daily")
    public ResponseEntity<List<DailyProductSalesResponse>> getDashboarDailyProductSales(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate) {

        return ResponseEntity.ok().body(reportService.getDailyProductSales(startDate, endDate));
    }
// ---------------------------
    @GetMapping("/dashboard/current-month")
    public ResponseEntity<ReportResponse> getDashboardReportResponse(){
        return ResponseEntity.ok().body(reportService.getCurrentMonthDashboard());
    }

    @GetMapping("/revenue/current-month")
    public ResponseEntity<ReportResponse> getDashboardReportResponseCurrentMonth(){
        return ResponseEntity.ok().body(reportService.getCurrentMonthDashboard());
    }

    @GetMapping("/revenue/current-year")
    public ResponseEntity<ReportResponse> getDashboardReportCurrentYear(){
        return ResponseEntity.ok().body(reportService.getCurrentMonthDashboard());
    }

    // ---------------------------



}
