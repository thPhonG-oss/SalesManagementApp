package com.project.sales_management.services.Impl;

import com.project.sales_management.configuration.JwtUtils;
import com.project.sales_management.dtos.requests.LoginRequest;
import com.project.sales_management.dtos.requests.RegisterRequest;
import com.project.sales_management.dtos.responses.AuthResponse;
import com.project.sales_management.dtos.responses.UserResponseDTO;
import com.project.sales_management.mappers.UserMapper;
import com.project.sales_management.models.RefreshToken;
import com.project.sales_management.models.User;
import com.project.sales_management.repositories.RefreshTokenRepository;
import com.project.sales_management.repositories.UserRepository;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.transaction.Transactional;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseCookie;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Slf4j
@Service
@RequiredArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
public class AuthService {
    UserRepository userRepository;
    PasswordEncoder passwordEncoder;
    RefreshTokenRepository refreshTokenRepository;
    JwtUtils jwtUtils;
    UserMapper userMapper;

    @Transactional
    public UserResponseDTO register(RegisterRequest registerRequest) {
        if(userRepository.findByUsername(registerRequest.getUsername()).isPresent()){
            throw new RuntimeException("Username is already in use");
        }

        User user = User.builder()
                .username(registerRequest.getUsername())
                .password(passwordEncoder.encode(registerRequest.getPassword()))
                .build();

        return userMapper.toUserResponseDTO(userRepository.save(user));
    }


    public boolean authenticate(String token){
        log.info("Authenticating token: " + token);

        String username = jwtUtils.getUsernameFromToken(token);
        User user = userRepository.findByUsername(username).orElseThrow(() -> new RuntimeException("User not found"));

        return jwtUtils.validateToken(token);
    }

    @Transactional
    public AuthResponse login(LoginRequest loginRequest){
        User user = userRepository.findByUsername(loginRequest.getUsername())
                .orElseThrow(() -> new RuntimeException("User not found"));

        if(!passwordEncoder.matches(loginRequest.getPassword(), user.getPassword())){
            throw new RuntimeException("Wrong password");
        }

        // xoa refresh token cu
        refreshTokenRepository.deleteByUser(user);

        String accessToken = jwtUtils.generateAccessToken(user.getUsername());
        String refreshToken = jwtUtils.generateRefreshToken(user.getUsername());
        RefreshToken refreshTokenEntity = RefreshToken.builder()
                .token(refreshToken)
                .user(user)
                .expiresAt(LocalDateTime.now().plusDays(7))
                .build();

        refreshTokenRepository.save(refreshTokenEntity);

        UserResponseDTO userResponseDTO = UserResponseDTO.builder()
                .id(user.getUserId())
                .username(user.getUsername())
                .accessToken(accessToken)
                .build();

        return AuthResponse.builder()
                .userInfo(userResponseDTO)
                .jwtCookie(jwtUtils.createRefreshCookie(refreshToken))
                .build();
    }

    @Transactional
    public ResponseCookie logout(HttpServletRequest request, HttpServletResponse response){
        String refreshToken = jwtUtils.getJwtFromCookies(request);
        log.info("Refresh token: {}", refreshToken);



        if(refreshToken == null){
            throw new RuntimeException("Refresh token is null");
        }
        if(!jwtUtils.validateToken(refreshToken)){
            throw new RuntimeException("Invalid refresh token");
        }

        String username = jwtUtils.getUsernameFromToken(refreshToken);
        User user = userRepository.findByUsername(username).orElseThrow(() -> new RuntimeException("User not found"));

        RefreshToken existedToken = refreshTokenRepository.findByUser(user).orElseThrow(() -> new RuntimeException("Refresh token not found"));

        log.info("Existed Refresh token: {}", existedToken.getToken());
        refreshTokenRepository.delete(existedToken);

        ResponseCookie jwtCookie = jwtUtils.clearCookie();


        return  jwtCookie;
    }

    @Transactional
    public AuthResponse refreshAccessToken(HttpServletRequest request, HttpServletResponse response){

        String refreshToken = jwtUtils.getJwtFromCookies(request);


        log.info("Refresh token: {}", refreshToken);

        if(refreshToken == null){
            throw new RuntimeException("Refresh token is null");
        }

        if(!jwtUtils.validateToken(refreshToken)){
            throw new RuntimeException("Invalid refresh token");
        }

        RefreshToken refreshTokenEntity = refreshTokenRepository.findByToken(refreshToken).orElseThrow(() -> new RuntimeException("Refresh token not found"));

        if(!refreshTokenEntity.getExpiresAt().isAfter(LocalDateTime.now())){
            refreshTokenRepository.delete(refreshTokenEntity);
            throw new RuntimeException("Expired refresh token");
        }

        User user = refreshTokenEntity.getUser();

        String newAccessToken = jwtUtils.generateAccessToken(user.getUsername());

        UserResponseDTO userResponseDTO = UserResponseDTO.builder()
                .id(user.getUserId())
                .username(user.getUsername())
                .accessToken(newAccessToken)
                .build();

        return AuthResponse.builder()
                .userInfo(userResponseDTO)
                .build();
    }
}
