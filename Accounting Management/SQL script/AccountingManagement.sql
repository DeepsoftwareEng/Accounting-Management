use AccountingManagement
drop database AccountingManagement
create table City(
	Id int primary key identity(1,1),
	TenThanhPho nvarchar(100)
)
create table State(
	Id int primary key identity(1,1),
	TenHuyen nvarchar(100)
)
create table Commune(
	Id int primary key identity(1,1),
	TenXa nvarchar(100)
)
create table Customer(
	MaKhachHang nvarchar(1000) primary key,
	TenKhachHang nvarchar(1000),
	DonVi nvarchar(1000),
	MaSoThue nvarchar(1000),
	DiaChiCuThe nvarchar(1000),
	SoDienThoai nvarchar(10),
	CreateDate datetime,
	IdThanhPho int,
	IdHuyen int,
	IdXa int,
	Foreign Key(IdThanhPho) references City(Id),
	Foreign Key(IdHuyen) references State(Id),
	Foreign Key(IdXa) references Commune(Id)
)
create table Bank(
	IdNganHang int primary key identity(1,1),
	TenNganHang nvarchar(100)
)
create table BankAccount(
	MaTaiKhoan varchar(1000) primary key,
	SoTaiKhoan varchar(1000),
	TienNo int,
	HienCo int,
	LoaiTaiKhoan varchar(1000),
	IdNganHang int,
	Foreign Key(IdNganHang) references Bank(IdNganHang)
)
create table Role(
	IdChucVu int primary key identity(1,1),
	TenChucVu nvarchar(1000)
)
create table Department(
	IdPhongBan int primary key identity(1,1),
	TenPhongBan nvarchar(1000)
)
create table Employee(
	MaNhanVien varchar(1000) primary key,
	TenNhanVien nvarchar(1000),
	NgaySinh datetime,
	IdChucVu int,
	IdPhongBan int,
	CreateDate datetime,
	Foreign Key(IdChucVu) references Role(IdChucVu),
	Foreign Key(IdPhongBan) references Department(IdPhongBan)
)
Create table BankLog(
	Id int primary key,
	NoiDung varchar(1000),
	SoTien real,
	MaTaiKHoan varchar(1000),
	Foreign Key(MaTaiKHoan) references BankAccount(MaTaiKHoan)
)
create table Account(
	IdAccount int primary key identity(1,1),
	Username varchar(1000),
	Password varchar(1000),
	AccountType varchar(100),
	Email varchar(1000)
)
create table Users(
	MaNguoiDung int primary key identity(1,1),
	TenNguoiDung varchar(1000),
	NgaySinh datetime,
	SoDienThoai varchar(10),
	DiaChi nvarchar(1000),
	IdAccount int,
	Foreign Key(IdAccount) references Account(IdAccount)
)
Create table Logs(
	Id int primary key identity(1,1),
	Action varchar(100),
	NgayThucHien datetime,
	NoiDung nvarchar(1000),
	IdAccount int,
	Foreign Key(IdAccount) references Account(IdAccount)
)
Create table Product(
	MaHangHoa varchar(100) primary key,
	TenHangHoa nvarchar(1000),
	DonViTinh nvarchar(1000),
	CreateDate datetime,
	SoLuong int,
	DonGia real
)
create table Invoice(
	MaHoaDon varchar(1000) primary key,
	NgayLap datetime,
	ThanhTien real,
	NoiDung nvarchar(1000),
	MaKhachHang nvarchar(1000),
	Foreign Key(MaKhachHang) references Customer(MaKhachHang)
)
create table Product_Invoice(
	Id int primary key identity(1,1),
	SoLuong int,
	MaHangHoa varchar(100),
	MaHoaDon varchar(1000),
	Foreign Key(MaHangHoa) references Product(MaHangHoa),
	Foreign Key(MaHoaDon) references Invoice(MaHoaDon)
)
Create table PhieuXuat(
	MaPhieu varchar(1000),
	DonVi varchar(1000),
	DiaChi varchar(1000),
	LiDo varchar(1000),
	NgayLap datetime,
	NguoiLap varchar(1000),
	ThuKho nvarchar(1000),
	NguoiNhan nvarchar(1000),
	GiamDoc nvarchar(1000),
	KeToanTruong nvarchar(1000),
	MaHoaDon varchar(1000),
	IdNhanVien varchar(1000),
	Foreign Key(IdNhanVien) references Employee(MaNhanVien),
	Foreign Key(MaHoaDon) references Invoice(MaHoaDon)
)
Create table PhieuNhap(
	MaPhieu varchar(1000),
	DonVi varchar(1000),
	DiaChi varchar(1000),
	NoiDung varchar(1000),
	NgayLap datetime,
	NguoiGiao nvarchar(1000),
	NguoiLap varchar(1000),
	ThuKho nvarchar(1000),
	NguoiNhan nvarchar(1000),
	GiamDoc nvarchar(1000),
	KeToanTruong nvarchar(1000),
	MaHoaDon varchar(1000),
	NoiNhap nvarchar(1000),
	Foreign Key(MaHoaDon) references Invoice(MaHoaDon)
)
create table Shop(
	MaSoThue varchar(1000) primary key,
	TenCongTy varchar(1000),
)
alter table Invoice
add MaKhachHang nvarchar(1000)
alter table Product
add CreateDate datetime
alter table Invoice
add CreateDate datetime
alter table Invoice 
add constraint fk_Invoice_Customer foreign key (MaKhachHang) references Customer(MaKhachHang)
alter table PhieuXuat
add IdNhanVien varchar(1000)
alter table PhieuXuat
add constraint fk_Employee_PhieuXuat foreign key (IdNhanVien) references Employee(MaNhanVien)
alter table Customer
add CreateDate datetime
alter table Invoice
drop column MaSoThueMua 
alter table PhieuNhap
add NguoiGiao nvarchar(1000)
alter table Department
drop column IdPhongBan
alter table Invoice
drop column MaSoThueBan
alter table Invoice
drop column DonViBan
drop table PhieuXuat
drop table Employee
drop table Department
drop table Role
insert into Account values ('admin', 'admin1', 'ADMIN', 'deepit2507@gmail.com'),
				           ('staff', 'staff', 'STAFF', 'deepit2507@gmail.com')
insert into Customer values ('CUST120524','NguyenPhucHung', 'DonViA', '123','SN21', '0823977189', 1, 1 ,1)
insert into Shop values ('ádfasdf', 'Công ty abc')
insert into City values (N'Hà N?i')
insert into State values (N'B?c T? Liêm')
insert into Commune values (N'Xuân Ph??ng')
select * from Customer
