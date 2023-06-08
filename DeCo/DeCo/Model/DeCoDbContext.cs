using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DeCo.Model;

public partial class DeCoDbContext : DbContext
{
    public DeCoDbContext()
    {
    }

    public DeCoDbContext(DbContextOptions<DeCoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-H1F4LGR;Database=DeCoDB;Trusted_Connection=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BillDeta__3214EC27E0FF65D9");

            entity.ToTable("BillDetail");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BillDate).HasColumnType("datetime");
            entity.Property(e => e.CusId).HasColumnName("CusID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.TotalMoney).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Cus).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.CusId)
                .HasConstraintName("FK__BillDetai__CusID__2E1BDC42");

            entity.HasOne(d => d.Employee).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__BillDetai__Emplo__2D27B809");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC27D8E84974");

            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MemberDate).HasColumnType("datetime");
            entity.Property(e => e.Spending).HasColumnType("decimal(19, 4)");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC27CE5CD163");

            entity.ToTable("Employee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasMaxLength(10);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC27DE387287");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(19, 4)");

            entity.HasOne(d => d.Bill).WithMany(p => p.Orders)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK__Orders__BillID__30F848ED");

            entity.HasOne(d => d.Product).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Orders__ProductI__31EC6D26");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC27147F5D23");

            entity.ToTable("Product");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ImportPrice).HasColumnType("decimal(19, 4)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(19, 4)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27FD740B9B");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Users__EmployeeI__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
