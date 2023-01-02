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

            entity.ToTable("TbAccount");

            entity.Property(e => e.AccountId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasComment("管理帳號");
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_tbOrder");

            entity.ToTable("TbOrder");

            entity.Property(e => e.OrderId).HasComment("訂單ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Userid)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("用戶ID");
        });

        modelBuilder.Entity<TbOrderProduction>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductionId }).HasName("PK_tbOrder_Production");

            entity.ToTable("TbOrder_Production");

            entity.Property(e => e.OrderId).HasComment("訂單ID");
            entity.Property(e => e.ProductionId).HasComment("產品ID");
        });

        modelBuilder.Entity<TbPoductionImage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TbPoduction_images");

            entity.Property(e => e.ImageId).HasComment("圖片ID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasComment("圖片位置");
            entity.Property(e => e.Orderby).HasComment("排序");
        });

        modelBuilder.Entity<TbProduction>(entity =>
        {
            entity.HasKey(e => e.ProductionId).HasName("PK_tbProduction");

            entity.ToTable("TbProduction");

            entity.Property(e => e.ProductionId).HasComment("產品ID");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasComment("商品描述");
            entity.Property(e => e.ImageId).HasComment("圖片ID");
            entity.Property(e => e.IsMarket).HasComment("是否上架");
            entity.Property(e => e.IsSold).HasComment("是否售出");
            entity.Property(e => e.Orderby).HasComment("排序");
            entity.Property(e => e.Price)
                .HasComment("價格")
                .HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasComment("標題");
            entity.Property(e => e.UpdateTime)
                .HasComment("更新時間")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TbShoppingcart>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TbShoppingcart");

            entity.Property(e => e.ProductionId).HasComment("產品ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasComment("用戶名");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_t_customer");

            entity.ToTable("TbUser");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
