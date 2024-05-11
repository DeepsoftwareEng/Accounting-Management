using System;
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

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhieuXuat> PhieuXuats { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductInvoice> ProductInvoices { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AccountingManagement;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__B7B00CE39D3EB3E5");

            entity.ToTable("Account");

            entity.Property(e => e.IdAccount).ValueGeneratedNever();
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
            entity.HasKey(e => e.IdNganHang).HasName("PK__Bank__043DBB0CF70D41B6");

            entity.ToTable("Bank");

            entity.Property(e => e.IdNganHang).ValueGeneratedNever();
            entity.Property(e => e.TenNganHang).HasMaxLength(100);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__BankAcco__AD7C6529D669760E");

            entity.ToTable("BankAccount");

            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.LoaiTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SoTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.IdNganHangNavigation).WithMany(p => p.BankAccounts)
                .HasForeignKey(d => d.IdNganHang)
                .HasConstraintName("FK__BankAccou__IdNga__4F7CD00D");
        });

        modelBuilder.Entity<BankLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankLog__3214EC07E4E85FC3");

            entity.ToTable("BankLog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MaTaiKhoan)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MaTaiKHoan");
            entity.Property(e => e.NoiDung)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.BankLogs)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK__BankLog__MaTaiKH__52593CB8");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC07F8D0F89C");

            entity.ToTable("City");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TenThanhPho).HasMaxLength(100);
        });

        modelBuilder.Entity<Commune>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Commune__3214EC075D8DB82F");

            entity.ToTable("Commune");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TenXa).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__Customer__88D2F0E574C49188");

            entity.ToTable("Customer");

            entity.Property(e => e.MaKhachHang).HasMaxLength(1000);
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

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__Invoice__835ED13B7BACED6D");

            entity.ToTable("Invoice");

            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DonViBan).HasMaxLength(1000);
            entity.Property(e => e.DonViMua).HasMaxLength(1000);
            entity.Property(e => e.MaKhachHang).HasMaxLength(1000);
            entity.Property(e => e.MaSoThueBan).HasMaxLength(1000);
            entity.Property(e => e.MaSoThueMua).HasMaxLength(1000);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NguoiMua).HasMaxLength(1000);
            entity.Property(e => e.NoiDung).HasMaxLength(1000);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("fk_Invoice_Customer");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07237EA8D5");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NgayThucHien).HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(1000);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Logs)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Logs__IdAccount__35BCFE0A");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PhieuNhap");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DonVi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.GiamDoc).HasMaxLength(1000);
            entity.Property(e => e.KeToanTruong).HasMaxLength(1000);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaPhieu)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NguoiLap)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NguoiNhan).HasMaxLength(1000);
            entity.Property(e => e.NoiDung)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NoiNhap).HasMaxLength(1000);
            entity.Property(e => e.ThuKho).HasMaxLength(1000);

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany()
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__PhieuNhap__MaHoa__412EB0B6");
        });

        modelBuilder.Entity<PhieuXuat>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PhieuXuat");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.DonVi)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.GiamDoc).HasMaxLength(1000);
            entity.Property(e => e.KeToanTruong).HasMaxLength(1000);
            entity.Property(e => e.LiDo)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MaPhieu)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NguoiLap)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.NguoiNhan).HasMaxLength(1000);
            entity.Property(e => e.ThuKho).HasMaxLength(1000);

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany()
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__PhieuXuat__MaHoa__3F466844");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.MaHangHoa).HasName("PK__Product__9737FBA85D54BC4E");

            entity.ToTable("Product");

            entity.Property(e => e.MaHangHoa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DonViTinh).HasMaxLength(1000);
            entity.Property(e => e.TenHangHoa).HasMaxLength(1000);
        });

        modelBuilder.Entity<ProductInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3214EC078C7675EE");

            entity.ToTable("Product_Invoice");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MaHangHoa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaHoaDon)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.MaHangHoaNavigation).WithMany(p => p.ProductInvoices)
                .HasForeignKey(d => d.MaHangHoa)
                .HasConstraintName("FK__Product_I__MaHan__3C69FB99");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ProductInvoices)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK__Product_I__MaHoa__3D5E1FD2");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__State__3214EC070D887242");

            entity.ToTable("State");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TenHuyen).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__Users__C539D7626B9F68A7");

            entity.Property(e => e.MaNguoiDung).ValueGeneratedNever();
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
                .HasConstraintName("FK__Users__IdAccount__32E0915F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
