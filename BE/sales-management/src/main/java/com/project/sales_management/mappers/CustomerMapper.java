package com.project.sales_management.mappers;

import com.project.sales_management.dtos.requests.CustomerRequest;
import com.project.sales_management.dtos.requests.CustomerUpdateRequest;
import com.project.sales_management.dtos.responses.CustomerResponse;
import com.project.sales_management.models.Customer;
import org.mapstruct.Mapper;
import org.mapstruct.MappingTarget;

@Mapper(componentModel = "spring")
public interface CustomerMapper {
    CustomerResponse toCustomerResponse(Customer customer);
    void updateEmployeeProfile(@MappingTarget Customer customer, CustomerUpdateRequest customerUpdateRequest);
    Customer toCustomer(CustomerRequest customerRequest);
}
