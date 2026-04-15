using Microsoft.EntityFrameworkCore;
using SAFORIX.Models;

namespace SAFORIX.Data
{
    public class SaforixDbContext : DbContext
    {
        public SaforixDbContext(DbContextOptions<SaforixDbContext> options)
            : base(options)
        {
        }

        // ===================== TABLES =====================
        public DbSet<Registration> Registration { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<UrlScanHistory> UrlScanHistories { get; set; }
        public DbSet<BehaviourScanHistory> BehaviourScanHistories { get; set; }
        public DbSet<DeviceScanHistory> DeviceScanHistories { get; set; }

        // 3 Separate Page Content Tables (legacy key-based)
        public DbSet<HomeContent> HomeContents { get; set; }
        public DbSet<AboutUsContent> AboutUsContents { get; set; }
        public DbSet<ThreatContent> ThreatContents { get; set; }

        // ── NEW Structured Tables ───────────────────────────────────────
        public DbSet<HomeHero> HomeHeroes { get; set; }
        public DbSet<HomeFeature> HomeFeatures { get; set; }
        public DbSet<AboutPageContent> AboutPageContents { get; set; }
        public DbSet<MissionVision> MissionVisions { get; set; }
        public DbSet<HowItWorksStep> HowItWorksSteps { get; set; }
        public DbSet<JourneyTimeline> JourneyTimelines { get; set; }

        public DbSet<HeaderNavItem> HeaderNavItems { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }

        // ===================== MODEL CONFIG =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Registration
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.password).IsRequired();
            });

            // AdminUsers
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();
            });

            // BehaviourScanHistory
            modelBuilder.Entity<BehaviourScanHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.EventType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DeviceName).HasMaxLength(120);
                entity.Property(e => e.DeviceType).HasMaxLength(50);
                entity.Property(e => e.OperatingSystem).HasMaxLength(80);
                entity.Property(e => e.Browser).HasMaxLength(80);
                entity.Property(e => e.IpAddress).HasMaxLength(64);
                entity.Property(e => e.Location).HasMaxLength(120);
                entity.Property(e => e.RiskLevel).HasMaxLength(20);
                entity.Property(e => e.AnalysisResult).HasMaxLength(500);
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            });

            // DeviceScanHistory
            modelBuilder.Entity<DeviceScanHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DeviceName).IsRequired().HasMaxLength(120);
                entity.Property(e => e.DeviceType).HasMaxLength(50);
                entity.Property(e => e.OperatingSystem).HasMaxLength(80);
                entity.Property(e => e.IpAddress).HasMaxLength(64);
                entity.Property(e => e.RiskLevel).HasMaxLength(20);
                entity.Property(e => e.AnalysisResult).HasMaxLength(700);
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            });

            // UrlScanHistory
            modelBuilder.Entity<UrlScanHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Url).IsRequired();
                entity.Property(e => e.RiskLevel).HasMaxLength(50);
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            });

            // HomeContents
            modelBuilder.Entity<HomeContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HomeContents");
                entity.Property(e => e.SectionKey).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            // AboutUsContents
            modelBuilder.Entity<AboutUsContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AboutUsContents");
                entity.Property(e => e.SectionKey).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            // ThreatContents
            modelBuilder.Entity<ThreatContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("ThreatContents");
                entity.Property(e => e.SectionKey).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            // HomeHero
            modelBuilder.Entity<HomeHero>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HomeHero");
                entity.Property(e => e.MainTitle).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SubTitle).HasMaxLength(200);
                entity.Property(e => e.ButtonText).HasMaxLength(100);
                entity.Property(e => e.ButtonLink).HasMaxLength(200);
            });

            // HomeFeatures
            modelBuilder.Entity<HomeFeature>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HomeFeatures");
                entity.Property(e => e.Icon).HasMaxLength(100);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(150);
            });

            // AboutPageContents
            modelBuilder.Entity<AboutPageContent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("AboutContent");
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            // MissionVision
            modelBuilder.Entity<MissionVision>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("MissionVision");
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Title).HasMaxLength(150);
            });

            // HowItWorksSteps
            modelBuilder.Entity<HowItWorksStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HowItWorksSteps");
                entity.Property(e => e.StepTitle).HasMaxLength(150);
            });

            // JourneyTimeline
            modelBuilder.Entity<JourneyTimeline>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("JourneyTimeline");
            });

            // HeaderNavItems
            modelBuilder.Entity<HeaderNavItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DisplayText).IsRequired().HasMaxLength(80);
                entity.Property(e => e.Controller).HasMaxLength(80);
                entity.Property(e => e.Action).HasMaxLength(80);
            });

            // SiteSettings
            modelBuilder.Entity<SiteSetting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SettingKey).IsRequired().HasMaxLength(80);
                entity.Property(e => e.SettingValue);
                entity.Property(e => e.Description).HasMaxLength(200);
            });
        }
    }
}
