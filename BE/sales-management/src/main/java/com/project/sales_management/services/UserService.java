package com.project.sales_management.services;

import com.project.sales_management.dtos.requests.RegisterRequest;
import com.project.sales_management.dtos.responses.UserResponseDTO;

public interface UserService {
    UserResponseDTO createNewUser(RegisterRequest request) throws Exception;

    UserResponseDTO getUserById(Long userId);
}
