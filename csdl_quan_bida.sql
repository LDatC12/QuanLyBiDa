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
DBCC CHECKIDENT (TableFood, RESEED, 0); -- reset IDENTITY 
GO
CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'-- Trống || Có người
)


ALTER TABLE TableFood
ADD timestart DATETIME

UPDATE dbo.TableFood SET timestart = NULL

SELECT * FROM TableFood

GO

CREATE TABLE Account 
(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Dat',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL DEFAULT 0 -- 1: admin && 0: staff
)
GO

DBCC CHECKIDENT (FoodCategory, RESEED, 0); -- reset IDENTITY cho id chạy bắt đầu từ 1
GO
CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

DBCC CHECKIDENT (Food, RESEED, 0); -- reset IDENTITY cho id chạy bắt đầu từ 1
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

DBCC CHECKIDENT (Bill, RESEED, 0); -- reset IDENTITY cho id chạy bắt đầu từ 1
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

DBCC CHECKIDENT (BillInfo, RESEED, 0); -- reset IDENTITY cho id chạy bắt đầu từ 1
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
GO]]]

DELETE TableFood

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

SELECT * FROM dbo.Bill

CREATE PROC USP_InsertBill
@idTable INT
AS 
BEGIN
	INSERT INTO Bill(
		DateCheckIn,
		DateCheckOut,
		idTable,
		status,
		discount
	) 
	VALUES(
		GETDATE(),
		NULL,
		@idTable,
		0,
		0
	)
END
GO

EXEC USP_InsertBill @idTable

CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT 
AS
BEGIN

	DECLARE @isExistBillInfo INT
	DECLARE @foodCount INT = 1

	SELECT @isExistBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b 
	WHERE idBill = @idBill AND idFood = @idFood

	IF (@isExistBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF (@newCount > 0)
			UPDATE dbo.BillInfo SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE 
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE 
	BEGIN
		INSERT dbo.BillInfo
				(idBill,
				idFood,
				count )
		VALUES (@idBill, 
				@idFood,
				@count
		)
	END
END
GO

SELECT MAX(id) FROM Bill


DELETE BillInfo
DELETE Bill

CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT

	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idTable INT

	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0

	UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable

END
GO
DROP TRIGGER UTG_UpdateBillInfo
CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT

	SELECT @idBill = id FROM Inserted
	
	DECLARE @idTable INT

	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count int = 0

	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0

	IF (@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.idTable = 1 


ALTER TABLE Bill
ADD discount INT

UPDATE dbo.Bill SET discount = 0
UPDATE dbo.Bill SET totalPrice = 0

SELECT discount FROM Bill


DROP PROC USP_SwitchTable
CREATE PROC USP_SwitchTable
@idTable1 INT, @idTable2 INT
AS BEGIN

	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT

	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0

	IF (@idFirstBill = NULL)
	BEGIN
		INSERT INTO dbo.Bill (
				DateCheckIn, 
				DateCheckOut,
				idTable,
				status)
		VALUES ( GETDATE(),
				NULL,
				@idTable1,
				0
				)
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	END

	IF (@idSecondBill = NULL)
	BEGIN
		INSERT INTO dbo.Bill (
				DateCheckIn, 
				DateCheckOut,
				idTable,
				status)
		VALUES ( GETDATE(),
				NULL,
				@idTable2,
				0
				)
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	END

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill

	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

	DROP TABLE IDBillInfoTable
END
GO

ALTER TABLE dbo.Bill ADD totalPrice FLOAT

GO

CREATE PROC USP_GetListBillByDate
@checkIn DATE, @checkOut DATE
AS
BEGIN
	SELECT t.name, DateCheckIn, DateCheckOut, discount, totalPrice
	FROM Bill AS b, TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1
	AND t.id = b.idTable
END
GO

DELETE Bill

SELECT * FROM Account WHERE userName = 'k9'