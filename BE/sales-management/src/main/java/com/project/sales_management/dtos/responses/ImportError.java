package com.project.sales_management.dtos.responses;

import lombok.*;
import lombok.experimental.FieldDefaults;

@Data
@NoArgsConstructor
@AllArgsConstructor
@FieldDefaults(level = AccessLevel.PRIVATE)
@Builder
public class ImportError {
    Integer rowNumber;
    String fieldName;
    String errorMessage;
}
