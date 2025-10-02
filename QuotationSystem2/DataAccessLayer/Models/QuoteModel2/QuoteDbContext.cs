//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace QuotationSystem2.DataAccessLayer.Models.QuoteModel2;

//public partial class QuoteDbContext : DbContext
//{
//    public QuoteDbContext()
//    {
//    }

//    public QuoteDbContext(DbContextOptions<QuoteDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Component> Components { get; set; }

//    public virtual DbSet<ComponentPrice> ComponentPrices { get; set; }

//    public virtual DbSet<Customer> Customers { get; set; }

//    public virtual DbSet<Language> Languages { get; set; }

//    public virtual DbSet<PipeDiameter> PipeDiameters { get; set; }

//    public virtual DbSet<PipePrice> PipePrices { get; set; }

//    public virtual DbSet<PipeThickness> PipeThicknesses { get; set; }

//    public virtual DbSet<PipeTop> PipeTops { get; set; }

//    public virtual DbSet<PipeTopPrice> PipeTopPrices { get; set; }

//    public virtual DbSet<PipeTopPricingMethod> PipeTopPricingMethods { get; set; }

//    public virtual DbSet<Product> Products { get; set; }

//    public virtual DbSet<ProductPipeTopMapping> ProductPipeTopMappings { get; set; }

//    public virtual DbSet<Stud> Studs { get; set; }

//    public virtual DbSet<Translation> Translations { get; set; }

//    public virtual DbSet<TranslationGroup> TranslationGroups { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkID=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-MOSMRPO;Database=QuotationSystem2DB;User ID=ssss9178;Password=00000000;TrustServerCertificate=True;Trusted_Connection=True");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Component>(entity =>
//        {
//            entity.HasKey(e => e.ComponentID).HasName("PK__Componen__D79CF02EFC5EB175");

//            entity.ToTable("Component", "quote");

//            entity.Property(e => e.ComponentID).HasColumnName("ComponentID");
//            entity.Property(e => e.ComponentName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.Translation).WithMany(p => p.Components)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Component_FK_TranslationID");
//        });

//        modelBuilder.Entity<ComponentPrice>(entity =>
//        {
//            entity.HasKey(e => new { e.DiameterID, e.ComponentID }).HasName("ComponentPrice_PK");

//            entity.ToTable("ComponentPrice", "quote_adv");

//            entity.Property(e => e.DiameterID).HasColumnName("DiameterID");
//            entity.Property(e => e.ComponentID).HasColumnName("ComponentID");
//            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

//            entity.HasOne(d => d.Component).WithMany(p => p.ComponentPrices)
//                .HasForeignKey(d => d.ComponentID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("ComponentPrice_FK_ComponentID");

//            entity.HasOne(d => d.Diameter).WithMany(p => p.ComponentPrices)
//                .HasForeignKey(d => d.DiameterID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("ComponentPrice_FK_DiameterID");
//        });

//        modelBuilder.Entity<Customer>(entity =>
//        {
//            entity.HasKey(e => e.CustomerID).HasName("PK__Customer__A4AE64B8EE2C377E");

//            entity.ToTable("Customer", "quote");

//            entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
//            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.Translation).WithMany(p => p.Customers)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Customer_FK_TranslationID");
//        });

//        modelBuilder.Entity<Language>(entity =>
//        {
//            entity.HasKey(e => e.LanguageID).HasName("PK__Language__B938558B5F000F94");

//            entity.ToTable("Language", "system");

//            entity.Property(e => e.LanguageID).HasColumnName("LanguageID");
//            entity.Property(e => e.DisplayName)
//                .HasMaxLength(100)
//                .IsUnicode(false);
//            entity.Property(e => e.LanguageCode)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<PipeDiameter>(entity =>
//        {
//            entity.HasKey(e => e.DiameterID).HasName("PK__PipeDiam__4B38DA1F4EC8397F");

//            entity.ToTable("PipeDiameter", "quote");

//            entity.Property(e => e.DiameterID).HasColumnName("DiameterID");
//            entity.Property(e => e.Diameter).HasColumnType("decimal(5, 2)");
//            entity.Property(e => e.DisplayName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<PipePrice>(entity =>
//        {
//            entity.HasKey(e => new { e.DiameterID, e.ThicknessID }).HasName("PipePrice_PK");

//            entity.ToTable("PipePrice", "quote_adv");

//            entity.Property(e => e.DiameterID).HasColumnName("DiameterID");
//            entity.Property(e => e.ThicknessID).HasColumnName("ThicknessID");
//            entity.Property(e => e.PipeUnitPrice).HasColumnType("decimal(6, 2)");

//            entity.HasOne(d => d.Diameter).WithMany(p => p.PipePrices)
//                .HasForeignKey(d => d.DiameterID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipePrice_FK_DiameterID");

//            entity.HasOne(d => d.Thickness).WithMany(p => p.PipePrices)
//                .HasForeignKey(d => d.ThicknessID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipePrice_FK_ThicknessID");
//        });

//        modelBuilder.Entity<PipeThickness>(entity =>
//        {
//            entity.HasKey(e => e.ThicknessID).HasName("PK__PipeThic__4B497D9AC52439FF");

//            entity.ToTable("PipeThickness", "quote");

//            entity.Property(e => e.ThicknessID).HasColumnName("ThicknessID");
//            entity.Property(e => e.DisplayName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<PipeTop>(entity =>
//        {
//            entity.HasKey(e => e.PipeTopID).HasName("PK__PipeTop__7FD028EE5081DDB2");

//            entity.ToTable("PipeTop", "quote");

//            entity.Property(e => e.PipeTopID).HasColumnName("PipeTopID");
//            entity.Property(e => e.PipeTopName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.PricingMethodNavigation).WithMany(p => p.PipeTops)
//                .HasForeignKey(d => d.PricingMethod)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipeTop_FK_PricingMethod");

//            entity.HasOne(d => d.Translation).WithMany(p => p.PipeTops)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipeTop_FK_TranslationID");
//        });

//        modelBuilder.Entity<PipeTopPrice>(entity =>
//        {
//            entity.HasKey(e => new { e.DiameterID, e.PipeTopID }).HasName("PipeTopPrice_PK");

//            entity.ToTable("PipeTopPrice", "quote_adv");

//            entity.Property(e => e.DiameterID).HasColumnName("DiameterID");
//            entity.Property(e => e.PipeTopID).HasColumnName("PipeTopID");
//            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");

//            entity.HasOne(d => d.Diameter).WithMany(p => p.PipeTopPrices)
//                .HasForeignKey(d => d.DiameterID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipeTopPrice_FK_DiameterID");

//            entity.HasOne(d => d.PipeTop).WithMany(p => p.PipeTopPrices)
//                .HasForeignKey(d => d.PipeTopID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipeTopPrice_FK_PipeTopID");
//        });

//        modelBuilder.Entity<PipeTopPricingMethod>(entity =>
//        {
//            entity.HasKey(e => e.MethodID).HasName("PK__PipeTopP__FC681FB13DBBAC84");

//            entity.ToTable("PipeTopPricingMethod", "quote");

//            entity.Property(e => e.MethodID).HasColumnName("MethodID");
//            entity.Property(e => e.MethodName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.Translation).WithMany(p => p.PipeTopPricingMethods)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("PipeTopPricingMethod_FK_TranslationID");
//        });

//        modelBuilder.Entity<Product>(entity =>
//        {
//            entity.HasKey(e => e.ProductID).HasName("PK__Product__B40CC6ED0106566E");

//            entity.ToTable("Product", "quote");

//            entity.Property(e => e.ProductID).HasColumnName("ProductID");
//            entity.Property(e => e.PipeDiscount).HasColumnType("decimal(5, 2)");
//            entity.Property(e => e.ProductName)
//                .HasMaxLength(100)
//                .IsUnicode(false);
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.Translation).WithMany(p => p.Products)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Product_FK_TranslationID");
//        });

//        modelBuilder.Entity<ProductPipeTopMapping>(entity =>
//        {
//            entity.HasKey(e => new { e.ProductID, e.PipeTopID }).HasName("ProductPipeTop_PK");

//            entity.ToTable("ProductPipeTopMapping", "quote_adv");

//            entity.Property(e => e.ProductID).HasColumnName("ProductID");
//            entity.Property(e => e.PipeTopID).HasColumnName("PipeTopID");

//            entity.HasOne(d => d.PipeTop).WithMany(p => p.ProductPipeTopMappings)
//                .HasForeignKey(d => d.PipeTopID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("ProductPipeTop_FK_PipeTopID");

//            entity.HasOne(d => d.Product).WithMany(p => p.ProductPipeTopMappings)
//                .HasForeignKey(d => d.ProductID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("ProductPipeTop_FK_ProductID");
//        });

//        modelBuilder.Entity<Stud>(entity =>
//        {
//            entity.HasKey(e => e.StudID).HasName("PK__StudPric__883D519B2A2F9CC1");

//            entity.ToTable("Stud", "quote");

//            entity.Property(e => e.StudID).HasColumnName("StudID");
//            entity.Property(e => e.DisplayName)
//                .HasMaxLength(30)
//                .IsUnicode(false);
//            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");
//        });

//        modelBuilder.Entity<Translation>(entity =>
//        {
//            entity.HasKey(e => new { e.TranslationID, e.LanguageID }).HasName("Translation_PK");

//            entity.ToTable("Translation", "quote_trans");

//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");
//            entity.Property(e => e.LanguageID).HasColumnName("LanguageID");
//            entity.Property(e => e.DisplayName)
//                .HasMaxLength(100)
//                .IsUnicode(false);

//            entity.HasOne(d => d.Language).WithMany(p => p.Translations)
//                .HasForeignKey(d => d.LanguageID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Translation_FK_LanguageID");

//            entity.HasOne(d => d.TranslationNavigation).WithMany(p => p.Translations)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Translation_FK_TranslationID");
//        });

//        modelBuilder.Entity<TranslationGroup>(entity =>
//        {
//            entity.HasKey(e => e.TranslationID).HasName("PK__Translat__663DA0AC37EFDC47");

//            entity.ToTable("TranslationGroup", "quote_trans");

//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");
//            entity.Property(e => e.Dummy)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
