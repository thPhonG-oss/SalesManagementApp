package com.project.sales_management.controllers;

import com.project.sales_management.dtos.requests.ChangePasswordRequest;
import com.project.sales_management.services.UserService;
import jakarta.validation.Valid;
import lombok.Getter;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequiredArgsConstructor
@RequestMapping("/api/v1/users")
@FieldDefaults(level = lombok.AccessLevel.PRIVATE, makeFinal = true)
public class UserController {

    UserService userService;

    @PostMapping("/change-password")
    public ResponseEntity<Object> changePassword(@RequestBody @Valid ChangePasswordRequest request) {
        return ResponseEntity.ok().body(userService.changePassword(request));
    }
}
