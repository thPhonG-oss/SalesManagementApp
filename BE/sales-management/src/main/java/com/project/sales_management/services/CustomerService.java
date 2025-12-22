package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.CustomerResponse;
import org.springframework.data.domain.Page;

import java.util.List;

public interface CustomerService {

    CustomerResponse createCustomer(CustomerRequest customerRequest);
    Page<CustomerResponse> getAllCustomer(Integer pageNumber, Integer pageSize);
    CustomerResponse getCustomerById(Long customerId);
    CustomerResponse updateCustomer(CustomerUpdateRequest customerUpdateRequest, Long customerId);
    CustomerResponse deleteCustomer(Long customerId);
}
