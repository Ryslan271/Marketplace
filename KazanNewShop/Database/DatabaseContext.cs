using System;
using System.Collections.Generic;
using KazanNewShop.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace KazanNewShop.Database;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PhotoProduct> PhotoProducts { get; set; }

    public virtual DbSet<PointOfIssue> PointOfIssues { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductList> ProductLists { get; set; }

    public virtual DbSet<ProductListOrder> ProductListOrders { get; set; }

    public virtual DbSet<Salesman> Salesmen { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=MarketShop;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Basket_1");

            entity.ToTable("Basket");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");

            entity.HasOne(d => d.Client).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Basket_Client");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.NumberOfCreditCard).HasMaxLength(16);
            entity.Property(e => e.Patronymic).HasMaxLength(150);
            entity.Property(e => e.Surname).HasMaxLength(150);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_User1");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Patronymic).HasMaxLength(150);
            entity.Property(e => e.Surname).HasMaxLength(150);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_User1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdClient).HasColumnName("ID_Client");
            entity.Property(e => e.IdPointOfIssue).HasColumnName("ID_PointOfIssue");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Client");

            entity.HasOne(d => d.PointOfIssue).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdPointOfIssue)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Address");
        });

        modelBuilder.Entity<PhotoProduct>(entity =>
        {
            entity.ToTable("PhotoProduct");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.Photo).HasColumnType("image");

            entity.HasOne(d => d.Product).WithMany(p => p.PhotoProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoProduct_Product1");
        });

        modelBuilder.Entity<PointOfIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Address");

            entity.ToTable("PointOfIssue");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Lat)
                .HasColumnType("decimal(8, 6)")
                .HasColumnName("lat");
            entity.Property(e => e.Lot)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("lot");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.IdCategory).HasColumnName("ID_Category");
            entity.Property(e => e.IdSalesman).HasColumnName("ID_Salesman");
            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category1");

            entity.HasOne(d => d.Salesman).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSalesman)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Salesman1");

            entity.HasOne(d => d.Status).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK_Product_Status1");
        });

        modelBuilder.Entity<ProductList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ProductList_1");

            entity.ToTable("ProductList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdBasket).HasColumnName("ID_Basket");
            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

            entity.HasOne(d => d.Basket).WithMany(p => p.ProductLists)
                .HasForeignKey(d => d.IdBasket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductList_Basket");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductLists)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductList_Product");
        });

        modelBuilder.Entity<ProductListOrder>(entity =>
        {
            entity.HasKey(e => new { e.IdProduct, e.IdOrder });

            entity.ToTable("ProductListOrder");

            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductListOrders)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductListOrder_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductListOrders)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductListOrder_Product");
        });

        modelBuilder.Entity<Salesman>(entity =>
        {
            entity.ToTable("Salesman");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DateOnMarketplace).HasColumnType("date");
            entity.Property(e => e.NameCompany).HasMaxLength(150);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Salesman)
                .HasForeignKey<Salesman>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salesman_User1");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
