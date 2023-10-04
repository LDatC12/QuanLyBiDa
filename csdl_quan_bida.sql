CREATE DATABASE QuanLyQuanBida
GO

USE QuanLyQuanBida
GO

-- food, drinnk
-- table
-- foodcatagory, drinkcatagory
-- account
-- bill: bill chung
-- bill info: 1 bill nhieu mon an

CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'-- Trống || Có người
)
GO

CREATE TABLE Account 
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Dat',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0

	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: Đã thanh toàn && 0: Chưa thanh toán

	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)

GO
INSERT INTO dbo.Account
	   ( UserName, 
		 DisplayName,
		 PassWord,
		 Type
	   )
VALUES ( N'K9',
		 N'RongK9',
		 N'1',
		 1
	   )

INSERT INTO dbo.Account
	   ( UserName, 
		 DisplayName,
		 PassWord,
		 Type
	   )
VALUES ( N'staff',
		 N'staff',
		 N'1',
		 0
	   )
GO

CREATE PROC USP_GetAccountByUserName 
@userName nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'K9' -- nvarchar(50)

GO

CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO

-- Thêm bàn
DECLARE @i INT = 0

WHILE @i <= 10
BEGIN
	INSERT dbo.TableFood (name) VALUES (N'Bàn ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END 
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.TableFood
GO

EXEC dbo.USP_GetTableList

-- Thêm category
INSERT dbo.FoodCategory
	(name)
VALUES (N'Đồ ăn vặt')
INSERT dbo.FoodCategory
	(name)
VALUES (N'Nước uống') 
INSERT dbo.FoodCategory
	(name)
VALUES (N'Cơm')
INSERT dbo.FoodCategory
	(name)
VALUES (N'Trái cây')

INSERT dbo.FoodCategory
	(name)
VALUES (N'Xúc xích')

-- Thêm món ăn
INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Bánh tráng trộn', 1, 10000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Tóp mỡ', 1, 15000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Sting', 2, 10000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Coca', 2, 10000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Cơm gà', 3, 25000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Cơm trộn', 3, 30000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Trái cây dĩa', 4, 20000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Trái cây tô', 4, 20000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Xúc xích Pony', 5, 20000)

INSERT dbo.Food
	(name, idCategory, price)
VALUES (N'Xúc xích Đức', 5, 20000)

-- thêm Bill
INSERT dbo.Bill (
		DateCheckIn,
		DateCheckOut,
		idTable, 
		status)
VALUES (GETDATE(),
		NULL,
		1,
		0
)

INSERT dbo.Bill (
		DateCheckIn,
		DateCheckOut,
		idTable, 
		status)
VALUES (GETDATE(),
		NULL,
		2,
		0
)
INSERT dbo.Bill (
		DateCheckIn,
		DateCheckOut,
		idTable, 
		status)
VALUES (GETDATE(),
		GETDATE(),
		3,
		1
)

-- thêm bill info
INSERT dbo.BillInfo
		(idBill, idFood, count )
VALUES (1, 
		1,
		2
)

INSERT dbo.BillInfo
		(idBill, idFood, count )
VALUES (1, 
		3,
		4
)
INSERT dbo.BillInfo
(idBill, idFood, count )
VALUES (2, 
		5,
		1
)
INSERT dbo.BillInfo
(idBill, idFood, count )
VALUES (2, 
		1,
		2
)
INSERT dbo.BillInfo
(idBill, idFood, count )
VALUES (3, 
		6,
		2
)
INSERT dbo.BillInfo
(idBill, idFood, count )
VALUES (3, 
		5,
		2
)
GO
SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f
WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.idTable = 3

SELECT * FROM dbo.Bill
SELECT * FROM dbo.BillInfo
SELECT * FROM dbo.Food
SELECT * FROM dbo.FoodCategory
