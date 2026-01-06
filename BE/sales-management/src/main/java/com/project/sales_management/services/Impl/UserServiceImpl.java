package com.project.sales_management.services.Impl;

import com.project.sales_management.dtos.requests.ChangePasswordRequest;
import com.project.sales_management.dtos.requests.RegisterRequest;
import com.project.sales_management.dtos.responses.UserResponseDTO;
import com.project.sales_management.mappers.UserMapper;
import com.project.sales_management.models.RefreshToken;
import com.project.sales_management.models.User;
import com.project.sales_management.repositories.RefreshTokenRepository;
import com.project.sales_management.repositories.UserRepository;
import com.project.sales_management.services.UserService;
import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Slf4j
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

    @Autowired
    RefreshTokenRepository refreshTokenRepository;

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


    @Transactional
    @Override
    public String changePassword(ChangePasswordRequest request){
        String username = SecurityContextHolder.getContext().getAuthentication().getName();
        log.info("Changing password for user: " + username);

        User user = userRepository.findByUsername(username).orElseThrow(()->new RuntimeException("User not found"));
        if(!passwordEncoder.matches(request.getOldPassword(), user.getPassword())){
            throw new RuntimeException("Old password is incorrect");
        }

        RefreshToken refreshToken = refreshTokenRepository.findByUser(user)
                .orElseThrow(() -> new RuntimeException("Refresh token not found for user: " + username));
        refreshTokenRepository.delete(refreshToken);
        log.info("Deleted refresh token for user: " + username);

        user.setPassword(passwordEncoder.encode(request.getNewPassword()));
        userRepository.save(user);
        log.info("Password changed successfully for user: " + username);

        return "Password changed successfully";
    }
}
