using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Brandsome.DAL.Models
{
    public partial class BrandsomeDbContext : DbContext
    {
        public BrandsomeDbContext()
        {
        }

        public BrandsomeDbContext(DbContextOptions<BrandsomeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<BoostMetrcEntityType> BoostMetrcEntityTypes { get; set; }
        public virtual DbSet<BoostMetric> BoostMetrics { get; set; }
        public virtual DbSet<BoostMetricLevel> BoostMetricLevels { get; set; }
        public virtual DbSet<BoostMetricRewardLog> BoostMetricRewardLogs { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<BusinessCity> BusinessCities { get; set; }
        public virtual DbSet<BusinessFollow> BusinessFollows { get; set; }
        public virtual DbSet<BusinessPhoneClick> BusinessPhoneClicks { get; set; }
        public virtual DbSet<BusinessReview> BusinessReviews { get; set; }
        public virtual DbSet<BusinessService> BusinessServices { get; set; }
        public virtual DbSet<BusinessView> BusinessViews { get; set; }
        public virtual DbSet<CampaignHistory> CampaignHistories { get; set; }
        public virtual DbSet<CampaignPool> CampaignPools { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostLike> PostLikes { get; set; }
        public virtual DbSet<PostMedium> PostMedia { get; set; }
        public virtual DbSet<PostType> PostTypes { get; set; }
        public virtual DbSet<PostView> PostViews { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=BrandSome;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Image).IsFixedLength();

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

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
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

            modelBuilder.Entity<BoostMetric>(entity =>
            {
                entity.HasOne(d => d.BoostMetricEntityType)
                    .WithMany(p => p.BoostMetrics)
                    .HasForeignKey(d => d.BoostMetricEntityTypeId)
                    .HasConstraintName("FK_BoostMetric_BoostMetrcEntityType");
            });

            modelBuilder.Entity<BoostMetricLevel>(entity =>
            {
                entity.HasOne(d => d.BoostMetric)
                    .WithMany(p => p.BoostMetricLevels)
                    .HasForeignKey(d => d.BoostMetricId)
                    .HasConstraintName("FK_BoostMetricLevel_BoostMetric");
            });

            modelBuilder.Entity<BoostMetricRewardLog>(entity =>
            {
                entity.HasOne(d => d.BoostMetricLevel)
                    .WithMany(p => p.BoostMetricRewardLogs)
                    .HasForeignKey(d => d.BoostMetricLevelId)
                    .HasConstraintName("FK_BoostMetricRewardLog_BoostMetricLevel");
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Businesses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Business_AspNetUsers");
            });

            modelBuilder.Entity<BusinessCity>(entity =>
            {
                entity.HasOne(d => d.Business)
                    .WithMany()
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_BusinessCity_Business");

                entity.HasOne(d => d.City)
                    .WithMany()
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_BusinessCity_City");
            });

            modelBuilder.Entity<BusinessFollow>(entity =>
            {
                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessFollows)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_BusinessFollow_Business");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessFollows)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BusinessFollow_AspNetUsers");
            });

            modelBuilder.Entity<BusinessPhoneClick>(entity =>
            {
                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessPhoneClicks)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_BusinessPhoneClick_Business");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessPhoneClicks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BusinessPhoneClick_AspNetUsers");
            });

            modelBuilder.Entity<BusinessReview>(entity =>
            {
                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessReviews)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_BusinessReview_Business");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessReviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BusinessReview_AspNetUsers");
            });

            modelBuilder.Entity<BusinessService>(entity =>
            {
                entity.HasOne(d => d.Business)
                    .WithMany(p => p.BusinessServices)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK_BusinessServices_Business");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.BusinessServices)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_BusinessServices_Services");
            });

            modelBuilder.Entity<BusinessView>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.BusinessViews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BusinessView_AspNetUsers");
            });

            modelBuilder.Entity<CampaignHistory>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.CampaignHistories)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_CampaignHistory_Post");
            });

            modelBuilder.Entity<CampaignPool>(entity =>
            {
                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignPools)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CampaignPool_CampaignHistory");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CampaignPools)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CampaignPool_City");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.CampaignPools)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_CampaignPool_Services");
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.HasOne(d => d.Devide)
                    .WithMany(p => p.Interests)
                    .HasForeignKey(d => d.DevideId)
                    .HasConstraintName("FK_Interest_Device");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Interests)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_Interest_Services");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostLike_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PostLike_AspNetUsers");
            });

            modelBuilder.Entity<PostMedium>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostMedia)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostMedia_Post");

                entity.HasOne(d => d.PostType)
                    .WithMany(p => p.PostMedia)
                    .HasForeignKey(d => d.PostTypeId)
                    .HasConstraintName("FK_PostMedia_PostType");
            });

            modelBuilder.Entity<PostView>(entity =>
            {
                entity.HasOne(d => d.Device)
                    .WithMany(p => p.PostViews)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK_PostView_Device");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostViews)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_PostView_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostViews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_PostView_AspNetUsers");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("FK_Services_SubCategory");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_SubCategory_Category");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
