package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.ApiResponse;
import com.project.sales_management.dtos.responses.CustomerResponse;
import com.project.sales_management.services.CustomerService;
import jakarta.validation.Valid;
import lombok.AccessLevel;
import lombok.Builder;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

import static java.util.stream.DoubleStream.builder;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/customers")
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CustomerController {
    CustomerService customerService;

    @PostMapping("")
    ResponseEntity<CustomerResponse> createCustomer(@Valid @RequestBody CustomerRequest customerRequest){
        CustomerResponse customerResponse= customerService.createCustomer(customerRequest);
        return ResponseEntity.ok().body(customerResponse);
    }

    @GetMapping("")
    ResponseEntity<Page<CustomerResponse>> getAllCustomer(
            @RequestParam(defaultValue = "0") Integer pageNumber,
            @RequestParam(defaultValue = "10") Integer pageSize
    ){
        Page<CustomerResponse> customerResponses = customerService.getAllCustomer(pageNumber, pageSize);
        return  ResponseEntity.ok().body(customerResponses);
    }

    @GetMapping("/{customerId}")
    ResponseEntity<CustomerResponse> getCustomerById(@PathVariable Long customerId){
        CustomerResponse customerResponse = customerService.getCustomerById(customerId);
        return ResponseEntity.ok().body(customerResponse);
    }

    @PutMapping("/{customerId}")
    ResponseEntity<CustomerResponse> updateCustomer(@Valid @RequestBody CustomerUpdateRequest customerUpdateRequest, @PathVariable Long customerId){
        CustomerResponse customerResponse = customerService.updateCustomer(customerUpdateRequest,customerId);
        return ResponseEntity.ok().body(customerResponse);
    }

    @DeleteMapping("/{customerId}")
    ResponseEntity<CustomerResponse> deleteCustomer(@PathVariable Long customerId){
        CustomerResponse customerResponse = customerService.deleteCustomer(customerId);
        return  ResponseEntity.ok().body(customerResponse);
    }


}
