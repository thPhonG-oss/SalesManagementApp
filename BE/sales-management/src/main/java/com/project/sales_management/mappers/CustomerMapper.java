package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.CustomerResponse;
import com.project.sales_management.models.Customer;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface CustomerMapper {
    CustomerResponse toCustomerResponse(Customer customer);
}
