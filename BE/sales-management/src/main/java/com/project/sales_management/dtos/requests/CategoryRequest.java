package com.project.sales_management.dtos.requests;

import jakarta.persistence.Column;
import lombok.*;
import lombok.experimental.FieldDefaults;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class CategoryRequest {
    String categoryName;
    String description;
    Boolean isActive;

}
