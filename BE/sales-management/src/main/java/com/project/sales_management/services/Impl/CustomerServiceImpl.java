package com.project.sales_management.services.Impl;


import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.CustomerResponse;
import com.project.sales_management.exception.AppException;
import com.project.sales_management.exception.ErrorCode;
import com.project.sales_management.mappers.CustomerMapper;
import com.project.sales_management.models.Customer;
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
        // Kiểm tra email đã tồn tại
        if (customerRepository.existsByEmail(customerRequest.getEmail())) {
            throw new AppException(ErrorCode.EMAIL_EXIST);
        }

        // Kiểm tra phone đã tồn tại
        if (customerRepository.existsByPhone(customerRequest.getPhone())) {
            throw new AppException(ErrorCode.PHONE_EXIST);
        }

        Customer customer = customerMapper.toCustomer(customerRequest);

        // Khởi tạo các giá trị mặc định
        customer.setTotalOrder(0);
        customer.setTotalSpent(0.0);
        customer.setCreatedAt(LocalDateTime.now());
        customer.setUpdatedAt(LocalDateTime.now());

        Customer savedCustomer = customerRepository.save(customer);


        return customerMapper.toCustomerResponse(savedCustomer);
    }

    @Override
    public List<CustomerResponse> getAllCustomer() {
        List<Customer> customers = customerRepository.findAll();
        return customers.stream()
                .map(customerMapper::toCustomerResponse)
                .collect(Collectors.toList());
    }

    @Override
    public CustomerResponse getCustomerById(Long customerId) {
        Customer customer = customerRepository.findById(customerId)
                .orElseThrow(() -> new AppException(ErrorCode.CUSTOMER_NOT_FOUND));

        return customerMapper.toCustomerResponse(customer);
    }

    @Override
    public CustomerResponse updateCustomer(CustomerUpdateRequest customerUpdateRequest, Long customerId) {
        // Tìm customer
        Customer customer = customerRepository.findById(customerId)
                .orElseThrow(() -> new AppException(ErrorCode.CUSTOMER_NOT_FOUND));

        // Kiểm tra email mới (nếu thay đổi)
        if (customerUpdateRequest.getEmail() != null &&
                !customerUpdateRequest.getEmail().equals(customer.getEmail())) {
            if (customerRepository.existsByEmail(customerUpdateRequest.getEmail())) {
                throw new AppException(ErrorCode.EMAIL_EXIST);
            }
        }

        // Kiểm tra phone mới (nếu thay đổi)
        if (customerUpdateRequest.getPhone() != null &&
                !customerUpdateRequest.getPhone().equals(customer.getPhone())) {
            if (customerRepository.existsByPhone(customerUpdateRequest.getPhone())) {
                throw new AppException(ErrorCode.PHONE_EXIST);
            }
        }

        // Sử dụng mapper để update
        customerMapper.updateEmployeeProfile(customer, customerUpdateRequest);

        // Cập nhật updatedAt
        customer.setUpdatedAt(LocalDateTime.now());

        // Save changes
        Customer updatedCustomer = customerRepository.save(customer);

        return customerMapper.toCustomerResponse(updatedCustomer);
    }

    @Override
    public CustomerResponse deleteCustomer(Long customerId) {
        Customer customer = customerRepository.findById(customerId)
                .orElseThrow(() -> new AppException(ErrorCode.CUSTOMER_NOT_FOUND));

        CustomerResponse response = customerMapper.toCustomerResponse(customer);


        customerRepository.delete(customer);

        return response;
    }

}
