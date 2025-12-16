Xin lỗi bạn vì sự bất tiện trước đó. Đây là toàn bộ nội dung file README.md nằm trong một block code duy nhất để bạn copy ngay lập tức:

Markdown

# Sales Management App - Backend API Guide

Tài liệu hướng dẫn cài đặt, cấu hình và chạy backend (Spring Boot) cho dự án **Sales Management App**. Tài liệu này dành cho team Frontend và Developers để thiết lập môi trường server local.

## 🛠 Yêu cầu hệ thống (Prerequisites)

Trước khi bắt đầu, đảm bảo máy của bạn đã cài đặt:

* **Java Development Kit (JDK)**: Phiên bản **21** (Project sử dụng Java 21).
* **MySQL Server**: Phiên bản 8.0 trở lên.
* **Maven**: Đã có sẵn Maven Wrapper (`mvnw`) trong project, không bắt buộc cài đặt Maven toàn cục.

## ⚙️ Cấu hình Môi trường (Environment Variables)

Backend sử dụng các biến môi trường để cấu hình kết nối Database, JWT và Cloudinary (được định nghĩa trong `src/main/resources/application-dev.yaml`).

Bạn cần thiết lập các biến này trong **Environment Variables** của IDE (IntelliJ, Eclipse, VS Code) hoặc tạo file script chạy.

### Danh sách biến môi trường bắt buộc

| Tên biến | Mô tả | Giá trị gợi ý (Local) |
| :--- | :--- | :--- |
| `PORT` | Cổng chạy server | `8080` (Mặc định FE đang trỏ vào cổng này) |
| `DATABASE_URL` | Đường dẫn kết nối MySQL | `jdbc:mysql://localhost:3306/sales_management_db?createDatabaseIfNotExist=true` |
| `DB_USERNAME` | Tên đăng nhập MySQL | `root` (hoặc user của bạn) |
| `DB_PASSWORD` | Mật khẩu MySQL | `123456` (hoặc pass của bạn) |
| `SECRET_KEY` | Khóa bí mật để ký JWT (HS512) | Chuỗi ngẫu nhiên dài (Ví dụ: `5367566B59703373367639792F423F4528482B4D6251655468576D5A71347437`) |
| `CLOUD_NAME` | Cloudinary Cloud Name | Tên cloud của bạn (đăng ký tại cloudinary.com) |
| `CLOUD_API_KEY` | Cloudinary API Key | API Key lấy từ Dashboard Cloudinary |
| `CLOUD_API_SECRET`| Cloudinary API Secret | API Secret lấy từ Dashboard Cloudinary |

> **Lưu ý quan trọng:**
> - Nếu không cấu hình `SECRET_KEY` đủ dài, ứng dụng có thể báo lỗi khi khởi động.
> - Nếu thiếu cấu hình Cloudinary, chức năng upload ảnh sản phẩm sẽ bị lỗi.

## 🚀 Hướng dẫn chạy ứng dụng

### Bước 1: Clone và mở project
Mở thư mục `BE/sales-management` bằng IDE (khuyên dùng IntelliJ IDEA).

### Bước 2: Tạo Database
Tạo một database rỗng trong MySQL tên là `sales_management_db` (hoặc tên tùy ý khớp với `DATABASE_URL` bạn cấu hình).

```sql
CREATE DATABASE sales_management_db;
Cơ chế Migration: Project sử dụng thư viện Flyway. Khi chạy lần đầu, Flyway sẽ tự động:

Tạo bảng (V1__Init_Schema.sql).

Insert dữ liệu mẫu (V2__Seed_Data.sql).

Bạn KHÔNG cần chạy file SQL thủ công.

Bước 3: Chạy ứng dụng
Cách 1: Sử dụng Command Line (Terminal) Tại thư mục gốc của backend (BE/sales-management):

Windows:

DOS

mvnw spring-boot:run -Dspring-boot.run.profiles=dev
Linux/macOS:

Bash

./mvnw spring-boot:run -Dspring-boot.run.profiles=dev
Cách 2: Sử dụng IntelliJ IDEA

Mở file src/main/java/com/project/sales_management/SalesManagementBackendApplication.java.

Nhấn chuột phải -> Run 'SalesManagementBackendApplication'.

Đảm bảo đã điền Environment Variables trong phần Edit Configurations.

📚 Tài liệu API (Swagger UI)
Sau khi server khởi động thành công, bạn có thể xem danh sách API và test thử trực tiếp tại:

Swagger UI: http://localhost:8080/swagger-ui/index.html

API Docs JSON: http://localhost:8080/v3/api-docs

🔐 Cơ chế Authentication & Hướng dẫn cho Frontend
Hệ thống sử dụng JWT (JSON Web Token) kết hợp với HttpOnly Cookie để tăng cường bảo mật.

1. Quy trình Đăng nhập (Login)
API: POST /api/v1/auth/login

Request Body:

JSON

{
    "username": "admin",
    "password": "admin"
}
Response: Server sẽ trả về 2 phần:

Body: Chứa thông tin User và accessToken (dùng cho các request tiếp theo).

Header (Set-Cookie): Chứa refreshToken trong HttpOnly Cookie (Frontend browser tự động lưu, JS không đọc được).

2. Gửi Request (Authenticated Requests)
Frontend cần lấy accessToken từ response login và đính kèm vào Header của mọi request cần quyền truy cập:

Authorization: Bearer <your_access_token>
3. Refresh Token (Khi Access Token hết hạn)
Access Token có hiệu lực ngắn (1 giờ). Khi hết hạn (gặp lỗi 401 hoặc 403), Frontend gọi API:

API: POST /api/v1/auth/refresh

Cơ chế: Browser sẽ tự động gửi cookie refreshToken đi kèm request này. Server sẽ trả về accessToken mới.

Lưu ý cho FE: Khi gọi API này, cần set withCredentials: true (hoặc tương đương) trong thư viện HTTP Client (Axios, Fetch) để gửi kèm Cookie.

4. Đăng xuất (Logout)
API: POST /api/v1/auth/logout

Server sẽ xóa Cookie refreshToken. Frontend cần chủ động xóa accessToken đang lưu ở Client.

👤 Tài khoản Test mặc định
Khi chạy lần đầu, hệ thống tự động tạo tài khoản Admin (trong file ApplicationInitConfig.java):

Username: admin

Password: admin

Ngoài ra, file V2__Seed_Data.sql cũng tạo sẵn dữ liệu mẫu cho Categories và Products để Frontend hiển thị lên UI ngay lập tức.

⚠️ Các lỗi thường gặp (Troubleshooting)
Lỗi Communications link failure:

Kiểm tra MySQL đã bật chưa.

Kiểm tra DATABASE_URL, user/pass trong Environment Variables.

Lỗi Java version mismatch:

Đảm bảo IDE và Terminal đang sử dụng JDK 21. Kiểm tra bằng lệnh java -version.

Lỗi BeanCreationException liên quan đến JWT:

Do SECRET_KEY quá ngắn hoặc chưa được cấu hình. Hãy tạo một chuỗi ngẫu nhiên dài hơn (ít nhất 32 ký tự).