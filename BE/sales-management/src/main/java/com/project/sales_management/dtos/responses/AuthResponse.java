package com.project.sales_management.dtos.responses;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import org.springframework.http.ResponseCookie;

@Data
@Builder
@AllArgsConstructor
public class AuthResponse {
    UserResponseDTO userInfo;
    ResponseCookie jwtCookie;
}
