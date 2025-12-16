/*
 * V2__Seed_Data.sql
 * Mô tả: Dữ liệu mẫu cho hệ thống bán sách
 * Bao gồm: 5 Category, 100+ Products, Users, Customers, Promotions
 */

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- 1. INSERT CATEGORIES (Danh mục)
-- Giả định ID sẽ là: 1-Văn học, 2-Kinh tế, 3-CNTT, 4-Kỹ năng sống, 5-Thiếu nhi
INSERT INTO categories (category_name, description, is_active) VALUES
('Văn học', 'Tiểu thuyết, truyện ngắn, văn học kinh điển trong và ngoài nước', TRUE),
('Kinh tế', 'Sách quản trị kinh doanh, bài học đầu tư, marketing', TRUE),
('Công nghệ thông tin', 'Sách lập trình, thuật toán, cơ sở dữ liệu, AI', TRUE),
('Tâm lý - Kỹ năng sống', 'Sách phát triển bản thân, chữa lành, nghệ thuật sống', TRUE),
('Thiếu nhi', 'Truyện tranh, sách giáo dục cho trẻ em', TRUE);

-- 3. INSERT PRODUCTS (Sách)
-- Lưu ý: category_id tương ứng với thứ tự insert ở trên

-- === NHÓM 1: VĂN HỌC (Category ID = 1) ===
INSERT INTO products (category_id, product_name, description, author, publisher, publication_year, price, stock_quantity) VALUES
(1, 'Nhà Giả Kim', 'Cuốn sách bán chạy nhất mọi thời đại về hành trình theo đuổi ước mơ.', 'Paulo Coelho', 'NXB Hội Nhà Văn', 2020, 79000, 100),
(1, 'Cây Cam Ngọt Của Tôi', 'Câu chuyện cảm động về chú bé Zezé.', 'José Mauro de Vasconcelos', 'NXB Hội Nhà Văn', 2021, 108000, 80),
(1, 'Hoàng Tử Bé', 'Một câu chuyện triết lý nhẹ nhàng dành cho cả người lớn và trẻ em.', 'Antoine De Saint-Exupéry', 'NXB Kim Đồng', 2019, 50000, 150),
(1, 'Mắt Biếc', 'Một tác phẩm nổi tiếng của Nguyễn Nhật Ánh được chuyển thể thành phim.', 'Nguyễn Nhật Ánh', 'NXB Trẻ', 2019, 110000, 200),
(1, 'Rừng Na Uy', 'Tiểu thuyết nổi tiếng nhất của Haruki Murakami.', 'Haruki Murakami', 'NXB Hội Nhà Văn', 2021, 155000, 60),
(1, 'Số Đỏ', 'Kiệt tác văn học hiện thực phê phán Việt Nam.', 'Vũ Trọng Phụng', 'NXB Văn Học', 2018, 65000, 90),
(1, 'Tắt Đèn', 'Tác phẩm hiện thực xuất sắc của Ngô Tất Tố.', 'Ngô Tất Tố', 'NXB Văn Học', 2017, 55000, 100),
(1, 'Chí Phèo', 'Tập truyện ngắn kinh điển của Nam Cao.', 'Nam Cao', 'NXB Văn Học', 2020, 60000, 120),
(1, 'Dế Mèn Phiêu Lưu Ký', 'Tác phẩm văn học thiếu nhi kinh điển của Việt Nam.', 'Tô Hoài', 'NXB Kim Đồng', 2022, 45000, 300),
(1, 'Hai Số Phận', 'Câu chuyện về hai người đàn ông sinh cùng ngày nhưng khác số phận.', 'Jeffrey Archer', 'NXB Văn Học', 2018, 189000, 50),
(1, 'Bố Già', 'Tiểu thuyết tội phạm kinh điển.', 'Mario Puzo', 'NXB Văn Học', 2020, 145000, 75),
(1, 'Tuổi Thơ Dữ Dội', 'Ký ức về những chiến sĩ nhỏ tuổi.', 'Phùng Quán', 'NXB Kim Đồng', 2021, 130000, 90),
(1, 'Ông Già Và Biển Cả', 'Tác phẩm đoạt giải Nobel của Hemingway.', 'Ernest Hemingway', 'NXB Văn Học', 2019, 50000, 80),
(1, 'Không Gia Đình', 'Hành trình của cậu bé Remi.', 'Hector Malot', 'NXB Văn Học', 2020, 120000, 100),
(1, 'Những Người Khốn Khổ', 'Kiệt tác văn học Pháp.', 'Victor Hugo', 'NXB Văn Học', 2019, 350000, 30),
(1, 'Đất Rừng Phương Nam', 'Vẻ đẹp thiên nhiên và con người Nam Bộ.', 'Đoàn Giỏi', 'NXB Kim Đồng', 2020, 70000, 110),
(1, 'Tiếng Gọi Nơi Hoang Dã', 'Câu chuyện về chú chó Buck.', 'Jack London', 'NXB Văn Học', 2018, 60000, 90),
(1, 'Trăm Năm Cô Đơn', 'Kiệt tác của chủ nghĩa hiện thực huyền ảo.', 'Gabriel Garcia Marquez', 'NXB Văn Học', 2021, 160000, 40),
(1, 'Đồi Gió Hú', 'Câu chuyện tình yêu và thù hận.', 'Emily Bronte', 'NXB Văn Học', 2019, 95000, 60),
(1, 'Kiêu Hãnh Và Định Kiến', 'Tác phẩm kinh điển về tình yêu và hôn nhân.', 'Jane Austen', 'NXB Văn Học', 2020, 98000, 85);

-- === NHÓM 2: KINH TẾ (Category ID = 2) ===
INSERT INTO products (category_id, product_name, description, author, publisher, publication_year, price, stock_quantity) VALUES
(2, 'Dạy Con Làm Giàu - Tập 1', 'Để không có tiền vẫn tạo ra tiền.', 'Robert T. Kiyosaki', 'NXB Trẻ', 2021, 85000, 200),
(2, 'Nhà Đầu Tư Thông Minh', 'Cuốn sách gối đầu giường của mọi nhà đầu tư.', 'Benjamin Graham', 'NXB Lao Động', 2020, 189000, 150),
(2, 'Marketing Giỏi Phải Kiếm Được Tiền', 'Tư duy marketing thực chiến.', 'Sergio Zyman', 'NXB Lao Động', 2019, 129000, 80),
(2, 'Những Đòn Tâm Lý Trong Thuyết Phục', 'Nghệ thuật gây ảnh hưởng.', 'Robert B. Cialdini', 'NXB Lao Động', 2021, 139000, 100),
(2, 'Tuần Làm Việc 4 Giờ', 'Bí quyết làm ít hơn, kiếm nhiều hơn.', 'Timothy Ferriss', 'NXB Lao Động', 2020, 110000, 70),
(2, 'Từ Tốt Đến Vĩ Đại', 'Tại sao một số công ty đạt bước nhảy vọt còn số khác thì không.', 'Jim Collins', 'NXB Trẻ', 2019, 155000, 90),
(2, 'Chiến Tranh Tiền Tệ', 'Sự thật về lịch sử tài chính thế giới.', 'Song Hong Bing', 'NXB Lao Động', 2018, 160000, 120),
(2, 'Phi Lý Trí', 'Khám phá những động lực vô hình đằng sau quyết định của con người.', 'Dan Ariely', 'NXB Lao Động', 2021, 125000, 85),
(2, 'Khởi Nghiệp Tinh Gọn', 'Phương pháp khởi nghiệp hiệu quả.', 'Eric Ries', 'NXB Trẻ', 2020, 140000, 100),
(2, 'Tỷ Phú Bán Giày', 'Câu chuyện về Zappos và văn hóa doanh nghiệp.', 'Tony Hsieh', 'NXB Lao Động', 2019, 115000, 60),
(2, 'Bí Mật Tư Duy Triệu Phú', 'Sự khác biệt trong tư duy giữa người giàu và người nghèo.', 'T. Harv Eker', 'NXB Trẻ', 2022, 90000, 250),
(2, 'Thế Giới Phẳng', 'Toàn cầu hóa trong thế kỷ 21.', 'Thomas L. Friedman', 'NXB Trẻ', 2018, 280000, 40),
(2, 'Dẫn Đầu Hay Là Chết', 'Tư duy khác biệt trong kinh doanh.', 'Grant Cardone', 'NXB Lao Động', 2021, 150000, 50),
(2, 'Tư Duy Nhanh Và Chậm', 'Cách não bộ đưa ra quyết định.', 'Daniel Kahneman', 'NXB Thế Giới', 2020, 219000, 95),
(2, 'Tiền Làm Chủ Cuộc Chơi', '7 bước để tự do tài chính.', 'Tony Robbins', 'NXB Thế Giới', 2019, 250000, 60),
(2, 'Lược Sử Kinh Tế Học', 'Lịch sử kinh tế qua các thời kỳ.', 'Niall Kishtainy', 'NXB Trẻ', 2021, 145000, 70),
(2, 'Elon Musk - Tỷ Phú Bán Giấc Mơ', 'Tiểu sử Elon Musk.', 'Ashlee Vance', 'NXB Công Thương', 2019, 189000, 110),
(2, 'Shoe Dog - Gã Nghiện Giày', 'Hồi ký của nhà sáng lập Nike.', 'Phil Knight', 'NXB Trẻ', 2020, 175000, 130),
(2, 'Đọc Vị Bất Kỳ Ai', 'Để không bị lừa dối và lợi dụng.', 'David J. Lieberman', 'NXB Lao Động', 2018, 69000, 200),
(2, 'Quốc Gia Khởi Nghiệp', 'Câu chuyện về nền kinh tế thần kỳ của Israel.', 'Dan Senor', 'NXB Thế Giới', 2021, 165000, 90);

-- === NHÓM 3: CNTT - LẬP TRÌNH (Category ID = 3) ===
INSERT INTO products (category_id, product_name, description, author, publisher, publication_year, price, stock_quantity) VALUES
(3, 'Clean Code', 'Mã sạch và con đường trở thành lập trình viên chuyên nghiệp.', 'Robert C. Martin', 'NXB Thanh Niên', 2018, 350000, 50),
(3, 'The Pragmatic Programmer', 'Hành trình từ thợ code thành nghệ nhân.', 'David Thomas, Andrew Hunt', 'Addison-Wesley', 2019, 450000, 40),
(3, 'Introduction to Algorithms', 'Sách gối đầu giường về thuật toán.', 'Thomas H. Cormen', 'MIT Press', 2020, 900000, 20),
(3, 'Design Patterns', 'Các mẫu thiết kế hướng đối tượng tái sử dụng.', 'Erich Gamma', 'Addison-Wesley', 2017, 400000, 30),
(3, 'Head First Java', 'Học Java theo phong cách trực quan.', 'Kathy Sierra', 'O Reilly', 2019, 320000, 60),
(3, 'Code Dạo Ký Sự', 'Chuyện nghề lập trình tại Việt Nam.', 'Phạm Huy Hoàng', 'NXB Thanh Niên', 2018, 110000, 150),
(3, 'Tớ Học Lập Trình', 'Sách lập trình Scratch cho trẻ em.', 'Nhiều tác giả', 'NXB Kim Đồng', 2021, 80000, 100),
(3, 'Spring in Action', 'Hướng dẫn toàn diện về Spring Framework.', 'Craig Walls', 'Manning', 2020, 500000, 25),
(3, 'You Don\'t Know JS', 'Hiểu sâu về JavaScript.', 'Kyle Simpson', 'O Reilly', 2019, 250000, 45),
(3, 'Clean Architecture', 'Kiến trúc phần mềm bền vững.', 'Robert C. Martin', 'Prentice Hall', 2018, 380000, 35),
(3, 'Refactoring', 'Cải thiện thiết kế mã nguồn hiện có.', 'Martin Fowler', 'Addison-Wesley', 2019, 420000, 30),
(3, 'Domain-Driven Design', 'Thiết kế theo miền dữ liệu.', 'Eric Evans', 'Addison-Wesley', 2017, 480000, 20),
(3, 'Grokking Algorithms', 'Thuật toán minh họa dễ hiểu.', 'Aditya Bhargava', 'Manning', 2018, 300000, 55),
(3, 'Effective Java', 'Các phương pháp hay nhất khi code Java.', 'Joshua Bloch', 'Addison-Wesley', 2018, 360000, 40),
(3, 'Cracking the Coding Interview', '189 câu hỏi phỏng vấn lập trình.', 'Gayle Laakmann McDowell', 'CareerCup', 2020, 450000, 50),
(3, 'Python Crash Course', 'Học nhanh lập trình Python.', 'Eric Matthes', 'No Starch Press', 2019, 350000, 65),
(3, 'Database System Concepts', 'Kiến thức nền tảng về cơ sở dữ liệu.', 'Abraham Silberschatz', 'McGraw-Hill', 2020, 600000, 15),
(3, 'Docker Deep Dive', 'Hiểu sâu về Container và Docker.', 'Nigel Poulton', 'Independently', 2021, 300000, 40),
(3, 'Kubernetes: Up and Running', 'Vận hành hệ thống với K8s.', 'Kelsey Hightower', 'O Reilly', 2019, 380000, 20),
(3, 'Soft Skills', 'Cẩm nang kỹ năng mềm cho lập trình viên.', 'John Sonmez', 'Manning', 2017, 250000, 80);

-- === NHÓM 4: TÂM LÝ - KỸ NĂNG (Category ID = 4) ===
INSERT INTO products (category_id, product_name, description, author, publisher, publication_year, price, stock_quantity) VALUES
(4, 'Đắc Nhân Tâm', 'Nghệ thuật thu phục lòng người.', 'Dale Carnegie', 'NXB Tổng Hợp TPHCM', 2022, 86000, 500),
(4, 'Quẳng Gánh Lo Đi Và Vui Sống', 'Cách vượt qua nỗi lo âu.', 'Dale Carnegie', 'NXB Tổng Hợp TPHCM', 2021, 78000, 300),
(4, 'Hạt Giống Tâm Hồn', 'Những câu chuyện nuôi dưỡng tâm hồn.', 'Nhiều tác giả', 'NXB Tổng Hợp TPHCM', 2020, 50000, 200),
(4, 'Hiểu Về Trái Tim', 'Nghệ thuật sống hạnh phúc.', 'Minh Niệm', 'NXB Tổng Hợp TPHCM', 2019, 138000, 150),
(4, 'Atomic Habits - Thay Đổi Tí Hon', 'Hiệu quả bất ngờ từ thói quen nhỏ.', 'James Clear', 'NXB Thế Giới', 2021, 189000, 120),
(4, 'Sức Mạnh Của Hiện Tại', 'Hướng dẫn giác ngộ tâm linh.', 'Eckhart Tolle', 'NXB Tổng Hợp TPHCM', 2020, 120000, 90),
(4, 'Muôn Kiếp Nhân Sinh', 'Khám phá quy luật nhân quả.', 'Nguyên Phong', 'NXB Tổng Hợp TPHCM', 2020, 220000, 250),
(4, 'Hành Trình Về Phương Đông', 'Huyền bí và minh triết.', 'Nguyên Phong', 'NXB Hội Nhà Văn', 2019, 110000, 180),
(4, 'Dám Bị Ghét', 'Tâm lý học Adler cho cuộc sống tự do.', 'Kishimi Ichiro', 'NXB Lao Động', 2018, 96000, 140),
(4, 'Đời Ngắn Đừng Ngủ Dài', 'Những bài học cuộc sống ngắn gọn.', 'Robin Sharma', 'NXB Trẻ', 2019, 75000, 210),
(4, 'Khéo Ăn Nói Sẽ Có Được Thiên Hạ', 'Kỹ năng giao tiếp.', 'Trác Nhã', 'NXB Văn Học', 2018, 110000, 130),
(4, 'Cân Bằng Cảm Xúc Cả Lúc Bão Giông', 'Quản trị cảm xúc.', 'Richard Nicholls', 'NXB Công Thương', 2020, 89000, 100),
(4, 'Lối Sống Tối Giản Của Người Nhật', 'Sống ít đi để hạnh phúc hơn.', 'Sasaki Fumio', 'NXB Lao Động', 2019, 85000, 115),
(4, 'Giận', 'Chuyển hóa năng lượng tiêu cực.', 'Thích Nhất Hạnh', 'NXB Hội Nhà Văn', 2017, 80000, 160),
(4, 'Tuổi Trẻ Đáng Giá Bao Nhiêu', 'Định hướng cho người trẻ.', 'Rosie Nguyễn', 'NXB Hội Nhà Văn', 2018, 80000, 400),
(4, 'Trên Đường Băng', 'Bài học khởi nghiệp và cuộc sống.', 'Tony Buổi Sáng', 'NXB Trẻ', 2019, 85000, 350),
(4, 'Nóng Giận Là Bản Năng Tĩnh Lặng Là Bản Lĩnh', 'Tu dưỡng tâm tính.', 'Tống Mặc', 'NXB Thế Giới', 2020, 89000, 125),
(4, 'Thao Túng Tâm Lý', 'Nhận diện và phòng tránh.', 'Shannon Thomas', 'NXB Lao Động', 2021, 149000, 95),
(4, 'Đừng Lựa Chọn An Nhàn Khi Còn Trẻ', 'Động lực phấn đấu.', 'Cảnh Thiên', 'NXB Thế Giới', 2019, 82000, 180),
(4, 'Phụ Nữ Thông Minh Phải Biết Tiêu Tiền', 'Quản lý tài chính cá nhân cho nữ.', 'Lois P. Frankel', 'NXB Lao Động', 2020, 110000, 70);

-- === NHÓM 5: THIẾU NHI (Category ID = 5) ===
INSERT INTO products (category_id, product_name, description, author, publisher, publication_year, price, stock_quantity) VALUES
(5, 'Doraemon - Tập 1', 'Chú mèo máy đến từ tương lai.', 'Fujiko F. Fujio', 'NXB Kim Đồng', 2022, 25000, 500),
(5, 'Conan - Tập 100', 'Thám tử lừng danh Conan.', 'Gosho Aoyama', 'NXB Kim Đồng', 2022, 25000, 450),
(5, 'Shin Cậu Bé Bút Chì', 'Câu chuyện hài hước về gia đình Shin.', 'Yoshito Usui', 'NXB Kim Đồng', 2021, 22000, 300),
(5, 'Kính Vạn Hoa', 'Tập truyện dài về tuổi học trò.', 'Nguyễn Nhật Ánh', 'NXB Kim Đồng', 2020, 85000, 200),
(5, 'Chuyện Con Mèo Dạy Hải Âu Bay', 'Câu chuyện về lời hứa.', 'Luis Sepulveda', 'NXB Hội Nhà Văn', 2019, 49000, 180),
(5, 'Charlotte và Wilbur', 'Tình bạn cảm động giữa nhện và lợn.', 'E.B. White', 'NXB Hội Nhà Văn', 2018, 60000, 120),
(5, 'Pippi Tất Dài', 'Cô bé tinh nghịch và khỏe mạnh nhất thế giới.', 'Astrid Lindgren', 'NXB Kim Đồng', 2020, 75000, 100),
(5, 'Alice Ở Xứ Sở Diệu Kỳ', 'Cuộc phiêu lưu vào thế giới tưởng tượng.', 'Lewis Carroll', 'NXB Văn Học', 2019, 65000, 90),
(5, 'Totto-chan Bên Cửa Sổ', 'Cuốn sách gối đầu giường về giáo dục trẻ em.', 'Kuroyanagi Tetsuko', 'NXB Nhã Nam', 2021, 98000, 150),
(5, 'Harry Potter và Hòn Đá Phù Thủy', 'Khởi đầu thế giới pháp thuật.', 'J.K. Rowling', 'NXB Trẻ', 2022, 185000, 300),
(5, 'Góc Sân Và Khoảng Trời', 'Thơ thiếu nhi.', 'Trần Đăng Khoa', 'NXB Kim Đồng', 2018, 45000, 140),
(5, 'Vừa Nhắm Mắt Vừa Mở Cửa Sổ', 'Thế giới trẻ thơ trong trẻo.', 'Nguyễn Ngọc Thuần', 'NXB Trẻ', 2020, 60000, 110),
(5, 'Cổ Tích Việt Nam', 'Tuyển tập truyện cổ tích.', 'Nhiều tác giả', 'NXB Văn Học', 2017, 120000, 200),
(5, 'Mười Vạn Câu Hỏi Vì Sao', 'Sách khoa học thường thức.', 'Nhiều tác giả', 'NXB Dân Trí', 2021, 95000, 180),
(5, 'Những Tấm Lòng Cao Cả', 'Nhật ký của cậu bé En-ri-cô.', 'Edmondo De Amicis', 'NXB Văn Học', 2019, 70000, 130),
(5, 'Lũ Trẻ Đường Tàu', 'Câu chuyện phiêu lưu ấm áp.', 'Edith Nesbit', 'NXB Văn Học', 2020, 80000, 90),
(5, 'Peter Pan', 'Cậu bé không bao giờ lớn.', 'J.M. Barrie', 'NXB Văn Học', 2018, 55000, 100),
(5, 'Bác Sĩ Dolittle', 'Người đàn ông biết nói chuyện với thú vật.', 'Hugh Lofting', 'NXB Kim Đồng', 2019, 60000, 80),
(5, 'Gió Qua Rặng Liễu', 'Câu chuyện đồng thoại kinh điển.', 'Kenneth Grahame', 'NXB Hội Nhà Văn', 2020, 72000, 70),
(5, 'Khu Vườn Bí Mật', 'Sức mạnh chữa lành của thiên nhiên.', 'Frances Hodgson Burnett', 'NXB Văn Học', 2021, 85000, 110);

-- 4. INSERT CUSTOMERS (Khách hàng)
INSERT INTO customers (customer_name, phone, email, address, total_orders, total_spent) VALUES
('Nguyễn Văn An', '0901234567', 'an.nguyen@gmail.com', '123 Đường Lê Lợi, Q1, TP.HCM', 0, 0),
('Trần Thị Bích', '0912345678', 'bich.tran@yahoo.com', '456 Đường Nguyễn Huệ, Q1, TP.HCM', 0, 0),
('Lê Hoàng Nam', '0987654321', 'nam.le@outlook.com', '789 Đường Điện Biên Phủ, Bình Thạnh, TP.HCM', 0, 0),
('Phạm Minh Tuấn', '0351234567', 'tuan.pham@gmail.com', '101 Đường Cầu Giấy, Hà Nội', 0, 0),
('Hoàng Thùy Linh', '0369876543', 'linh.hoang@gmail.com', '202 Đường Nguyễn Văn Linh, Đà Nẵng', 0, 0);

-- 5. INSERT PROMOTIONS (Khuyến mãi)
INSERT INTO promotions (
    promotion_code, promotion_name, description, discount_type, discount_value,
    min_order_value, max_discount_value, start_date, end_date, usage_limit
) VALUES
('WELCOME', 'Chào bạn mới', 'Giảm 50k cho đơn đầu tiên', 'FIXED_AMOUNT', 50000, 200000, 50000, NOW(), DATE_ADD(NOW(), INTERVAL 1 YEAR), 1000),
('FREESHIP', 'Miễn phí vận chuyển', 'Giảm tối đa 30k phí ship', 'FIXED_AMOUNT', 30000, 150000, 30000, NOW(), DATE_ADD(NOW(), INTERVAL 6 MONTH), 5000),
('SUMMER25', 'Hè rực rỡ', 'Giảm 10% cho đơn từ 500k', 'PERCENTAGE', 10.00, 500000, 100000, NOW(), DATE_ADD(NOW(), INTERVAL 3 MONTH), 200),
('BOOKLOVER', 'Mọt sách', 'Giảm 5% mọi đơn hàng', 'PERCENTAGE', 5.00, 0, 20000, NOW(), DATE_ADD(NOW(), INTERVAL 1 YEAR), 10000);

-- 6. INSERT ORDERS SAMPLE (Tùy chọn: để có dữ liệu test dashboard)
-- Tạo đơn hàng cho khách hàng Nguyễn Văn An (ID 1)
INSERT INTO orders (customer_id, order_code, subtotal, total_amount, payment_method, shipping_address)
VALUES (1, 'ORD-2023-001', 178000, 178000, 'CASH_ON_DELIVERY', '123 Đường Lê Lợi, Q1, TP.HCM');

-- Giả sử ID sách: Nhà Giả Kim (1) và Mắt Biếc (4)
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price)
VALUES
(1, 1, 1, 79000, 79000), -- Nhà giả kim
(1, 4, 1, 99000, 99000); -- Mắt biếc (giá giả định)