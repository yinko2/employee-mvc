using EmployeeMVC.Models;
using EmployeeMVC.Settings;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMVC.Data
{
    public partial class ApiDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IActionContextAccessor _actionContextAccessor;
        public ApiDbContext(DbContextOptions<ApiDbContext> options, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor, IConfiguration configuration)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            _configuration = configuration;
            var mysqlDbSettings = _configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();
            _connectionString = mysqlDbSettings.ConnectionString;
        }

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Holiday> Holidays { get; set; } = null!;
        public virtual DbSet<Leave> Leaves { get; set; } = null!;
        public virtual DbSet<Eventlog> Eventlogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse("8.0.27-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("tbl_employee");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<Eventlog>(entity =>
            {
                entity.ToTable("tbl_eventlog");

                entity.HasIndex(e => e.UserId, "user_id");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ErrorMessage).HasColumnType("text");

                entity.Property(e => e.FormName).HasMaxLength(100);

                entity.Property(e => e.LogDateTime).HasColumnType("datetime");

                entity.Property(e => e.LogMessage).HasColumnType("text");

                entity.Property(e => e.LogType).HasComment("Info = 1, Error = 2,Warning = 3, Insert = 4,Update = 5, Delete = 6");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Holiday>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.ToTable("tbl_holiday");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.ToTable("tbl_leave");

                entity.HasIndex(e => e.EmployeeId, "EmployeeID");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Reason).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
