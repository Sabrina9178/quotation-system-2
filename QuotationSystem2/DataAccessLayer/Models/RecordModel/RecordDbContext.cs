using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuotationSystem2.DataAccessLayer.Models.RecordModel;

public partial class RecordDbContext : DbContext
{
    public RecordDbContext()
    {
    }

    public RecordDbContext(DbContextOptions<RecordDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SealingPlateRecord> SealingPlateRecords { get; set; }

    public virtual DbSet<SheerStudRecord> SheerStudRecords { get; set; }

    public virtual DbSet<WaterMeterRecord> WaterMeterRecords { get; set; }

    public virtual DbSet<WaterMeterRecordNipple> WaterMeterRecordNipples { get; set; }

    public virtual DbSet<WaterMeterRecordSocket> WaterMeterRecordSockets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkID=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-MOSMRPO;Database=QuotationSystem2DB;User ID=ssss9178;Password=00000000;TrustServerCertificate=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SealingPlateRecord>(entity =>
        {
            entity.HasKey(e => e.SealingPlateID).HasName("PK__SealingP__3EE302654671109D");

            entity.ToTable("SealingPlateRecord", "record");

            entity.Property(e => e.SealingPlateID).HasColumnName("SealingPlateID");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeDiameterID).HasColumnName("PipeDiameterID");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeThicknessID).HasColumnName("PipeThicknessID");
            entity.Property(e => e.PipeTopID).HasColumnName("PipeTopID");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.SealingPlatePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SocketDiameterID).HasColumnName("SocketDiameterID");
            entity.Property(e => e.SocketPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");
        });

        modelBuilder.Entity<SheerStudRecord>(entity =>
        {
            entity.HasKey(e => e.SheerStudID).HasName("PK__SheerStu__0E916AEF9AF3250B");

            entity.ToTable("SheerStudRecord", "record");

            entity.Property(e => e.SheerStudID).HasColumnName("SheerStudID");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeDiameterID).HasColumnName("PipeDiameterID");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.StudPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.StudSpecID).HasColumnName("StudSpecID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");
        });

        modelBuilder.Entity<WaterMeterRecord>(entity =>
        {
            entity.HasKey(e => e.WaterMeterID).HasName("PK__WaterMet__28A465A96DBAF7E1");

            entity.ToTable("WaterMeterRecord", "record");

            entity.Property(e => e.WaterMeterID).HasColumnName("WaterMeterID");
            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
            entity.Property(e => e.EndPlatePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Operator)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.PipeDiameterID).HasColumnName("PipeDiameterID");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeThicknessID).HasColumnName("PipeThicknessID");
            entity.Property(e => e.PipeTopID).HasColumnName("PipeTopID");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");
        });

        modelBuilder.Entity<WaterMeterRecordNipple>(entity =>
        {
            entity.HasKey(e => e.NippleID).HasName("PK__WaterMet__82AEC05ADEA74533");

            entity.ToTable("WaterMeterRecordNipple", "record");

            entity.HasIndex(e => e.NippleID, "WaterMeterRecordNipple_INX_NippleID");

            entity.Property(e => e.NippleID).HasColumnName("NippleID");
            entity.Property(e => e.NippleDiameterID).HasColumnName("NippleDiameterID");
            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.WaterMeterID).HasColumnName("WaterMeterID");

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

            entity.Property(e => e.SocketID).HasColumnName("SocketID");
            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SocketDiameterID).HasColumnName("SocketDiameterID");
            entity.Property(e => e.WaterMeterID).HasColumnName("WaterMeterID");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterRecordSockets)
                .HasForeignKey(d => d.WaterMeterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordSocket_FK_WaterMeterID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
