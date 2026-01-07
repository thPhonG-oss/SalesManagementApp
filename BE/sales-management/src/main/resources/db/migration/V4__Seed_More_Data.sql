/*
 * V5__Seed_Large_Data.sql
 * MỤC TIÊU: Insert thêm 50 Customers và 50 Orders để test hiệu năng và phân trang
 * LƯU Ý: Script này sử dụng biến @ord_id, hãy chạy trên MySQL Workbench hoặc Tool quản trị DB.
 */

-- =============================================
-- 1. INSERT 50 KHÁCH HÀNG MỚI (Từ ID tự tăng)
-- =============================================
INSERT INTO customers (customer_name, phone, email, address) VALUES
('Lê Đức Thắng', '0930275593', 'thang.le@gmail.com', '485 Trần Hưng Đạo, Q5, TP.HCM'),
('Võ Thanh Linh', '0900317342', 'linh.vo@gmail.com', '59 Điện Biên Phủ, Q1, Hà Nội'),
('Vũ Hoàng Huy', '0968668572', 'huy.vu@gmail.com', '754 Pasteur, Bình Thạnh, TP.HCM'),
('Lý Hoàng Nam', '0980253092', 'nam.ly@gmail.com', '628 Nguyễn Huệ, Q3, Cần Thơ'),
('Đỗ Phương Giang', '0953580524', 'giang.do@gmail.com', '421 Hai Bà Trưng, Bình Thạnh, Hải Phòng'),
('Hoàng Văn Quỳnh', '0997267805', 'quynh.hoang@gmail.com', '678 Hai Bà Trưng, Q10, Hải Phòng'),
('Hoàng Quang Nam', '0985224169', 'nam.hoang@gmail.com', '494 Điện Biên Phủ, Q10, Đà Nẵng'),
('Ngô Phương Trung', '0967886174', 'trung.ngo@gmail.com', '119 Pasteur, Bình Thạnh, Hải Phòng'),
('Hoàng Công Thịnh', '0996211307', 'thinh.hoang@gmail.com', '40 Nguyễn Thị Minh Khai, Q1, TP.HCM'),
('Trần Thị Phúc', '0927786212', 'phuc.tran@gmail.com', '84 Trần Hưng Đạo, Gò Vấp, Hà Nội'),
('Vũ Mỹ Lan', '0951209752', 'lan.vu@gmail.com', '95 Phạm Văn Đồng, Q3, Đà Nẵng'),
('Phan Văn Hùng', '0933269771', 'hung.phan@gmail.com', '601 Nguyễn Huệ, Q10, Hà Nội'),
('Võ Công Tùng', '0937374450', 'tung.vo@gmail.com', '81 Pasteur, Q1, TP.HCM'),
('Dương Thị Yến', '0979351622', 'yen.duong@gmail.com', '887 Nguyễn Huệ, Tân Bình, Hà Nội'),
('Hoàng Công An', '0926521637', 'an.hoang@gmail.com', '910 Nguyễn Thị Minh Khai, Q10, Hà Nội'),
('Phạm Thanh Quỳnh', '0905672903', 'quynh.pham@gmail.com', '712 Lê Lợi, Q10, Hà Nội'),
('Nguyễn Thanh Tùng', '0997457654', 'tung.nguyen@gmail.com', '452 Cách Mạng Tháng 8, Q3, Đà Nẵng'),
('Nguyễn Quang Minh', '0925393262', 'minh.nguyen@gmail.com', '785 Pasteur, Q3, Cần Thơ'),
('Huỳnh Công Giang', '0937967420', 'giang.huynh@gmail.com', '339 Lê Lợi, Tân Bình, Hải Phòng'),
('Dương Hoàng Thịnh', '0959808027', 'thinh.duong@gmail.com', '788 Pasteur, Gò Vấp, Đà Nẵng'),
('Dương Quang Trung', '0971902201', 'trung.duong@gmail.com', '65 Cách Mạng Tháng 8, Q5, Cần Thơ'),
('Võ Minh Quân', '0968602979', 'quan.vo@gmail.com', '158 Lê Lợi, Tân Bình, Hải Phòng'),
('Dương Ngọc Trang', '0907031047', 'trang.duong@gmail.com', '115 Hai Bà Trưng, Q1, TP.HCM'),
('Đỗ Minh Việt', '0980777526', 'viet.do@gmail.com', '435 Trần Hưng Đạo, Gò Vấp, Đà Nẵng'),
('Võ Minh Long', '0995053848', 'long.vo@gmail.com', '248 Cách Mạng Tháng 8, Q10, Hà Nội'),
('Trần Hoàng Dũng', '0940520648', 'dung.tran@gmail.com', '912 Võ Văn Kiệt, Q3, Hà Nội'),
('Bùi Quang Giang', '0984318683', 'giang.bui@gmail.com', '107 Điện Biên Phủ, Bình Thạnh, Hà Nội'),
('Vũ Phương Phúc', '0951155243', 'phuc.vu@gmail.com', '557 Trần Hưng Đạo, Q5, Hải Phòng'),
('Đỗ Minh Hùng', '0944840177', 'hung.do@gmail.com', '513 Nguyễn Thị Minh Khai, Phú Nhuận, Hải Phòng'),
('Bùi Ngọc Vân', '0976442008', 'van.bui@gmail.com', '854 Võ Văn Kiệt, Q1, Hà Nội'),
('Phan Hữu Lan', '0945448403', 'lan.phan@gmail.com', '557 Nguyễn Huệ, Gò Vấp, Hà Nội'),
('Ngô Thanh Khánh', '0986468098', 'khanh.ngo@gmail.com', '450 Pasteur, Tân Bình, Cần Thơ'),
('Huỳnh Thanh Tùng', '0990589577', 'tung.huynh@gmail.com', '515 Pasteur, Q3, Đà Nẵng'),
('Bùi Quang Dũng', '0921771926', 'dung.bui@gmail.com', '889 Phạm Văn Đồng, Gò Vấp, TP.HCM'),
('Hoàng Phương Thịnh', '0904598738', 'thinh.hoang@gmail.com', '420 Phạm Văn Đồng, Bình Thạnh, Cần Thơ'),
('Hoàng Ngọc Thảo', '0901131561', 'thao.hoang@gmail.com', '116 Nguyễn Huệ, Q1, Hà Nội'),
('Dương Thị Nga', '0948589216', 'nga.duong@gmail.com', '110 Võ Văn Kiệt, Q1, Đà Nẵng'),
('Vũ Văn Bình', '0990204194', 'binh.vu@gmail.com', '598 Cách Mạng Tháng 8, Tân Bình, Hà Nội'),
('Phan Công An', '0974824146', 'an.phan@gmail.com', '392 Cách Mạng Tháng 8, Q10, Cần Thơ'),
('Vũ Đức Thịnh', '0924080371', 'thinh.vu@gmail.com', '417 Điện Biên Phủ, Phú Nhuận, TP.HCM'),
('Trần Thanh Dũng', '0931425511', 'dung.tran@gmail.com', '747 Cách Mạng Tháng 8, Tân Bình, TP.HCM'),
('Huỳnh Văn Nam', '0921431379', 'nam.huynh@gmail.com', '780 Trần Hưng Đạo, Phú Nhuận, TP.HCM'),
('Vũ Hữu Thắng', '0991429590', 'thang.vu@gmail.com', '722 Pasteur, Tân Bình, Đà Nẵng'),
('Trần Công Huy', '0902188466', 'huy.tran@gmail.com', '870 Hai Bà Trưng, Q1, TP.HCM'),
('Vũ Văn Lan', '0958043799', 'lan.vu@gmail.com', '566 Phạm Văn Đồng, Q5, Cần Thơ'),
('Võ Văn Tùng', '0953097809', 'tung.vo@gmail.com', '416 Lê Lợi, Gò Vấp, TP.HCM'),
('Đặng Thanh Hải', '0952067831', 'hai.dang@gmail.com', '856 Điện Biên Phủ, Q5, Hà Nội'),
('Hồ Công Quân', '0954230755', 'quan.ho@gmail.com', '322 Phạm Văn Đồng, Q1, Cần Thơ'),
('Huỳnh Minh Quỳnh', '0915291102', 'quynh.huynh@gmail.com', '583 Võ Văn Kiệt, Bình Thạnh, TP.HCM'),
('Đỗ Văn Hải', '0959468951', 'hai.do@gmail.com', '221 Hai Bà Trưng, Bình Thạnh, Hải Phòng');

-- =============================================
-- 2. INSERT 50 ĐƠN HÀNG (KÈM ORDER ITEMS)
-- =============================================

-- Order 1: Đã thanh toán
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (2, 'ORD-AUTO-001', '2025-11-24 06:47:54', 'PAID', 390000, 0, 390000, 'BANK_TRANSFER', '59 Điện Biên Phủ, Q1, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 35, 1, 120000, 120000),
(@ord_id, 62, 1, 80000, 80000),
(@ord_id, 8, 2, 95000, 190000);

-- Order 2: Mua ít
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (2, 'ORD-AUTO-002', '2026-01-06 20:04:02', 'PAID', 120000, 0, 120000, 'CREDIT_CARD', '59 Điện Biên Phủ, Q1, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 77, 1, 120000, 120000);

-- Order 3: Mới tạo (Created)
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (25, 'ORD-AUTO-003', '2025-11-07 20:58:49', 'CREATED', 930000, 0, 930000, 'CREDIT_CARD', '248 Cách Mạng Tháng 8, Q10, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 60, 3, 150000, 450000),
(@ord_id, 12, 3, 120000, 360000),
(@ord_id, 17, 1, 120000, 120000);

-- Order 4: Đã hủy (Cancelled)
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (19, 'ORD-AUTO-004', '2025-09-30 23:09:25', 'CANCELLED', 1141000, 0, 1141000, 'CREDIT_CARD', '339 Lê Lợi, Tân Bình, Hải Phòng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 89, 1, 75000, 75000),
(@ord_id, 61, 2, 80000, 160000),
(@ord_id, 87, 1, 189000, 189000),
(@ord_id, 65, 3, 189000, 567000),
(@ord_id, 85, 1, 150000, 150000);

-- Order 5: Giá trị cao
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (41, 'ORD-AUTO-005', '2025-11-29 15:48:57', 'CREATED', 1335000, 0, 1335000, 'CREDIT_CARD', '747 Cách Mạng Tháng 8, Tân Bình, TP.HCM');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 42, 2, 50000, 100000),
(@ord_id, 32, 2, 75000, 150000),
(@ord_id, 51, 2, 120000, 240000),
(@ord_id, 82, 1, 95000, 95000),
(@ord_id, 99, 3, 250000, 750000);

-- Order 6
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (8, 'ORD-AUTO-006', '2026-01-03 01:41:33', 'PAID', 1380000, 0, 1380000, 'CASH_ON_DELIVERY', '119 Pasteur, Bình Thạnh, Hải Phòng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 19, 3, 95000, 285000),
(@ord_id, 52, 3, 95000, 285000),
(@ord_id, 7, 3, 150000, 450000),
(@ord_id, 21, 3, 120000, 360000);

-- Order 7
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (45, 'ORD-AUTO-007', '2025-11-30 04:11:03', 'PAID', 475000, 0, 475000, 'CREDIT_CARD', '566 Phạm Văn Đồng, Q5, Cần Thơ');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 68, 2, 95000, 190000),
(@ord_id, 58, 3, 95000, 285000);

-- Order 8
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (47, 'ORD-AUTO-008', '2025-10-12 10:08:52', 'PAID', 230000, 0, 230000, 'BANK_TRANSFER', '856 Điện Biên Phủ, Q5, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 30, 1, 80000, 80000),
(@ord_id, 94, 1, 150000, 150000);

-- Order 9
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (38, 'ORD-AUTO-009', '2025-12-25 21:04:53', 'CREATED', 1370000, 0, 1370000, 'CREDIT_CARD', '598 Cách Mạng Tháng 8, Tân Bình, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 38, 3, 80000, 240000),
(@ord_id, 11, 2, 75000, 150000),
(@ord_id, 43, 2, 250000, 500000),
(@ord_id, 62, 3, 50000, 150000),
(@ord_id, 9, 3, 110000, 330000);

-- Order 10
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (12, 'ORD-AUTO-010', '2025-11-01 22:05:44', 'PAID', 1724000, 0, 1724000, 'CREDIT_CARD', '601 Nguyễn Huệ, Q10, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 38, 1, 189000, 189000),
(@ord_id, 60, 3, 110000, 330000),
(@ord_id, 31, 1, 95000, 95000),
(@ord_id, 99, 3, 250000, 750000),
(@ord_id, 34, 3, 120000, 360000);

-- Order 11
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (12, 'ORD-AUTO-011', '2025-10-24 20:30:58', 'PAID', 300000, 0, 300000, 'CREDIT_CARD', '601 Nguyễn Huệ, Q10, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 76, 2, 150000, 300000);

-- Order 12
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (3, 'ORD-AUTO-012', '2025-11-06 11:53:40', 'PAID', 95000, 0, 95000, 'CASH_ON_DELIVERY', '754 Pasteur, Bình Thạnh, TP.HCM');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 86, 1, 95000, 95000);

-- Order 13
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (1, 'ORD-AUTO-013', '2025-11-07 18:48:30', 'CREATED', 240000, 0, 240000, 'BANK_TRANSFER', '123 Đường Lê Lợi, Q1, TP.HCM');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 98, 3, 80000, 240000);

-- Order 14: Nhiều món
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (33, 'ORD-AUTO-014', '2025-10-23 22:11:32', 'PAID', 920000, 0, 920000, 'CREDIT_CARD', '515 Pasteur, Q3, Đà Nẵng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 41, 1, 150000, 150000),
(@ord_id, 75, 1, 80000, 80000),
(@ord_id, 20, 2, 95000, 190000),
(@ord_id, 9, 3, 150000, 450000),
(@ord_id, 36, 1, 50000, 50000);

-- Order 15: Có giảm giá
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (53, 'ORD-AUTO-015', '2025-12-03 09:23:02', 'CANCELLED', 1110000, 50000, 1060000, 'CREDIT_CARD', '221 Hai Bà Trưng, Bình Thạnh, Hải Phòng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 31, 2, 150000, 300000),
(@ord_id, 57, 2, 150000, 300000),
(@ord_id, 99, 3, 120000, 360000),
(@ord_id, 70, 1, 150000, 150000);

-- Order 16
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (14, 'ORD-AUTO-016', '2025-11-26 14:34:40', 'CANCELLED', 660000, 0, 660000, 'CREDIT_CARD', '887 Nguyễn Huệ, Tân Bình, Hà Nội');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 45, 3, 120000, 360000),
(@ord_id, 27, 2, 150000, 300000);

-- Order 17
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (24, 'ORD-AUTO-017', '2025-10-11 19:28:00', 'CREATED', 1368000, 0, 1368000, 'CREDIT_CARD', '435 Trần Hưng Đạo, Gò Vấp, Đà Nẵng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 88, 1, 250000, 250000),
(@ord_id, 96, 3, 150000, 450000),
(@ord_id, 76, 2, 189000, 378000),
(@ord_id, 50, 1, 50000, 50000),
(@ord_id, 94, 2, 120000, 240000);

-- Order 18: Giảm giá 50k
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (44, 'ORD-AUTO-018', '2026-01-07 13:04:26', 'PAID', 870000, 50000, 820000, 'BANK_TRANSFER', '870 Hai Bà Trưng, Q1, TP.HCM');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 85, 3, 80000, 240000),
(@ord_id, 55, 3, 120000, 360000),
(@ord_id, 77, 1, 120000, 120000),
(@ord_id, 58, 3, 50000, 150000);

-- Order 19
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (1, 'ORD-AUTO-019', '2025-10-04 19:11:21', 'CANCELLED', 50000, 0, 50000, 'BANK_TRANSFER', '123 Đường Lê Lợi, Q1, TP.HCM');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 78, 1, 50000, 50000);

-- Order 20
INSERT INTO orders (customer_id, order_code, order_date, status, subtotal, discount_amount, total_amount, payment_method, shipping_address)
VALUES (7, 'ORD-AUTO-020', '2025-10-20 02:41:06', 'PAID', 245000, 0, 245000, 'CREDIT_CARD', '494 Điện Biên Phủ, Q10, Đà Nẵng');
SET @ord_id = LAST_INSERT_ID();
INSERT INTO order_items (order_id, product_id, quantity, unit_price, total_price) VALUES
(@ord_id, 72, 2, 75000, 150000),
(@ord_id, 9, 1, 95000, 95000);