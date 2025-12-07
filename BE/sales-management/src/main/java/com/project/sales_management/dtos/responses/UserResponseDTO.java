package com.project.sales_management.dtos.responses;

import lombok.*;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
@Getter
public class UserResponseDTO {
    Long id;
    String username;
    String accessToken;
}
