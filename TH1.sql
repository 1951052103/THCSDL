USE Northwind

--1.	Cho biến @NgaySinh kiểu datetime lưu thông tin ngày sinh. Viết script xuất ra màn hình bằng lệnh PRINT thông báo theo dạng “Tuổi là [@tuổi]”.

DECLARE @NgaySinh datetime
SET DATEFORMAT dmy
SET @NgaySinh = '9/9/2009'
PRINT N'Tuổi là ' + CONVERT(nvarchar, @NgaySinh,  106)

--2.	Viết khối lệnh hiển thị số đơn đặt hàng mỗi nhân viên đã đặt trong năm 1997, 
--nếu số đơn nhiều ít hơn 3 thì hiện thông báo "số lượng đơn hàng quá ít”. 
--Ngược lại hiển thị số lượng đơn hàng đã lập.

DECLARE @KetQua TABLE(EmployeeID INT , ThongBao NVARCHAR(50))
INSERT INTO @KetQua
SELECT EmployeeID,
CASE
    WHEN COUNT(OrderID) > 30 THEN N'số lượng:' + CAST(COUNT(OrderID) AS nvarchar)
    ELSE N'số lượng đơn hàng quá ít'
END AS ThongBao
FROM Orders
WHERE YEAR(OrderDate)=1997
GROUP BY EmployeeID

SELECT *
FROM @KetQua

--1)	Viết stored- procedure xuất danh sách các danh mục chưa có sản phẩm nào.

CREATE PROCEDURE Cau1
AS
BEGIN
	SELECT *
	FROM Categories
	WHERE CategoryID NOT IN (SELECT DISTINCT CategoryID
								FROM Categories
								)
END
GO

EXEC Cau1

--2)	Viết stored- procedure xuất danh sách khách hàng có đơn đặt hàng chưa giao với số lượng sản phẩm mua > 1.
--o	Thủ tục có tham số vào
--3)	Viết stored-procedure truyền vào mã sản phẩm, xuất ra thông tin sản phẩm.
--4)	Viết stored-procedure truyền vào ngày bắt đầu, ngày kết thúc, 
--xuất danh sách sản phẩm được đặt hàng trong khoảng thời gian trên. 
--(Nếu không nhập ngày bắt đầu thì lấy ngày đầu tiên của tháng hiện hành, 
--nếu ngày kết thúc không nhập thì lấy ngày hiện hành).
--o	Thủ tục có tham số vào và ra


CREATE PROC Cau4 
(
	@NgayBD datetime,
	@NgayKT datetime
)
AS
BEGIN
	IF @NgayBD IS NULL
		SET @NgayBD = (SELECT CONVERT(NVARCHAR(20), DATEADD(DAY, -(DAY(GETDATE() - 1)), GETDATE() ), 103))
	if @NgayKT IS NULL
		SET @NgayKT = GETDATE() 

	SELECT *
	FROM Products
	WHERE ProductID IN (SELECT DISTINCT ProductID
								FROM [Order Details] od, [Orders]
								WHERE Orders.OrderID = od.OrderID AND OrderDate BETWEEN @NgayBD AND @NgayKT)
END
GO

EXEC Cau4 '1/1/1996', '1/1/1997'

--6)	Viết stored-procedure thêm một sản phẩm mới
--Input: 	thông tin sản phẩm
--Output: 	1: Thêm sản phẩm thành công 
--2: Mã sản phẩm đã tồn tại 
---1: Lỗi hệ thống 

CREATE PROC Cau6 
(
	@ProductName nvarchar(40),
	@CategoryId int,
	@KQ int output
)
AS
BEGIN
	BEGIN TRY
	INSERT INTO Products(ProductName, CategoryID, Discontinued)
	VALUES (@ProductName, @CategoryId, 0)
	IF @@ROWCOUNT > 0
		SET @KQ = 1
	END TRY
	BEGIN CATCH
		SET @KQ = -1;
		THROW 50001, 'Loi he thong', 1
	END CATCH
END
GO

DECLARE @KQ1 int
EXEC CAU6 'Gao', '1', @KQ1 output
EXEC CAU6 '1', '20', @KQ1 output