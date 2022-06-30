USE Northwind


--1)	Viết hàm kiểm tra mã khách hàng có tồn tại hay không, trả về giá trị True nếu có, ngược lại trả về False.
CREATE FUNCTION Cau1 (
	@MaKH nchar(5)
)
RETURNS CHAR(5)
AS
BEGIN
	DECLARE @KQ CHAR(5)

	IF EXISTS(SELECT CustomerID FROM Customers WHERE CustomerID = @MaKH)
		SET @KQ = 'TRUE'
	ELSE
		SET @KQ = 'FALSE'

	RETURN @KQ

END
GO

DECLARE @KQ CHAR(5)
SELECT @KQ = dbo.Cau1('ALFKI')
PRINT @KQ

SELECT dbo.Cau1('ALFKI')
SELECT dbo.Cau1('ABCDE')

DROP FUNCTION dbo.Cau1 

--2)	Viết hàm xuất danh sách 10 khách hàng mua nhiều sản phẩm nhất.
CREATE FUNCTION Cau2 ()
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

SELECT * FROM dbo.Cau2()

DROP FUNCTION dbo.Cau2

--3)	Viết hàm truyền vào mã khách hàng, xuất mã đơn hàng, tổng số mặt hàng đã đặt, tổng giá trị của từng phiếu đặt của khách hàng đó.
CREATE FUNCTION Cau3 (
	@MaKH NCHAR(5)
)
RETURNS TABLE
AS
	
	RETURN (
		SELECT Orders.OrderID, COUNT(ProductID) 'Tong mat hang', SUM((UnitPrice*Quantity)*(1-Discount)) 'Tong gia tri'
		FROM Orders, [Order Details] od
		WHERE CustomerID = @MaKH AND Orders.OrderID = od.OrderID
		GROUP BY Orders.OrderID
	)
GO

SELECT * FROM dbo.Cau3('ALFKI')
SELECT * FROM dbo.Cau3('VINET')

DROP FUNCTION dbo.Cau3

--4)	Viết thủ tục thêm đơn đặt hàng vào bảng đơn hàng, kiểm tra khách hàng trước khi thêm (sử dụng câu 1).
CREATE PROCEDURE Cau4
	@MaKH NCHAR(5)
AS
BEGIN
	DECLARE @KQ CHAR(5)
	SELECT @KQ = dbo.Cau1(@MaKH)
	
	IF @KQ = 'TRUE'
		BEGIN
			INSERT INTO Orders(CustomerID) VALUES(@MaKH)
			PRINT 'Them thanh cong don hang'
		END
	ELSE
		PRINT 'Sai ma khach hang'
END
GO

EXEC Cau4 'ALFKI'
EXEC Cau4 'ABC'

DELETE Orders WHERE OrderID = '11079'

--5)	Viết thủ tục tạo 1 đơn hàng mới với thông tin: mã khách hàng, và thông tin 1 sản phẩm của đơn hàng.
--	Cài đặt các ràng buộc sau:
--6)	Ngày đặt hàng không lớn hơn ngày hiện tại.
--7)	Một khách hàng không đặt quá 10 đơn hàng trong 1 ngày.
