package com.project.sales_management.mappers;

import com.project.sales_management.dtos.responses.UserResponseDTO;
import com.project.sales_management.models.User;
import org.mapstruct.Mapper;

@Mapper(componentModel = "spring")
public interface UserMapper {
    User toUser(UserResponseDTO userResponseDTO);
    UserResponseDTO toUserResponseDTO(User user);
}
