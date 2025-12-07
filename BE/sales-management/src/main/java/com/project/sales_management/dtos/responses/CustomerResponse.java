package com.project.sales_management.dtos.responses;

import com.fasterxml.jackson.annotation.JsonInclude;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@Builder
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@JsonInclude(JsonInclude.Include.NON_NULL)
public class CustomerResponse {
    Long customerId;
    String customerName;
    String phone;
    String email;
    String address;
    Integer totalOrder;
    Double totalSpent;
    String lastOrderDate;
    String createdAt;
    String updatedAt;
}
