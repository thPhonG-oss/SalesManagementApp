package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.CustomerResponse;

import java.util.List;

public interface CustomerService {

    CustomerResponse createCustomer(CustomerRequest customerRequest);
    List<CustomerResponse> getAllCustomer();
    CustomerResponse getCustomerById(Long customerId);
    CustomerResponse updateCustomer(CustomerUpdateRequest customerUpdateRequest, Long customerId);
    CustomerResponse deleteCustomer(Long customerId);
}
