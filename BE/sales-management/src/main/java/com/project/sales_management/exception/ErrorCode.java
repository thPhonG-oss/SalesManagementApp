package com.project.sales_management.exception;

import lombok.AccessLevel;
import lombok.Getter;
import lombok.experimental.FieldDefaults;
import org.springframework.http.HttpStatus;
import org.springframework.http.HttpStatusCode;

@Getter
@FieldDefaults(level = AccessLevel.PRIVATE)
public enum ErrorCode {
    UNAUTHENTICATED ("1001","UNAUTHENTICATED", HttpStatus.UNAUTHORIZED),
    UNAUTHORIZED("1002", "You do not have permission", HttpStatus.FORBIDDEN),
    INVALID_KEY("1003", "Uncategorized error", HttpStatus.BAD_REQUEST),
    EMAIL_EXIST("1007", "Email already exists", HttpStatus.BAD_REQUEST),
    PHONE_EXIST("1008", "Phone already exists", HttpStatus.BAD_REQUEST),
    CUSTOMER_NOT_FOUND("1009", "CUSTOMER_NOT_FOUND", HttpStatus.BAD_REQUEST),

    ;
    ErrorCode(String code, String message, HttpStatus httpStatusCode) {
        this.code = code;
        this.message = message;
        this.httpStatusCode= httpStatusCode;
    }
    private final String code;
    private  final String message;
    private HttpStatusCode httpStatusCode;
}
