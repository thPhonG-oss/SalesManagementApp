@echo off
title Sales Management App Runner

echo ========================================================
echo  CAU HINH MOI TRUONG VA KHOI DONG UNG DUNG
echo ========================================================

:: 1. CAU HINH JAVA 21 (Bat buoc)
set "JAVA_HOME=C:\Program Files\Java\jdk-21"
set "Path=%JAVA_HOME%\bin;%Path%"

:: 2. CAU HINH DATABASE (Khop voi Docker MySQL cua ban)
:: Luu y: Port o day la port DATABASE (3306), khong phai port Server
set "DATABASE_URL=jdbc:mysql://localhost:3306/sales_management?useSSL=false&allowPublicKeyRetrieval=true&serverTimezone=UTC"
set "DB_USERNAME=root"
set "DB_PASSWORD=123456"

:: 3. CAU HINH BAO MAT (JWT Secret Key)
set "SECRET_KEY="

:: 4. CAU HINH CLOUDINARY (Upload anh)
set "CLOUD_NAME="
set "CLOUD_API_KEY="
set "CLOUD_API_SECRET="

:: 5. KIEM TRA VA CHAY
echo.
echo --- Kiem tra Java ---
java -version
echo.
echo --- Dang khoi dong Spring Boot tren PORT 8081 ---

:: QUAN TRONG: Them tham so --server.port=8081 vao cuoi dong de ep chay cong nay
call mvnw.cmd spring-boot:run -Dspring-boot.run.profiles=dev -Dspring-boot.run.arguments="--server.port=8081"

pause
