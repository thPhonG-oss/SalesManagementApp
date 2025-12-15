package com.project.sales_management.services.Impl;

import com.project.sales_management.mappers.CustomerMapper;
import com.project.sales_management.repositories.CustomerRepository;
import com.project.sales_management.services.CustomerService;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class CustomerServiceImpl implements CustomerService {
    CustomerRepository customerRepository;
    CustomerMapper customerMapper;
}
