//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace QuotationSystem2.DataAccessLayer.Models.SystemModel2;

//public partial class SystemDbContext : DbContext
//{
//    public SystemDbContext()
//    {
//    }

//    public SystemDbContext(DbContextOptions<SystemDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Employee> Employees { get; set; }

//    public virtual DbSet<Language> Languages { get; set; }

//    public virtual DbSet<Role> Roles { get; set; }

//    public virtual DbSet<Translation> Translations { get; set; }

//    public virtual DbSet<TranslationGroup> TranslationGroups { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkID=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-MOSMRPO;Database=QuotationSystem2DB;User ID=ssss9178;Password=00000000;TrustServerCertificate=True;Trusted_Connection=True");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Employee>(entity =>
//        {
//            entity.HasKey(e => e.EmployeeID).HasName("PK__Employee__7AD04FF158B283F4");

//            entity.ToTable("Employee", "system");

//            entity.HasIndex(e => e.Account, "UQ_Employees_Account").IsUnique();

//            entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
//            entity.Property(e => e.Account)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.LastLogin)
//                .HasDefaultValueSql("(getdate())")
//                .HasColumnType("datetime");
//            entity.Property(e => e.Name)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.Password)
//                .HasMaxLength(50)
//                .IsUnicode(false)
//                .HasDefaultValue("00000000");
//            entity.Property(e => e.RoleID).HasColumnName("RoleID");

//            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
//                .HasForeignKey(d => d.RoleID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Employee_FK_RoleID");
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

//        modelBuilder.Entity<Role>(entity =>
//        {
//            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A9416A2B7");

//            entity.ToTable("Role", "system");

//            entity.Property(e => e.RoleID).HasColumnName("RoleID");
//            entity.Property(e => e.RoleName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.TranslationID).HasColumnName("TranslationID");

//            entity.HasOne(d => d.Translation).WithMany(p => p.Roles)
//                .HasForeignKey(d => d.TranslationID)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("Role_FK_TranslationID");
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
