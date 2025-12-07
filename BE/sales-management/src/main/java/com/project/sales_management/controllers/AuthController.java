package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.LoginRequest;
import com.project.sales_management.dtos.requests.RegisterRequest;
import com.project.sales_management.dtos.responses.AuthResponse;
import com.project.sales_management.dtos.responses.UserResponseDTO;
import com.project.sales_management.services.Impl.AuthService;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseCookie;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Map;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/auth")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class AuthController {
    AuthService authService;

    @PostMapping("/register")
    public ResponseEntity<UserResponseDTO> register(@RequestBody RegisterRequest registerRequest){
        return new ResponseEntity<>(authService.register(registerRequest), HttpStatus.CREATED);
    }

    @PostMapping("/login")
    public ResponseEntity<UserResponseDTO> login(@RequestBody LoginRequest loginRequest){
        AuthResponse authResponse = authService.login(loginRequest);
        return ResponseEntity.ok()
                .header(HttpHeaders.SET_COOKIE, authResponse.getJwtCookie().toString())
                .body(authResponse.getUserInfo());
    }

    @PostMapping("/authenticate")
    public ResponseEntity<Object> authenticate(@RequestBody Map<String, String> token){
        boolean isValid = authService.authenticate(token.get("token"));
        return ResponseEntity.ok().body(isValid);
    }

    @PostMapping("/logout")
    public ResponseEntity<Object> logout(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse){
        ResponseCookie clearJwtCookie = authService.logout(httpServletRequest, httpServletResponse);
        return ResponseEntity.ok()
                .header(HttpHeaders.SET_COOKIE, clearJwtCookie.toString())
                .body("Logout successfully");
    }

    @PostMapping("/refresh")
    public ResponseEntity<UserResponseDTO> refresh(HttpServletRequest request, HttpServletResponse response){
        UserResponseDTO userResponseDTO = authService.refreshAccessToken(request, response).getUserInfo();
        return ResponseEntity.ok().body(userResponseDTO);
    }
}
