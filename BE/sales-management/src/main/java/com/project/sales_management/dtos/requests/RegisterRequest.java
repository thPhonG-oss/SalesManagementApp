package com.project.sales_management.dtos.requests;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
public class RegisterRequest {
    String username;
    String password;
}
