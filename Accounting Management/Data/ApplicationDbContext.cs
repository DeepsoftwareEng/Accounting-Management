using Accounting_Management.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<State> Huyens { get; set; }
        public DbSet<Customer> KhachHangs { get; set; }
        public DbSet<Invoice> HoaDons { get; set; }
        public DbSet<Log> NhatKyHoatDong { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<City> ThanhPhos { get; set; }
        public DbSet<Commune> Xas { get; set; }
        public DbSet<PhieuXuat> PhieuXuats { get; set; }
        public DbSet<Product> HangHoas { get; set; }
        public DbSet<User> NguoiDung { get; set; }
        public DbSet<Product_Invoice> Product_Invoice { get; set; }
    }
}
