using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChemiClean.Models
{
    public partial class ChemiCleanContext : DbContext
    {
       
        public ChemiCleanContext(DbContextOptions<ChemiCleanContext> options)
            : base(options)
        {
        }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<ProductDataSheets> ProductDataSheets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=MARCO-PC\\MSSQLSERVER01;Initial Catalog=ChemiClean;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.SupplierName).HasMaxLength(250);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });
        }
    }
}
