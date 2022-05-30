using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using IpService.Data;

namespace IpService.Data.DbContexts
{
    public partial class IpServiceContext : DbContext
    {
        public IpServiceContext()
        {
        }

        public IpServiceContext(DbContextOptions<IpServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IpDetail> IpDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IpDetail>(entity =>
            {
                entity.HasKey(e => e.Ip)
                    .HasName("PK__IpDetail__3214EC0B6F32AC41");

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Continent)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
