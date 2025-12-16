package com.project.sales_management.services.Impl;


import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.CustomerResponse;
import com.project.sales_management.mappers.CustomerMapper;
import com.project.sales_management.repositories.CustomerRepository;
import com.project.sales_management.services.CustomerService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Slf4j
public class CustomerServiceImpl implements CustomerService {
    CustomerRepository customerRepository;
    CustomerMapper customerMapper;

    @Override
    public CustomerResponse createCustomer(CustomerRequest customerRequest) {
        return null;
    }

    @Override
    public List<CustomerResponse> getAllCustomer() {
        return List.of();
    }

    @Override
    public CustomerResponse getCustomerById(Long customerId) {
        return null;
    }

    @Override
    public CustomerResponse updateCustomer(CustomerUpdateRequest customerUpdateRequest, Long customerId) {
        return null;
    }

    @Override
    public CustomerResponse deleteCustomer(Long customerId) {
        return null;
    }
}
