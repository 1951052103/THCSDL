USE Northwind


--1)	Viết hàm kiểm tra mã khách hàng có tồn tại hay không, trả về giá trị True nếu có, ngược lại trả về False.
CREATE FUNCTION fCau1 (
	@MaKH nchar(5)
)
RETURNS BIT
AS
BEGIN
	DECLARE @KQ BIT

	IF EXISTS(SELECT CustomerID FROM Customers WHERE CustomerID = @MaKH)
		SET @KQ = 1
	ELSE
		SET @KQ = 0

	RETURN @KQ

END
GO

SELECT dbo.fCau1('ALFKI')

DROP FUNCTION dbo.fCau1 

--2)	Viết hàm xuất danh sách 10 khách hàng mua nhiều sản phẩm nhất.
CREATE FUNCTION fCau2 ()
RETURNS TABLE
AS
	
	RETURN (
		SELECT * 
		FROM Customers
		WHERE CustomerID IN (
			SELECT TOP 5 CustomerID
			FROM Orders, [Order Details] od
			WHERE Orders.OrderID = od.OrderID
			GROUP BY CustomerID
			ORDER BY SUM(Quantity) DESC 
		)
	)
GO

SELECT * FROM dbo.fCau2()

DROP FUNCTION dbo.fCau2

--3)	Viết hàm truyền vào mã khách hàng, xuất mã đơn hàng, tổng số mặt hàng đã đặt, tổng giá trị của từng phiếu đặt của khách hàng đó.
CREATE FUNCTION fCau3 (
	@MaKH NCHAR(5)
)
RETURNS @KetQua TABLE(MaDonHang int, TSMH int, TongTien MONEY)
AS
	
	BEGIN
		INSERT INTO @KetQua
		SELECT Orders.OrderID, COUNT(ProductID), SUM(UnitPrice*Quantity*(1-Discount))
		FROM Orders, [Order Details] od
		WHERE CustomerID = @MaKH AND Orders.OrderID = od.OrderID
		GROUP BY Orders.OrderID
		RETURN
	END
GO

SELECT * FROM dbo.fCau3('ALFKI')
SELECT * FROM dbo.fCau3('SAVEA')

DROP FUNCTION dbo.fCau3

--4)	Viết thủ tục thêm đơn đặt hàng vào bảng đơn hàng, kiểm tra khách hàng trước khi thêm (sử dụng câu 1).
CREATE PROCEDURE fCau4
	@MaKH NCHAR(5)
AS
BEGIN
	
	
	IF ( dbo.fCau1(@MaKH) = 1 )
		BEGIN
			INSERT INTO Orders(CustomerID) VALUES(@MaKH)
			IF @@ROWCOUNT = 1
				PRINT N'Thêm thành công'
			ELSE
				PRINT 'Thêm thất bại'
		END
	ELSE
		PRINT N'KH không tồn tại'
END
GO

EXEC fCau4 'ALFKI'
EXEC fCau4 'ABC'

DELETE Orders WHERE OrderID = '11079'

--5)	Viết thủ tục tạo 1 đơn hàng mới với thông tin: mã khách hàng, và thông tin 1 sản phẩm của đơn hàng.
CREATE PROCEDURE fCau5
	@MaKH NCHAR(5),
	@MaSP INT,
	@SoLuong SMALLINT,
	@DonGia MONEY,
	@GiamGia SMALLINT
AS
BEGIN
	BEGIN TRY
		DECLARE @MaDH INT
		INSERT INTO Orders(CustomerID) VALUES(@MaKH)
		
		SAVE TRAN ThemCTDH

		SET @MaDH = (SELECT TOP 1 OrderId FROM Orders ORDER BY 1 DESC)

		INSERT INTO [Order Details]
		VALUES (@MaDH, @MaSP, @DonGia, @SoLuong, @GiamGia)
		
		PRINT N'Thêm thành công'
	END TRY
	BEGIN CATCH
		PRINT N'Thêm thất bại, DH chưa có SP'
		ROLLBACK TRAN ThemCTDH
	END CATCH
END
GO

--	Cài đặt các ràng buộc sau:
--6)	Ngày đặt hàng không lớn hơn ngày hiện tại.

CREATE TRIGGER trCau6 ON Orders
	FOR INSERT, UPDATE
AS 
	BEGIN
		SET DATEFORMAT dmy
		IF (UPDATE (OrderDate))
			IF (EXISTS(SELECT OrderID FROM Orders WHERE OrderDate > GETDATE()))
				BEGIN
					PRINT N'Ngày đặt hàng không lớn hơn ngày hiện tại' 
					ROLLBACK TRANSACTION
				END
	END
GO

INSERT INTO Orders (OrderDate) VALUES (GETDATE()+1)

--7)	Một khách hàng không đặt quá 10 đơn hàng trong 1 ngày.
ALTER TRIGGER trCau7 ON Orders
	FOR INSERT, UPDATE
AS 
	BEGIN
		SET DATEFORMAT dmy
		IF (UPDATE (OrderDate))
			IF (EXISTS(
				SELECT COUNT(OrderID) 
				FROM Orders 
				WHERE OrderDate < GETDATE() 
				GROUP BY OrderDate, CustomerID
				HAVING COUNT(OrderID) > 1)
			)
				BEGIN
					RAISEERROR N'Một khách hàng không đặt quá 10 đơn hàng trong 1 ngày' 
					ROLLBACK TRANSACTION
				END
	END
GO

INSERT INTO Orders (CustomerID, OrderDate) VALUES ('ALFKI', GETDATE())
INSERT INTO Orders (CustomerID, OrderDate) VALUES ('ALFKI', GETDATE()-10)