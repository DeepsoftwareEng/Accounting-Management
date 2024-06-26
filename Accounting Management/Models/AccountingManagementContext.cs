﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Accounting_Management.Models;

public partial class AccountingManagementContext : DbContext
{
    public AccountingManagementContext()
    {
    }

    public AccountingManagementContext(DbContextOptions<AccountingManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<BankAccount> BankAccounts { get; set; }

    public virtual DbSet<BankLog> BankLogs { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Commune> Communes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhieuXuat> PhieuXuats { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductInvoice> ProductInvoices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AccountingManagement;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__B7B00CE34347A77B");

            entity.ToTable("Account");

            entity.Property(e => e.AccountType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.IdNganHang).HasName("PK__Bank__043DBB0C1B95DD8C");

            entity.ToTable("Bank");

            entity.Property(e => e.TenNganHang).HasMaxLength(100);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__BankAcco__AD7C652950BF9E11");

            entity.ToTable("BankAccount");

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LoaiTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SoTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TenTaiKhoan).HasMaxLength(1000);

            entity.HasOne(d => d.IdNganHangNavigation).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.IdNganHang)
                .HasConstraintName("FK__BankAccou__IdNga__30F848ED");
        });

        modelBuilder.Entity<BankLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankLog__3214EC07AE7E20D7");

            entity.ToTable("BankLog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MaTaiKHoan");
            entity.Property(e => e.NoiDung)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.BankLogs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__BankLog__MaTaiKH__33D4B598");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC07C403C87A");

            entity.ToTable("City");

            entity.Property(e => e.TenThanhPho).HasMaxLength(100);
        });

        modelBuilder.Entity<Commune>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Commune__3214EC0788CAF4FA");

            entity.ToTable("Commune");

            entity.Property(e => e.TenXa).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__Customer__88D2F0E5ABC73A78");

            entity.ToTable("Customer");

            entity.Property(e => e.MaKhachHang).HasMaxLength(1000);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChiCuThe).HasMaxLength(1000);
            entity.Property(e => e.DonVi).HasMaxLength(1000);
            entity.Property(e => e.MaSoThue).HasMaxLength(1000);
            entity.Property(e => e.SoDienThoai).HasMaxLength(10);
            entity.Property(e => e.TenKhachHang).HasMaxLength(1000);

            entity.HasOne(d => d.IdHuyenNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdHuyen)
                .HasConstraintName("FK__Customer__IdHuye__2B3F6F97");

            entity.HasOne(d => d.IdThanhPhoNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdThanhPho)
                .HasConstraintName("FK__Customer__IdThan__2A4B4B5E");

            entity.HasOne(d => d.IdXaNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.IdXa)
                .HasConstraintName("FK__Customer__IdXa__2C3393D0");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.IdPhongBan).HasName("PK__Departme__CFFBAF4C3FBE5FB0");

            entity.ToTable("Department");

            entity.Property(e => e.TenPhongBan).HasMaxLength(1000);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__Employee__77B2CA4730A9007B");

            entity.ToTable("Employee");

            entity.Property(e => e.MaNhanVien)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DiaChiCuThe).HasMaxLength(1000);
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenNhanVien).HasMaxLength(1000);

            entity.HasOne(d => d.IdChucVuNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdChucVu)
                .HasConstraintName("FK__Employee__IdChuc__00200768");

            entity.HasOne(d => d.IdHuyenNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdHuyen)
                .HasConstraintName("fk_Employee_Huyen");

            entity.HasOne(d => d.IdPhongBanNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdPhongBan)
                .HasConstraintName("FK__Employee__IdPhon__01142BA1");

            entity.HasOne(d => d.IdThanhPhoNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdThanhPho)
                .HasConstraintName("fk_Employee_ThanhPho");

            entity.HasOne(d => d.IdXaNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdXa)
                .HasConstraintName("fk_Employee_Xa");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__Invoice__835ED13BD865A0A4");

            entity.ToTable("Invoice");

            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaKhachHang).HasMaxLength(1000);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(1000);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__Invoice__MaKhach__403A8C7D");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07EA18B40A");

            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NgayThucHien).HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(1000);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Logs__IdAccount__3B75D760");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__PhieuNha__2660BFE0EA9043A9");

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.MaPhieu)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DonVi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.GiamDoc)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.IsThanhToan).HasColumnName("isThanhToan");
            entity.Property(e => e.KeToanTruong)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NguoiGiao)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NguoiLap)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NguoiNhan).HasMaxLength(1000);
            entity.Property(e => e.NoiDung)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NoiNhap).HasMaxLength(1000);
            entity.Property(e => e.ThuKho).HasMaxLength(1000);

            entity.HasOne(d => d.GiamDocNavigation).WithMany(p => p.PhieuNhapGiamDocNavigations)
                .HasForeignKey(d => d.GiamDoc)
                .HasConstraintName("FK__PhieuNhap__GiamD__4E53A1AA");

            entity.HasOne(d => d.KeToanTruongNavigation).WithMany(p => p.PhieuNhapKeToanTruongNavigations)
                .HasForeignKey(d => d.KeToanTruong)
                .HasConstraintName("FK__PhieuNhap__KeToa__4F47C5E3");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__PhieuNhap__MaHoa__503BEA1C");

            entity.HasOne(d => d.NguoiLapNavigation).WithMany(p => p.PhieuNhapNguoiLapNavigations)
                .HasForeignKey(d => d.NguoiLap)
                .HasConstraintName("FK__PhieuNhap__Nguoi__4D5F7D71");
        });

        modelBuilder.Entity<PhieuXuat>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__PhieuXua__2660BFE06E3AECC4");

            entity.ToTable("PhieuXuat");

            entity.Property(e => e.MaPhieu)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DiaChi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DonVi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.GiamDoc)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.IsThanhToan).HasColumnName("isThanhToan");
            entity.Property(e => e.KeToanTruong)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LiDo)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NguoiLap)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NguoiNhan).HasMaxLength(1000);
            entity.Property(e => e.NhanVienGiao)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ThuKho).HasMaxLength(1000);

            entity.HasOne(d => d.GiamDocNavigation).WithMany(p => p.PhieuXuatGiamDocNavigations)
                .HasForeignKey(d => d.GiamDoc)
                .HasConstraintName("FK__PhieuXuat__GiamD__489AC854");

            entity.HasOne(d => d.KeToanTruongNavigation).WithMany(p => p.PhieuXuatKeToanTruongNavigations)
                .HasForeignKey(d => d.KeToanTruong)
                .HasConstraintName("FK__PhieuXuat__KeToa__498EEC8D");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__PhieuXuat__MaHoa__4A8310C6");

            entity.HasOne(d => d.NguoiLapNavigation).WithMany(p => p.PhieuXuatNguoiLapNavigations)
                .HasForeignKey(d => d.NguoiLap)
                .HasConstraintName("FK__PhieuXuat__Nguoi__47A6A41B");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.MaHangHoa).HasName("PK__Product__9737FBA8129F49F6");

            entity.ToTable("Product");

            entity.Property(e => e.MaHangHoa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DonViTinh).HasMaxLength(1000);
            entity.Property(e => e.TenHangHoa).HasMaxLength(1000);
        });

        modelBuilder.Entity<ProductInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3214EC07FB73E497");

            entity.ToTable("Product_Invoice");

            entity.Property(e => e.MaHangHoa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.MaHangHoaNavigation).WithMany(p => p.ProductInvoices)
                .HasForeignKey(d => d.MaHangHoa)
                .HasConstraintName("FK__Product_I__MaHan__4316F928");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ProductInvoices)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__Product_I__MaHoa__440B1D61");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdChucVu).HasName("PK__Role__81D916DF2E898CD9");

            entity.ToTable("Role");

            entity.Property(e => e.TenChucVu).HasMaxLength(1000);
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.MaSoThue).HasName("PK__Shop__1E811CB0B3223873");

            entity.ToTable("Shop");

            entity.Property(e => e.MaSoThue)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TenCongTy)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__State__3214EC073AB9B868");

            entity.ToTable("State");

            entity.Property(e => e.TenHuyen).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__Users__C539D76259897E4B");

            entity.Property(e => e.DiaChi).HasMaxLength(1000);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Users__IdAccount__38996AB5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
