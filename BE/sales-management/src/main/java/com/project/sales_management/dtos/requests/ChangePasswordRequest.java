package com.project.sales_management.dtos.requests;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class ChangePasswordRequest {

    @NotBlank(message = "Old password must not be blank")
    private String oldPassword;

    @NotNull(message = "New password must not be null")
    private String newPassword;

    @NotNull(message = "Confirm new password must not be null")
    private String confirmNewPassword;
}
