using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuotationSystem2.DataAccessLayer.Models;

public partial class QuotationSystem2DBContext : DbContext
{
    public QuotationSystem2DBContext(DbContextOptions<QuotationSystem2DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<ComponentPrice> ComponentPrices { get; set; }

    public virtual DbSet<ComponentTranslation> ComponentTranslations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerTranslation> CustomerTranslations { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<PipeDiameter> PipeDiameters { get; set; }

    public virtual DbSet<PipePrice> PipePrices { get; set; }

    public virtual DbSet<PipeThickness> PipeThicknesses { get; set; }

    public virtual DbSet<PipeTop> PipeTops { get; set; }

    public virtual DbSet<PipeTopPrice> PipeTopPrices { get; set; }

    public virtual DbSet<PipeTopPricingMethod> PipeTopPricingMethods { get; set; }

    public virtual DbSet<PipeTopTranslation> PipeTopTranslations { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPipeTopMapping> ProductPipeTopMappings { get; set; }

    public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

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

        modelBuilder.Entity<ComponentPrice>(entity =>
        {
            entity.HasKey(e => new { e.DiameterID, e.ComponentID }).HasName("ComponentPrice_PK");

            entity.ToTable("ComponentPrice", "quote_adv");

            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Component).WithMany(p => p.ComponentPrices)
                .HasForeignKey(d => d.ComponentID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ComponentPrice_FK_ComponentID");

            entity.HasOne(d => d.Diameter).WithMany(p => p.ComponentPrices)
                .HasForeignKey(d => d.DiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ComponentPrice_FK_DiameterID");
        });

        modelBuilder.Entity<ComponentTranslation>(entity =>
        {
            entity.HasKey(e => new { e.ComponentID, e.LanguageCode }).HasName("ComponentTranslation_PK");

            entity.ToTable("ComponentTranslation", "quote_trans");

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Component).WithMany(p => p.ComponentTranslations)
                .HasForeignKey(d => d.ComponentID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ComponentTranslation_FK_ComponentID");
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

        modelBuilder.Entity<CustomerTranslation>(entity =>
        {
            entity.HasKey(e => new { e.CustomerID, e.LanguageCode }).HasName("CustomerTranslation_PK");

            entity.ToTable("CustomerTranslation", "quote_trans");

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerTranslations)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CustomerTranslation_FK_CustomerID");
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

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Employee_FK_RoleID");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageID).HasName("PK__Language__B938558B5F000F94");

            entity.ToTable("Language", "system");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(e => e.OperatorID).HasName("PK__Operator__7BB12F8E02128EBE");

            entity.ToTable("Operator", "quote");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OperatorName)
                .HasMaxLength(50)
                .IsUnicode(false);
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

        modelBuilder.Entity<PipePrice>(entity =>
        {
            entity.HasKey(e => new { e.DiameterID, e.ThicknessID }).HasName("PipePrice_PK");

            entity.ToTable("PipePrice", "quote_adv");

            entity.Property(e => e.PipeUnitPrice).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Diameter).WithMany(p => p.PipePrices)
                .HasForeignKey(d => d.DiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipePrice_FK_DiameterID");

            entity.HasOne(d => d.Thickness).WithMany(p => p.PipePrices)
                .HasForeignKey(d => d.ThicknessID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipePrice_FK_ThicknessID");
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

            entity.HasOne(d => d.PricingMethodNavigation).WithMany(p => p.PipeTops)
                .HasForeignKey(d => d.PricingMethod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTop_FK_PricingMethod");

            entity.HasOne(d => d.Translation).WithMany(p => p.PipeTops)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTop_FK_TranslationID");
        });

        modelBuilder.Entity<PipeTopPrice>(entity =>
        {
            entity.HasKey(e => new { e.DiameterID, e.PipeTopID }).HasName("PipeTopPrice_PK");

            entity.ToTable("PipeTopPrice", "quote_adv");

            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Diameter).WithMany(p => p.PipeTopPrices)
                .HasForeignKey(d => d.DiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTopPrice_FK_DiameterID");

            entity.HasOne(d => d.PipeTop).WithMany(p => p.PipeTopPrices)
                .HasForeignKey(d => d.PipeTopID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTopPrice_FK_PipeTopID");
        });

        modelBuilder.Entity<PipeTopPricingMethod>(entity =>
        {
            entity.HasKey(e => e.MethodID).HasName("PK__PipeTopP__FC681FB13DBBAC84");

            entity.ToTable("PipeTopPricingMethod", "quote");

            entity.Property(e => e.MethodName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Translation).WithMany(p => p.PipeTopPricingMethods)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTopPricingMethod_FK_TranslationID");
        });

        modelBuilder.Entity<PipeTopTranslation>(entity =>
        {
            entity.HasKey(e => new { e.PipeTopID, e.LanguageCode }).HasName("PipeTopTranslation_PK");

            entity.ToTable("PipeTopTranslation", "quote_trans");

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.PipeTop).WithMany(p => p.PipeTopTranslations)
                .HasForeignKey(d => d.PipeTopID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PipeTopTranslation_FK_ProductID");
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

        modelBuilder.Entity<ProductPipeTopMapping>(entity =>
        {
            entity.HasKey(e => new { e.ProductID, e.PipeTopID }).HasName("ProductPipeTop_PK");

            entity.ToTable("ProductPipeTopMapping", "quote_adv");

            entity.HasOne(d => d.PipeTop).WithMany(p => p.ProductPipeTopMappings)
                .HasForeignKey(d => d.PipeTopID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPipeTop_FK_PipeTopID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPipeTopMappings)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPipeTop_FK_ProductID");
        });

        modelBuilder.Entity<ProductTranslation>(entity =>
        {
            entity.HasKey(e => new { e.ProductID, e.LanguageCode }).HasName("ProductTranslation_PK");

            entity.ToTable("ProductTranslation", "quote_trans");

            entity.Property(e => e.LanguageCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductTranslations)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductTranslation_FK_ProductID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A9416A2B7");

            entity.ToTable("Role", "system");

            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Translation).WithMany(p => p.Roles)
                .HasForeignKey(d => d.TranslationID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Role_FK_TranslationID");
        });

        modelBuilder.Entity<SealingPlateRecord>(entity =>
        {
            entity.HasKey(e => e.SealingPlateID).HasName("PK__SealingP__3EE302654671109D");

            entity.ToTable("SealingPlateRecord", "record");

            entity.Property(e => e.CustomerDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopUnitPrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipeUnitPrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.SealingPlatePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SocketPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.SocketUnitPrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.TotalPriceBeforeDiscount).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.WeldingUnitPrice).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_CustomerID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.EmployeeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SealingPlateRecord_FK_EmployeeID");

            entity.HasOne(d => d.OperatorNavigation).WithMany(p => p.SealingPlateRecords)
                .HasForeignKey(d => d.Operator)
                .HasConstraintName("SealingPlateRecord_FK_Operator");

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

            entity.Property(e => e.CustomerDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeUnitPrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.StudPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.StudUnitPrice).HasColumnType("decimal(6, 2)");
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

            entity.HasOne(d => d.OperatorNavigation).WithMany(p => p.SheerStudRecords)
                .HasForeignKey(d => d.Operator)
                .HasConstraintName("SheerStudRecord_FK_Operator");

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

            entity.HasOne(d => d.Language).WithMany(p => p.Translations)
                .HasForeignKey(d => d.LanguageID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Translation_FK_LanguageID");

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

            entity.Property(e => e.CustomerDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EndPlatePrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Operand).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeLength).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipePrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopPrice).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PipeTopUnitPrice).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.PipeUnitPrice).HasColumnType("decimal(6, 2)");
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

            entity.HasOne(d => d.OperatorNavigation).WithMany(p => p.WaterMeterRecords)
                .HasForeignKey(d => d.Operator)
                .HasConstraintName("WaterMeterRecord_FK_Operator");

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
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.NippleDiameter).WithMany(p => p.WaterMeterRecordNipples)
                .HasForeignKey(d => d.NippleDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordNipple_FK_NippleDiameterID");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterRecordNipples)
                .HasForeignKey(d => d.WaterMeterID)
                .HasConstraintName("WaterMeterRecordNipple_FK_WaterMeterID");
        });

        modelBuilder.Entity<WaterMeterRecordSocket>(entity =>
        {
            entity.HasKey(e => e.SocketID).HasName("PK__WaterMet__A1A1723F613DC2EA");

            entity.ToTable("WaterMeterRecordSocket", "record");

            entity.HasIndex(e => e.SocketID, "WaterMeterRecordSocket_INX_SocketID");

            entity.Property(e => e.Price).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.SocketDiameter).WithMany(p => p.WaterMeterRecordSockets)
                .HasForeignKey(d => d.SocketDiameterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WaterMeterRecordSocket_FK_SocketDiameterID");

            entity.HasOne(d => d.WaterMeter).WithMany(p => p.WaterMeterRecordSockets)
                .HasForeignKey(d => d.WaterMeterID)
                .HasConstraintName("WaterMeterRecordSocket_FK_WaterMeterID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
