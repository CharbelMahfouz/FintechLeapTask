using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DAL.Models;

namespace DAL.Data
{
    public partial class FintechLeapDbContext : DbContext
    {
        public FintechLeapDbContext()
        {
        }

        public FintechLeapDbContext(DbContextOptions<FintechLeapDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccResetPasswordDetail> AccResetPasswordDetails { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccResetPasswordDetail>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AccResetPasswordDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ACC_ResetPasswordDetails_AspNetUsers");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");
                        });
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<ClientProfile>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.ClientProfiles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ClientProfile_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
