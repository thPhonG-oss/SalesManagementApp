package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.RegisterRequest;
import com.project.sales_management.dtos.responses.UserResponseDTO;
import com.project.sales_management.mappers.UserMapper;
import com.project.sales_management.models.User;
import com.project.sales_management.repositories.UserRepository;
import com.project.sales_management.services.UserService;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class UserServiceImpl implements UserService {

    @Autowired
    private UserMapper userMapper;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Override
    public UserResponseDTO createNewUser(RegisterRequest request) throws Exception {

        if(userRepository.existsByUsername(request.getUsername())){
            throw new RuntimeException("Username is already in use");
        }

        User user = User.builder()
                .username(request.getUsername())
                .password(passwordEncoder.encode(request.getPassword()))
                .build();

        return userMapper.toUserResponseDTO(userRepository.save(user));
    }

    @Override
    public UserResponseDTO getUserById(Long userId){

        User user = userRepository.findById(userId).orElseThrow(()->new RuntimeException("User not found"));

        return userMapper.toUserResponseDTO(user);
    }
}
