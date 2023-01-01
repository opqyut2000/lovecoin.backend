using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DbModels;

namespace WebApplication1.Context;

public partial class LovecoinContext : DbContext
{
    public LovecoinContext()
    {
    }

    public LovecoinContext(DbContextOptions<LovecoinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbOrder> TbOrders { get; set; }

    public virtual DbSet<TbOrderProduction> TbOrderProductions { get; set; }

    public virtual DbSet<TbPoductionImage> TbPoductionImages { get; set; }

    public virtual DbSet<TbProduction> TbProductions { get; set; }

    public virtual DbSet<TbShoppingcart> TbShoppingcarts { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-UCHB2DM8\\SQLEXPRESS;Initial Catalog=lovecoin;User ID=sa;Password=z}^!iZhWu[M5;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK_agent_1");

            entity.ToTable("tbAccount");

            entity.Property(e => e.AccountId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("tbOrder");

            entity.Property(e => e.OrderId).HasColumnName("Order_Id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Userid)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbOrderProduction>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductionId });

            entity.ToTable("tbOrder_Production");

            entity.Property(e => e.OrderId).HasColumnName("Order_Id");
            entity.Property(e => e.ProductionId).HasColumnName("Production_Id");
        });

        modelBuilder.Entity<TbPoductionImage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbPoduction_images");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageUrl).HasMaxLength(100);
        });

        modelBuilder.Entity<TbProduction>(entity =>
        {
            entity.HasKey(e => e.ProductionId);

            entity.ToTable("tbProduction");

            entity.Property(e => e.ProductionId).HasColumnName("Production_Id");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbShoppingcart>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbShoppingcart");

            entity.Property(e => e.ProductionId).HasColumnName("Production_Id");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_t_customer");

            entity.ToTable("tbUser");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
