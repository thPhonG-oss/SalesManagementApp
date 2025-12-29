package com.project.sales_management.dtos.responses;


import lombok.*;
import lombok.experimental.FieldDefaults;

import java.util.ArrayList;
import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Builder
@FieldDefaults(level = AccessLevel.PRIVATE)
public class ProductImportResponse {
    Boolean success;
    String message;
    Integer totalRows;
    Integer importedRows;
    Integer skippedRows;
    List<ImportError> errors = new ArrayList<>();
}
