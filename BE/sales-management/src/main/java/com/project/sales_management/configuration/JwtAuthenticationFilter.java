package com.project.sales_management.configuration;

import com.project.sales_management.services.Impl.CustomUserDetailsServices;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;

@Component
@Slf4j
@RequiredArgsConstructor
public class JwtAuthenticationFilter extends OncePerRequestFilter {
    private final JwtUtils jwtUtils;
    private final CustomUserDetailsServices userDetailsService;

    @Override
    protected void doFilterInternal(
            HttpServletRequest request, HttpServletResponse response, FilterChain filterChain
    ) throws ServletException, IOException {

        String header = request.getHeader("Authorization");
        if(header != null && header.startsWith("Bearer ")) {
            try{
                String token = header.substring(7);
                String username = jwtUtils.getUsernameFromToken(token);
                log.info("username : " + username);

                if (username != null && SecurityContextHolder.getContext().getAuthentication() == null) {
                    UserDetails userDetails = userDetailsService.loadUserByUsername(username);

                    if (jwtUtils.validateToken(token)) {
                        log.info("token valid");
                        UsernamePasswordAuthenticationToken authToken =
                                new UsernamePasswordAuthenticationToken(
                                        userDetails, null, userDetails.getAuthorities()
                                );
                        authToken.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
                        SecurityContextHolder.getContext().setAuthentication(authToken);
                        log.info("Authentication set for user: " + username);
                    }
                }
            } catch (Exception e){
                log.info("Cannot set user authentication: {}", e.getMessage());
                log.error(e.getMessage());
            }
        }

        filterChain.doFilter(request, response);
    }

    private String parseJwtToken(HttpServletRequest request) {
        String jwtFromHeader = jwtUtils.getTokenFromHeader(request);

        if(jwtFromHeader != null){
            return jwtFromHeader;
        }

        return null;
    }
}
