using Microsoft.EntityFrameworkCore;
using QuanLySinhVienORM.Models;

namespace QuanLySinhVienORM.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhViens { get; set; }
    }
}