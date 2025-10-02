using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuotationSystem2.DataAccessLayer.Models;

namespace QuotationSystem2.DataAccessLayer.Models.Contexts;

public partial class RecordDbContext : DbContext
{
    public RecordDbContext(DbContextOptions<RecordDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<PipeDiameter> PipeDiameters { get; set; }

    public virtual DbSet<PipeThickness> PipeThicknesses { get; set; }

    public virtual DbSet<PipeTop> PipeTops { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SealingPlateRecord> SealingPlateRecords { get; set; }

    public virtual DbSet<SheerStudRecord> SheerStudRecords { get; set; }

    public virtual DbSet<Stud> Studs { get; set; }

    public virtual DbSet<Translation> Translations { get; set; }

    public virtual DbSet<TranslationGroup> TranslationGroups { get; set; }

    public virtual DbSet<WaterMeterRecord> WaterMeterRecords { get; set; }

    public virtual DbSet<WaterMeterRecordNipple> WaterMeterRecordNipples { get; set; }

    public virtual DbSet<WaterMeterRecordSocket> WaterMeterRecordSockets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ComponentID).HasName("PK__Componen__D79CF02EFC5EB175");

            entity.ToTable("Component", "quote");

            entity.Property(e => e.ComponentName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Translation).WithMany(p => p.Components)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Component_FK_TranslationID");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID).HasName("PK__Customer__A4AE64B8EE2C377E");

            entity.ToTable("Customer", "quote");

            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Translation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Customer_FK_TranslationID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID).HasName("PK__Employee__7AD04FF158B283F4");

            entity.ToTable("Employee", "system");

            entity.HasIndex(e => e.Account, "UQ_Employees_Account").IsUnique();

            entity.Property(e => e.Account)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("00000000");
        });

        modelBuilder.Entity<PipeDiameter>(entity =>
        {
            entity.HasKey(e => e.DiameterID).HasName("PK__PipeDiam__4B38DA1F4EC8397F");

            entity.ToTable("PipeDiameter", "quote");

            entity.Property(e => e.Diameter).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PipeThickness>(entity =>
        {
            entity.HasKey(e => e.ThicknessID).HasName("PK__PipeThic__4B497D9AC52439FF");

            entity.ToTable("PipeThickness", "quote");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PipeTop>(entity =>
        {
            entity.HasKey(e => e.PipeTopID).HasName("PK__PipeTop__7FD028EE5081DDB2");

            entity.ToTable("PipeTop", "quote");

            entity.Property(e => e.PipeTopName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Translation).WithMany(p => p.PipeTops)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTop_FK_TranslationID");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK__Product__B40CC6ED0106566E");

            entity.ToTable("Product", "quote");

            entity.Property(e => e.PipeDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Translation).WithMany(p => p.Products)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_FK_TranslationID");
        });

        modelBuilder.Entity<SealingPlateRecord>(entity =>
        {
            entity.HasKey(e => e.SealingPlateID).HasName("PK__SealingP__3EE302654671109D");

            entity.ToTable("SealingPlateRecord", "record");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SealingPlatePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SocketPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_CustomerID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_EmployeeID");

            entity.HasOne(d => d.PipeDiameter).WithMany(p => p.SealingPlateRecordPipeDiameters)
                .HasForeignKey(d => d.PipeDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_PipeDiameterID");

            entity.HasOne(d => d.PipeThickness).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.PipeThicknessID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_PipeThicknessID");

            entity.HasOne(d => d.PipeTop).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.PipeTopID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_PipeTopID");

            entity.HasOne(d => d.Product).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_ProductID");

            entity.HasOne(d => d.SocketDiameter).WithMany(p => p.SealingPlateRecordSocketDiameters)
                .HasForeignKey(d => d.SocketDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_SocketDiameterID");
        });

        modelBuilder.Entity<SheerStudRecord>(entity =>
        {
            entity.HasKey(e => e.SheerStudID).HasName("PK__SheerStu__0E916AEF9AF3250B");

            entity.ToTable("SheerStudRecord", "record");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.StudPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SheerStudRecord_FK_CustomerID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SheerStudRecord_FK_EmployeeID");

            entity.HasOne(d => d.PipeDiameter).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.PipeDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SheerStudRecord_FK_PipeDiameterID");

            entity.HasOne(d => d.Product).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SheerStudRecord_FK_ProductID");

            entity.HasOne(d => d.StudSpec).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.StudSpecID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SheerStudRecord_FK_StudSpecID");
        });

        modelBuilder.Entity<Stud>(entity =>
        {
            entity.HasKey(e => e.StudID).HasName("PK__StudPric__883D519B2A2F9CC1");

            entity.ToTable("Stud", "quote");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");
        });

        modelBuilder.Entity<Translation>(entity =>
        {
            entity.HasKey(e => new { e.TranslationID, e.LanguageID }).HasName("Translation_PK");

            entity.ToTable("Translation", "quote_trans");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.TranslationNavigation).WithMany(p => p.Translations)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Translation_FK_TranslationID");
        });

        modelBuilder.Entity<TranslationGroup>(entity =>
        {
            entity.HasKey(e => e.TranslationID).HasName("PK__Translat__663DA0AC37EFDC47");

            entity.ToTable("TranslationGroup", "quote_trans");

            entity.Property(e => e.Dummy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WaterMeterRecord>(entity =>
        {
            entity.HasKey(e => e.WaterMeterID).HasName("PK__WaterMet__28A465A96DBAF7E1");

            entity.ToTable("WaterMeterRecord", "record");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EndPlatePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecord_FK_CustomerID");

            entity.HasOne(d => d.Employee).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecord_FK_EmployeeID");

            entity.HasOne(d => d.PipeDiameter).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.PipeDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecord_FK_PipeDiameterID");

            entity.HasOne(d => d.PipeThickness).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.PipeThicknessID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecord_FK_PipeThicknessID");

            entity.HasOne(d => d.PipeTop).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.PipeTopID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecord_FK_PipeTopID");
        });

        modelBuilder.Entity<WaterMeterRecordNipple>(entity =>
        {
            entity.HasKey(e => e.NippleID).HasName("PK__WaterMet__82AEC05ADEA74533");

            entity.ToTable("WaterMeterRecordNipple", "record");

            entity.HasIndex(e => e.NippleID, "WaterMeterRecordNipple_INX_NippleID");

            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.NippleDiameter).WithMany(p => p.WaterMeterRecordNipples)
                .HasForeignKey(d => d.NippleDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordNipple_FK_NippleDiameterID");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterRecordNipples)
                .HasForeignKey(d => d.WaterMeterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordNipple_FK_WaterMeterID");
        });

        modelBuilder.Entity<WaterMeterRecordSocket>(entity =>
        {
            entity.HasKey(e => e.SocketID).HasName("PK__WaterMet__A1A1723F613DC2EA");

            entity.ToTable("WaterMeterRecordSocket", "record");

            entity.HasIndex(e => e.SocketID, "WaterMeterRecordSocket_INX_SocketID");

            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");

            entity.HasOne(d => d.SocketDiameter).WithMany(p => p.WaterMeterRecordSockets)
                .HasForeignKey(d => d.SocketDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordSocket_FK_SocketDiameterID");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterRecordSockets)
                .HasForeignKey(d => d.WaterMeterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordSocket_FK_WaterMeterID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
