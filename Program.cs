using Microsoft.EntityFrameworkCore;
using SAFORIX.Data;
using SAFORIX.Models;

var builder = WebApplication.CreateBuilder(args);

// ================= MVC =================
builder.Services.AddControllersWithViews();

// ================= DATABASE =================
builder.Services.AddDbContext<SaforixDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ================= SESSION =================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ================= DATABASE SETUP ON STARTUP =================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SaforixDbContext>();

    // Creates Registration, AdminUsers, UrlScanHistories (via EF model)
    db.Database.EnsureCreated();

    // ═══════════════════════════════════════════════════════════════
    // STEP 1: CREATE MISSING TABLES (if not exist)
    // ═══════════════════════════════════════════════════════════════

    // ── BehaviourScanHistories ──────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[BehaviourScanHistories]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[BehaviourScanHistories](
        [Id]              INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_BehaviourScanHistories] PRIMARY KEY,
        [UserId]          INT NULL,
        [Email]           NVARCHAR(150) NOT NULL,
        [EventType]       NVARCHAR(50)  NOT NULL,
        [LoginTime]       DATETIME2     NOT NULL,
        [DeviceName]      NVARCHAR(120) NULL,
        [DeviceType]      NVARCHAR(50)  NULL,
        [OperatingSystem] NVARCHAR(80)  NULL,
        [Browser]         NVARCHAR(80)  NULL,
        [IpAddress]       NVARCHAR(64)  NULL,
        [Location]        NVARCHAR(120) NULL,
        [RiskPercentage]  INT           NOT NULL,
        [RiskLevel]       NVARCHAR(20)  NOT NULL,
        [AnalysisResult]  NVARCHAR(500) NOT NULL,
        [ScannedAt]       DATETIME2     NOT NULL
    );
END
");

    // ── DeviceScanHistories ─────────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[DeviceScanHistories]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[DeviceScanHistories](
        [Id]                       INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_DeviceScanHistories] PRIMARY KEY,
        [UserId]                   INT NULL,
        [DeviceName]               NVARCHAR(120) NOT NULL,
        [DeviceType]               NVARCHAR(50)  NULL,
        [OperatingSystem]          NVARCHAR(80)  NULL,
        [IpAddress]                NVARCHAR(64)  NULL,
        [FirewallEnabled]          BIT           NOT NULL,
        [AntivirusEnabled]         BIT           NOT NULL,
        [DiskEncryptionEnabled]    BIT           NOT NULL,
        [OsUpToDate]               BIT           NOT NULL,
        [SuspiciousProcessesFound] BIT           NOT NULL,
        [UnknownUsbDetected]       BIT           NOT NULL,
        [OpenPortsCount]           INT           NOT NULL,
        [RiskPercentage]           INT           NOT NULL,
        [RiskLevel]                NVARCHAR(20)  NOT NULL,
        [AnalysisResult]           NVARCHAR(700) NOT NULL,
        [ScannedAt]                DATETIME2     NOT NULL
    );
END
");

    // ── HomeContents (replaces PageContents for Home page) ──────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[HomeContents]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HomeContents](
        [Id]         INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_HomeContents] PRIMARY KEY,
        [SectionKey] NVARCHAR(80)  NOT NULL,
        [Title]      NVARCHAR(200) NULL,
        [Body]       NVARCHAR(MAX) NULL,
        [SortOrder]  INT           NOT NULL DEFAULT 0,
        [IsActive]   BIT           NOT NULL DEFAULT 1,
        [CreatedAt]  DATETIME2     NOT NULL,
        [UpdatedAt]  DATETIME2     NULL
    );
END
");

    // ── AboutUsContents (replaces PageContents for About Us page) ───
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[AboutUsContents]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AboutUsContents](
        [Id]         INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AboutUsContents] PRIMARY KEY,
        [SectionKey] NVARCHAR(80)  NOT NULL,
        [Title]      NVARCHAR(200) NULL,
        [Body]       NVARCHAR(MAX) NULL,
        [SortOrder]  INT           NOT NULL DEFAULT 0,
        [IsActive]   BIT           NOT NULL DEFAULT 1,
        [CreatedAt]  DATETIME2     NOT NULL,
        [UpdatedAt]  DATETIME2     NULL
    );
END
");

    // ── ThreatContents (replaces PageContents for Threat page) ─────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[ThreatContents]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ThreatContents](
        [Id]         INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_ThreatContents] PRIMARY KEY,
        [SectionKey] NVARCHAR(80)  NOT NULL,
        [Title]      NVARCHAR(200) NULL,
        [Body]       NVARCHAR(MAX) NULL,
        [SortOrder]  INT           NOT NULL DEFAULT 0,
        [IsActive]   BIT           NOT NULL DEFAULT 1,
        [CreatedAt]  DATETIME2     NOT NULL,
        [UpdatedAt]  DATETIME2     NULL
    );
END
");

    // ── HeaderNavItems ──────────────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[HeaderNavItems]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HeaderNavItems](
        [Id]          INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_HeaderNavItems] PRIMARY KEY,
        [DisplayText] NVARCHAR(80) NOT NULL,
        [Controller]  NVARCHAR(80) NULL,
        [Action]      NVARCHAR(80) NULL,
        [ParentId]    INT          NULL,
        [SortOrder]   INT          NOT NULL DEFAULT 0,
        [IsActive]    BIT          NOT NULL DEFAULT 1
    );
END
");

    // ── SiteSettings ────────────────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[SiteSettings]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[SiteSettings](
        [Id]           INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_SiteSettings] PRIMARY KEY,
        [SettingKey]   NVARCHAR(80)  NOT NULL,
        [SettingValue] NVARCHAR(MAX) NULL,
        [Description]  NVARCHAR(200) NULL
    );
END
");

    // ── HomeHero ──────────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[HomeHero]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HomeHero](
        [Id]          INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_HomeHero] PRIMARY KEY,
        [MainTitle]   NVARCHAR(200) NOT NULL,
        [SubTitle]    NVARCHAR(200) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [ButtonText]  NVARCHAR(100) NULL,
        [ButtonLink]  NVARCHAR(200) NULL,
        [IsActive]    BIT NOT NULL DEFAULT 1,
        [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
");

    // ── HomeFeatures ──────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[HomeFeatures]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HomeFeatures](
        [Id]           INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_HomeFeatures] PRIMARY KEY,
        [Icon]         NVARCHAR(100) NULL,
        [Title]        NVARCHAR(150) NOT NULL,
        [Description]  NVARCHAR(MAX) NULL,
        [DisplayOrder] INT NOT NULL DEFAULT 0,
        [IsActive]     BIT NOT NULL DEFAULT 1
    );
END
");

    // ── AboutContent ─────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[AboutContent]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[AboutContent](
        [Id]          INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AboutContent] PRIMARY KEY,
        [Title]       NVARCHAR(200) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [IsActive]    BIT NOT NULL DEFAULT 1,
        [CreatedAt]   DATETIME2 NOT NULL DEFAULT GETDATE()
    );
END
");

    // ── MissionVision ────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[MissionVision]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[MissionVision](
        [Id]          INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_MissionVision] PRIMARY KEY,
        [Type]        NVARCHAR(50)  NOT NULL,
        [Title]       NVARCHAR(150) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [IsActive]    BIT NOT NULL DEFAULT 1
    );
END
");

    // ── HowItWorksSteps ─────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[HowItWorksSteps]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[HowItWorksSteps](
        [Id]          INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_HowItWorksSteps] PRIMARY KEY,
        [StepTitle]   NVARCHAR(150) NULL,
        [Description] NVARCHAR(MAX) NULL,
        [StepOrder]   INT NOT NULL DEFAULT 0,
        [IsActive]    BIT NOT NULL DEFAULT 1
    );
END
");

    // ── JourneyTimeline ────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF OBJECT_ID(N'[dbo].[JourneyTimeline]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[JourneyTimeline](
        [Id]           INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_JourneyTimeline] PRIMARY KEY,
        [Year]         INT NOT NULL,
        [Description]  NVARCHAR(MAX) NULL,
        [DisplayOrder] INT NOT NULL DEFAULT 0
    );
END
");

    // ═══════════════════════════════════════════════════════════════
    // STEP 2: ADD UserId COLUMN to scan tables (if column missing)
    //         Column added FIRST, FK constraint added SEPARATELY
    // ═══════════════════════════════════════════════════════════════

    // ── UrlScanHistories: column ────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[UrlScanHistories]') AND name = 'UserId')
BEGIN
    ALTER TABLE [dbo].[UrlScanHistories] ADD [UserId] INT NULL;
END
");
    // ── UrlScanHistories: FK ────────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_UrlScanHistories_Registration')
BEGIN
    ALTER TABLE [dbo].[UrlScanHistories]
        ADD CONSTRAINT [FK_UrlScanHistories_Registration]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Registration]([Id]) ON DELETE SET NULL;
END
");

    // ── BehaviourScanHistories: column ──────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[BehaviourScanHistories]') AND name = 'UserId')
BEGIN
    ALTER TABLE [dbo].[BehaviourScanHistories] ADD [UserId] INT NULL;
END
");
    // ── BehaviourScanHistories: FK ──────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BehaviourScanHistories_Registration')
BEGIN
    ALTER TABLE [dbo].[BehaviourScanHistories]
        ADD CONSTRAINT [FK_BehaviourScanHistories_Registration]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Registration]([Id]) ON DELETE SET NULL;
END
");

    // ── DeviceScanHistories: column ─────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[DeviceScanHistories]') AND name = 'UserId')
BEGIN
    ALTER TABLE [dbo].[DeviceScanHistories] ADD [UserId] INT NULL;
END
");
    // ── DeviceScanHistories: FK ─────────────────────────────────────
    db.Database.ExecuteSqlRaw(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DeviceScanHistories_Registration')
BEGIN
    ALTER TABLE [dbo].[DeviceScanHistories]
        ADD CONSTRAINT [FK_DeviceScanHistories_Registration]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Registration]([Id]) ON DELETE SET NULL;
END
");

    // ═══════════════════════════════════════════════════════════════
    // STEP 3: SEED DATA
    // ═══════════════════════════════════════════════════════════════

    var now = DateTime.Now;

    // ── Admin User ──────────────────────────────────────────────────
    if (!db.AdminUsers.Any())
    {
        db.AdminUsers.Add(new AdminUser { Username = "admin", Password = "Admin@123" });
        db.SaveChanges();
    }

    // ── Header Nav Items ────────────────────────────────────────────
    if (!db.HeaderNavItems.Any())
    {
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "Home",                Controller = "Home",   Action = "Index", ParentId = null, SortOrder = 1, IsActive = true });
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "About Us",            Controller = "About",  Action = "Index", ParentId = null, SortOrder = 2, IsActive = true });
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "Services",            Controller = null,     Action = null,    ParentId = null, SortOrder = 3, IsActive = true });
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "Threat Intelligence", Controller = "Threat", Action = "Index", ParentId = null, SortOrder = 4, IsActive = true });
        db.SaveChanges();
        int servicesId = db.HeaderNavItems.First(x => x.DisplayText == "Services").Id;
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "URL Scan",       Controller = "Services", Action = "UrlScan",       ParentId = servicesId, SortOrder = 1, IsActive = true });
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "Device Scan",    Controller = "Services", Action = "DeviceScan",    ParentId = servicesId, SortOrder = 2, IsActive = true });
        db.HeaderNavItems.Add(new HeaderNavItem { DisplayText = "Behaviour Scan", Controller = "Services", Action = "BehaviourScan", ParentId = servicesId, SortOrder = 3, IsActive = true });
        db.SaveChanges();
    }

    // ── Home Page (HomeContents table) ──────────────────────────────
    if (!db.HomeContents.Any())
    {
        db.HomeContents.AddRange(
            new HomeContent { SectionKey = "hero",        Title = "SAFORIX",                  Body = "Security Analysis Framework",                                                                                                  SortOrder = 1, IsActive = true, CreatedAt = now },
            new HomeContent { SectionKey = "description", Title = null,                        Body = "Advanced threat detection powered by real-time URL analysis, device security scanning, and intelligent behaviour monitoring.", SortOrder = 2, IsActive = true, CreatedAt = now },
            new HomeContent { SectionKey = "feature1",    Title = "⚡ Real-time Analysis",     Body = "Instant threat detection across URLs, devices, and behaviour patterns.",                                                       SortOrder = 3, IsActive = true, CreatedAt = now },
            new HomeContent { SectionKey = "feature2",    Title = "🔒 Secure Processing",      Body = "All scans processed securely with zero data retention after analysis.",                                                        SortOrder = 4, IsActive = true, CreatedAt = now },
            new HomeContent { SectionKey = "feature3",    Title = "👁 Behaviour Detection",    Body = "Monitor login patterns and flag anomalies before they become threats.",                                                        SortOrder = 5, IsActive = true, CreatedAt = now }
        );
        db.SaveChanges();
    }

    // ── About Us Page (AboutUsContents table) ───────────────────────
    if (!db.AboutUsContents.Any())
    {
        db.AboutUsContents.AddRange(
            // Hero
            new AboutUsContent { SectionKey = "hero_title",       Title = "SAFORIX",                 Body = null,                                                                                                                                                                                                                        SortOrder = 1,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "hero_body",        Title = null,                       Body = "SAFORIX is an intelligent Security Analysis Framework designed to identify, evaluate, and prevent cyber threats before they cause damage. We combine behaviour monitoring, risk evaluation, and real-time analysis to protect users in a connected digital world.", SortOrder = 2, IsActive = true, CreatedAt = now },
            // Mission & Vision
            new AboutUsContent { SectionKey = "mv_title",         Title = "Our Mission & Vision",     Body = null,                                                                                                                                                                                                                        SortOrder = 3,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "mv_subtitle",      Title = null,                       Body = "Purpose-driven security for a digital future",                                                                                                                                                                              SortOrder = 4,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "mission",          Title = "Our Mission",              Body = "Our mission is to proactively safeguard users against modern cyber threats such as malicious URLs, unsafe devices, and abnormal user behaviour. SAFORIX continuously monitors, evaluates, and responds to risks in real time to ensure maximum digital safety.", SortOrder = 5, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "vision",           Title = "Our Vision",               Body = "We envision SAFORIX as a globally trusted cyber defense platform that empowers individuals and organizations with transparent, intelligent, and scalable security insights that evolve with emerging threats.",               SortOrder = 6,  IsActive = true, CreatedAt = now },
            // How it Works
            new AboutUsContent { SectionKey = "how_title",        Title = "How SAFORIX Works",        Body = null,                                                                                                                                                                                                                        SortOrder = 7,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_subtitle",     Title = null,                       Body = "Intelligent security decision pipeline",                                                                                                                                                                                    SortOrder = 8,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_1_title",      Title = "📥 Data Input",            Body = "URLs, device details, and behavioural signals are collected securely.",                                                                                                                                                      SortOrder = 9,  IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_1_body",       Title = null,                       Body = "URLs, device details, and behavioural signals are collected securely.",                                                                                                                                                      SortOrder = 10, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_2_title",      Title = "📊 Risk Evaluation",       Body = "Data is analysed using rule-based logic and scoring mechanisms.",                                                                                                                                                           SortOrder = 11, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_2_body",       Title = null,                       Body = "Data is analysed using rule-based logic and scoring mechanisms.",                                                                                                                                                           SortOrder = 12, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_3_title",      Title = "👁 Behaviour Analysis",    Body = "Suspicious and abnormal patterns are identified in real time.",                                                                                                                                                              SortOrder = 13, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_3_body",       Title = null,                       Body = "Suspicious and abnormal patterns are identified in real time.",                                                                                                                                                              SortOrder = 14, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_4_title",      Title = "🛑 Security Decision",     Body = "Threats are classified and appropriate actions are recommended.",                                                                                                                                                           SortOrder = 15, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "how_4_body",       Title = null,                       Body = "Threats are classified and appropriate actions are recommended.",                                                                                                                                                           SortOrder = 16, IsActive = true, CreatedAt = now },
            // Philosophy
            new AboutUsContent { SectionKey = "philo_title",      Title = "Our Security Philosophy",  Body = null,                                                                                                                                                                                                                        SortOrder = 17, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_subtitle",   Title = null,                       Body = "Security is a continuous process, not a one-time action",                                                                                                                                                                   SortOrder = 18, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_1_title",    Title = "🔍 Detect Early",          Body = "Identify threats at the earliest possible stage.",                                                                                                                                                                           SortOrder = 19, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_1_body",     Title = null,                       Body = "Identify threats at the earliest possible stage.",                                                                                                                                                                           SortOrder = 20, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_2_title",    Title = "🧠 Analyze Deeply",        Body = "Understand intent, patterns, and risk context.",                                                                                                                                                                             SortOrder = 21, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_2_body",     Title = null,                       Body = "Understand intent, patterns, and risk context.",                                                                                                                                                                             SortOrder = 22, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_3_title",    Title = "⚡ Respond Fast",          Body = "Deliver actionable results with minimal delay.",                                                                                                                                                                              SortOrder = 23, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_3_body",     Title = null,                       Body = "Deliver actionable results with minimal delay.",                                                                                                                                                                              SortOrder = 24, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_4_title",    Title = "🔐 Protect Always",        Body = "Ensure continuous protection and monitoring.",                                                                                                                                                                                SortOrder = 25, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "philo_4_body",     Title = null,                       Body = "Ensure continuous protection and monitoring.",                                                                                                                                                                                SortOrder = 26, IsActive = true, CreatedAt = now },
            // Journey
            new AboutUsContent { SectionKey = "journey_title",    Title = "Our Journey",              Body = null,                                                                                                                                                                                                                        SortOrder = 27, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_subtitle", Title = null,                       Body = "From concept to intelligent security platform",                                                                                                                                                                              SortOrder = 28, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_1_year",   Title = "2024",                     Body = null,                                                                                                                                                                                                                        SortOrder = 29, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_1_text",   Title = null,                       Body = "Research, threat modelling, and architecture design",                                                                                                                                                                        SortOrder = 30, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_2_year",   Title = "2025",                     Body = null,                                                                                                                                                                                                                        SortOrder = 31, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_2_text",   Title = null,                       Body = "Development of core scanning and behaviour modules",                                                                                                                                                                         SortOrder = 32, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_3_year",   Title = "2026",                     Body = null,                                                                                                                                                                                                                        SortOrder = 33, IsActive = true, CreatedAt = now },
            new AboutUsContent { SectionKey = "journey_3_text",   Title = null,                       Body = "Launch of SAFORIX platform with admin & user dashboards",                                                                                                                                                                    SortOrder = 34, IsActive = true, CreatedAt = now }
        );
        db.SaveChanges();
    }

    // ── Threat Intelligence Page (ThreatContents table) ─────────────
    if (!db.ThreatContents.Any())
    {
        db.ThreatContents.AddRange(
            // Hero
            new ThreatContent { SectionKey = "hero",             Title = "Threat Intelligence & Security Awareness", Body = "Understand modern cyber threats and learn how SAFORIX protects you",   SortOrder = 1,  IsActive = true, CreatedAt = now },
            // Threat Cards
            new ThreatContent { SectionKey = "threat_1_title",   Title = "🎣 Phishing Attacks",          Body = "Fake websites and emails designed to steal login credentials, banking details, or personal data.",          SortOrder = 2,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "threat_2_title",   Title = "🦠 Malware & Ransomware",      Body = "Malicious software that damages systems, steals information, or locks files for ransom.",                   SortOrder = 3,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "threat_3_title",   Title = "🔗 Scam URLs",                 Body = "Dangerous links that redirect users to fake or harmful websites pretending to be legitimate.",              SortOrder = 4,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "threat_4_title",   Title = "🧠 Behaviour Anomalies",       Body = "Unusual login times, devices, or locations that may indicate account compromise.",                          SortOrder = 5,  IsActive = true, CreatedAt = now },
            // Protection
            new ThreatContent { SectionKey = "protection_title", Title = "How SAFORIX Protects You",     Body = null,                                                                                                        SortOrder = 6,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "protection_1",     Title = null,                           Body = "✔ Real-time URL risk analysis with percentage scoring",                                                    SortOrder = 7,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "protection_2",     Title = null,                           Body = "✔ Device security evaluation based on system configuration",                                               SortOrder = 8,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "protection_3",     Title = null,                           Body = "✔ Behaviour monitoring to detect suspicious activity",                                                     SortOrder = 9,  IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "protection_4",     Title = null,                           Body = "✔ Centralized scan history for tracking threats",                                                         SortOrder = 10, IsActive = true, CreatedAt = now },
            // Tips
            new ThreatContent { SectionKey = "tips_title",       Title = "Cyber Safety Tips",            Body = null,                                                                                                        SortOrder = 11, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_1",            Title = null,                           Body = "Always verify website URLs before clicking",                                                               SortOrder = 12, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_2",            Title = null,                           Body = "Avoid unknown email attachments",                                                                          SortOrder = 13, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_3",            Title = null,                           Body = "Use strong and unique passwords",                                                                          SortOrder = 14, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_4",            Title = null,                           Body = "Keep OS and antivirus updated",                                                                            SortOrder = 15, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_5",            Title = null,                           Body = "Avoid public Wi-Fi for sensitive logins",                                                                  SortOrder = 16, IsActive = true, CreatedAt = now },
            new ThreatContent { SectionKey = "tip_6",            Title = null,                           Body = "Enable two-factor authentication",                                                                         SortOrder = 17, IsActive = true, CreatedAt = now }
        );
        db.SaveChanges();
    }

    // ── Site Settings ───────────────────────────────────────────────
    if (!db.SiteSettings.Any())
    {
        db.SiteSettings.AddRange(
            new SiteSetting { SettingKey = "LogoText",        SettingValue = "SAFORIX",                                     Description = "Main logo text" },
            new SiteSetting { SettingKey = "Tagline",         SettingValue = "Security Analysis Framework",                 Description = "Header tagline" },
            new SiteSetting { SettingKey = "FooterCopyright", SettingValue = "© 2026 SAFORIX — Security Analysis Framework", Description = "Footer copyright text" }
        );
        db.SaveChanges();
    }

    // ═══════════════════════════════════════════════════════════════
    // STEP 4: SEED NEW STRUCTURED TABLES
    // ═══════════════════════════════════════════════════════════════

    // ── HomeHero seed ───────────────────────────────────────────────
    if (!db.HomeHeroes.Any())
    {
        db.HomeHeroes.Add(new HomeHero
        {
            MainTitle   = "SAFORIX",
            SubTitle    = "Security Analysis Framework",
            Description = "Advanced threat detection powered by real-time URL analysis, device security scanning, and intelligent behaviour monitoring.",
            ButtonText  = "Get Started →",
            ButtonLink  = "/Services/UrlScan",
            IsActive    = true,
            CreatedAt   = now
        });
        db.SaveChanges();
    }

    // ── HomeFeatures seed ───────────────────────────────────────────
    if (!db.HomeFeatures.Any())
    {
        db.HomeFeatures.AddRange(
            new HomeFeature { Icon = "⚡", Title = "Real-time Analysis",   Description = "Instant threat detection across URLs, devices, and behaviour patterns.",             DisplayOrder = 1, IsActive = true },
            new HomeFeature { Icon = "🔒", Title = "Secure Processing",    Description = "All scans processed securely with zero data retention after analysis.",              DisplayOrder = 2, IsActive = true },
            new HomeFeature { Icon = "👁", Title = "Behaviour Detection",  Description = "Monitor login patterns and flag anomalies before they become threats.",              DisplayOrder = 3, IsActive = true },
            new HomeFeature { Icon = "🛡", Title = "URL Shield",           Description = "Evaluate links in real time with detailed risk percentage scoring.",                 DisplayOrder = 4, IsActive = true },
            new HomeFeature { Icon = "🖥", Title = "Device Guard",         Description = "Comprehensive hardware and software security audit for your device.",                DisplayOrder = 5, IsActive = true },
            new HomeFeature { Icon = "📊", Title = "Threat Reports",       Description = "Detailed actionable reports to understand and resolve identified security risks.",   DisplayOrder = 6, IsActive = true }
        );
        db.SaveChanges();
    }

    // ── AboutPageContent seed ───────────────────────────────────────
    if (!db.AboutPageContents.Any())
    {
        db.AboutPageContents.Add(new AboutPageContent
        {
            Title       = "About SAFORIX",
            Description = "SAFORIX is an intelligent Security Analysis Framework designed to identify, evaluate, and prevent cyber threats before they cause damage. We combine behaviour monitoring, URL risk scoring, and device analysis to protect users in an ever-evolving digital landscape.",
            IsActive    = true,
            CreatedAt   = now
        });
        db.SaveChanges();
    }

    // ── MissionVision seed ──────────────────────────────────────────
    if (!db.MissionVisions.Any())
    {
        db.MissionVisions.AddRange(
            new MissionVision
            {
                Type        = "Mission",
                Title       = "Our Mission",
                Description = "To proactively safeguard users against modern cyber threats such as malicious URLs, unsafe devices, and abnormal behaviour. SAFORIX evaluates and responds to risks in real time to ensure maximum digital safety.",
                IsActive    = true
            },
            new MissionVision
            {
                Type        = "Vision",
                Title       = "Our Vision",
                Description = "A globally trusted cyber defense platform that empowers individuals and organizations with transparent, intelligent, and scalable security insights that evolve with emerging threats.",
                IsActive    = true
            }
        );
        db.SaveChanges();
    }

    // ── HowItWorksSteps seed ────────────────────────────────────────
    if (!db.HowItWorksSteps.Any())
    {
        db.HowItWorksSteps.AddRange(
            new HowItWorksStep { StepTitle = "📥 Data Input",        Description = "URLs, device details, and behavioural signals are collected securely from the user.",  StepOrder = 1, IsActive = true },
            new HowItWorksStep { StepTitle = "📊 Risk Evaluation",   Description = "Data is analysed using rule-based logic and a multi-factor risk scoring mechanism.", StepOrder = 2, IsActive = true },
            new HowItWorksStep { StepTitle = "👁 Behaviour Analysis", Description = "Suspicious and abnormal patterns are identified and compared against known baselines.", StepOrder = 3, IsActive = true },
            new HowItWorksStep { StepTitle = "🛑 Security Decision", Description = "Threats are classified by severity and appropriate protective actions are recommended.", StepOrder = 4, IsActive = true }
        );
        db.SaveChanges();
    }

    // ── JourneyTimeline seed ────────────────────────────────────────
    if (!db.JourneyTimelines.Any())
    {
        db.JourneyTimelines.AddRange(
            new JourneyTimeline { Year = 2024, Description = "Research, threat modelling, and architecture design began for SAFORIX platform.",      DisplayOrder = 1 },
            new JourneyTimeline { Year = 2025, Description = "Development of core scanning engine: URL analysis, device scan, and behaviour modules.", DisplayOrder = 2 },
            new JourneyTimeline { Year = 2026, Description = "Launch of SAFORIX with full admin panel, user dashboard, and real-time threat reports.",  DisplayOrder = 3 }
        );
        db.SaveChanges();
    }
}

// ================= MIDDLEWARE ORDER =================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// ✅ SESSION MUST COME BEFORE CONTROLLERS
app.UseSession();

app.UseAuthorization();

// ================= ROUTES =================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LoginRegistration}/{id?}"
);

app.MapControllerRoute(
    name: "admin-login",
    pattern: "Admin/Login",
    defaults: new { controller = "AdminAccount", action = "Login" }
);

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=AdminDashboard}/{action=Index}/{id?}"
);

app.Run();
