package com.project.sales_management.configuration;

import com.project.sales_management.models.User;
import com.project.sales_management.repositories.UserRepository;
import lombok.AccessLevel;
import lombok.RequiredArgsConstructor;
import lombok.experimental.FieldDefaults;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.ApplicationRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.password.PasswordEncoder;

@Configuration
@FieldDefaults(level = AccessLevel.PRIVATE, makeFinal = true)
@Slf4j
@RequiredArgsConstructor
public class ApplicationInitConfig {
    UserRepository userRepository;
    PasswordEncoder passwordEncoder;

    @Bean
    ApplicationRunner init(UserRepository userRepository) {
        return args -> {
            if(!userRepository.findByUsername("admin").isPresent()) {
                User adminUser = User.builder()
                        .username("admin")
                        .password(passwordEncoder.encode("admin")) // "admin123" encoded with BCrypt
                        .build();

                userRepository.save(adminUser);
            }
        };
    }
}
