-- ============================================================
-- SAFORIX - PageContents Table: Full Seed Data
-- Home Page, About Us Page, Threat Intelligence Page
-- Run in SQL Server Management Studio (SSMS)
-- ============================================================

-- ────────────────────────────────────────────────────────────
-- CREATE TABLE (if not already exists)
-- ────────────────────────────────────────────────────────────
IF OBJECT_ID(N'[dbo].[PageContents]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PageContents](
        [Id]         INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_PageContents] PRIMARY KEY,
        [PageKey]    NVARCHAR(50)  NOT NULL,
        [SectionKey] NVARCHAR(80)  NOT NULL,
        [Title]      NVARCHAR(200) NULL,
        [Body]       NVARCHAR(MAX) NULL,
        [SortOrder]  INT           NOT NULL DEFAULT 0,
        [IsActive]   BIT           NOT NULL DEFAULT 1,
        [CreatedAt]  DATETIME2     NOT NULL DEFAULT GETDATE(),
        [UpdatedAt]  DATETIME2     NULL
    );
    PRINT 'PageContents table created.';
END
GO

-- ────────────────────────────────────────────────────────────
-- HOME PAGE  (PageKey = 'Home')
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM [dbo].[PageContents] WHERE PageKey = 'Home')
BEGIN
    INSERT INTO [dbo].[PageContents] (PageKey, SectionKey, Title, Body, SortOrder, IsActive, CreatedAt)
    VALUES
        ('Home', 'hero',        'SAFORIX',                  'Security Analysis Framework',                                                                                                  1, 1, GETDATE()),
        ('Home', 'description', NULL,                       'Advanced threat detection powered by real-time URL analysis, device security scanning, and intelligent behaviour monitoring.',  2, 1, GETDATE()),
        ('Home', 'feature1',    '⚡ Real-time Analysis',    'Instant threat detection across URLs, devices, and behaviour patterns.',                                                        3, 1, GETDATE()),
        ('Home', 'feature2',    '🔒 Secure Processing',     'All scans processed securely with zero data retention after analysis.',                                                         4, 1, GETDATE()),
        ('Home', 'feature3',    '👁 Behaviour Detection',   'Monitor login patterns and flag anomalies before they become threats.',                                                         5, 1, GETDATE());

    PRINT 'Home page content inserted.';
END
ELSE
    PRINT 'Home page content already exists. Skipped.';
GO

-- ────────────────────────────────────────────────────────────
-- ABOUT US PAGE  (PageKey = 'AboutUs')
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM [dbo].[PageContents] WHERE PageKey = 'AboutUs')
BEGIN
    INSERT INTO [dbo].[PageContents] (PageKey, SectionKey, Title, Body, SortOrder, IsActive, CreatedAt)
    VALUES
    -- Hero
    ('AboutUs', 'hero_title',       'SAFORIX',                      NULL,                                                                                                                                                                                                                                                  1,  1, GETDATE()),
    ('AboutUs', 'hero_body',        NULL,                           'SAFORIX is an intelligent Security Analysis Framework designed to identify, evaluate, and prevent cyber threats before they cause damage. We combine behaviour monitoring, risk evaluation, and real-time analysis to protect users in a connected digital world.',  2,  1, GETDATE()),

    -- Mission & Vision
    ('AboutUs', 'mv_title',         'Our Mission & Vision',         NULL,                                                                                                                                                                                                                                                  3,  1, GETDATE()),
    ('AboutUs', 'mv_subtitle',      NULL,                           'Purpose-driven security for a digital future',                                                                                                                                                                                                        4,  1, GETDATE()),
    ('AboutUs', 'mission',          'Our Mission',                  'Our mission is to proactively safeguard users against modern cyber threats such as malicious URLs, unsafe devices, and abnormal user behaviour. SAFORIX continuously monitors, evaluates, and responds to risks in real time to ensure maximum digital safety.',  5,  1, GETDATE()),
    ('AboutUs', 'vision',           'Our Vision',                   'We envision SAFORIX as a globally trusted cyber defense platform that empowers individuals and organizations with transparent, intelligent, and scalable security insights that evolve with emerging threats.',                                          6,  1, GETDATE()),

    -- How it Works
    ('AboutUs', 'how_title',        'How SAFORIX Works',            NULL,                                                                                                                                                                                                                                                  7,  1, GETDATE()),
    ('AboutUs', 'how_subtitle',     NULL,                           'Intelligent security decision pipeline',                                                                                                                                                                                                              8,  1, GETDATE()),
    ('AboutUs', 'how_1_title',      '📥 Data Input',                'URLs, device details, and behavioural signals are collected securely.',                                                                                                                                                                               9,  1, GETDATE()),
    ('AboutUs', 'how_1_body',       NULL,                           'URLs, device details, and behavioural signals are collected securely.',                                                                                                                                                                               10, 1, GETDATE()),
    ('AboutUs', 'how_2_title',      '📊 Risk Evaluation',           'Data is analysed using rule-based logic and scoring mechanisms.',                                                                                                                                                                                     11, 1, GETDATE()),
    ('AboutUs', 'how_2_body',       NULL,                           'Data is analysed using rule-based logic and scoring mechanisms.',                                                                                                                                                                                     12, 1, GETDATE()),
    ('AboutUs', 'how_3_title',      '👁 Behaviour Analysis',        'Suspicious and abnormal patterns are identified in real time.',                                                                                                                                                                                       13, 1, GETDATE()),
    ('AboutUs', 'how_3_body',       NULL,                           'Suspicious and abnormal patterns are identified in real time.',                                                                                                                                                                                       14, 1, GETDATE()),
    ('AboutUs', 'how_4_title',      '🛑 Security Decision',         'Threats are classified and appropriate actions are recommended.',                                                                                                                                                                                     15, 1, GETDATE()),
    ('AboutUs', 'how_4_body',       NULL,                           'Threats are classified and appropriate actions are recommended.',                                                                                                                                                                                     16, 1, GETDATE()),

    -- Security Philosophy
    ('AboutUs', 'philo_title',      'Our Security Philosophy',      NULL,                                                                                                                                                                                                                                                  17, 1, GETDATE()),
    ('AboutUs', 'philo_subtitle',   NULL,                           'Security is a continuous process, not a one-time action',                                                                                                                                                                                             18, 1, GETDATE()),
    ('AboutUs', 'philo_1_title',    '🔍 Detect Early',              'Identify threats at the earliest possible stage.',                                                                                                                                                                                                    19, 1, GETDATE()),
    ('AboutUs', 'philo_1_body',     NULL,                           'Identify threats at the earliest possible stage.',                                                                                                                                                                                                    20, 1, GETDATE()),
    ('AboutUs', 'philo_2_title',    '🧠 Analyze Deeply',            'Understand intent, patterns, and risk context.',                                                                                                                                                                                                      21, 1, GETDATE()),
    ('AboutUs', 'philo_2_body',     NULL,                           'Understand intent, patterns, and risk context.',                                                                                                                                                                                                      22, 1, GETDATE()),
    ('AboutUs', 'philo_3_title',    '⚡ Respond Fast',              'Deliver actionable results with minimal delay.',                                                                                                                                                                                                      23, 1, GETDATE()),
    ('AboutUs', 'philo_3_body',     NULL,                           'Deliver actionable results with minimal delay.',                                                                                                                                                                                                      24, 1, GETDATE()),
    ('AboutUs', 'philo_4_title',    '🔐 Protect Always',            'Ensure continuous protection and monitoring.',                                                                                                                                                                                                        25, 1, GETDATE()),
    ('AboutUs', 'philo_4_body',     NULL,                           'Ensure continuous protection and monitoring.',                                                                                                                                                                                                        26, 1, GETDATE()),

    -- Journey
    ('AboutUs', 'journey_title',    'Our Journey',                  NULL,                                                                                                                                                                                                                                                  27, 1, GETDATE()),
    ('AboutUs', 'journey_subtitle', NULL,                           'From concept to intelligent security platform',                                                                                                                                                                                                       28, 1, GETDATE()),
    ('AboutUs', 'journey_1_year',   '2024',                         NULL,                                                                                                                                                                                                                                                  29, 1, GETDATE()),
    ('AboutUs', 'journey_1_text',   NULL,                           'Research, threat modelling, and architecture design',                                                                                                                                                                                                 30, 1, GETDATE()),
    ('AboutUs', 'journey_2_year',   '2025',                         NULL,                                                                                                                                                                                                                                                  31, 1, GETDATE()),
    ('AboutUs', 'journey_2_text',   NULL,                           'Development of core scanning and behaviour modules',                                                                                                                                                                                                  32, 1, GETDATE()),
    ('AboutUs', 'journey_3_year',   '2026',                         NULL,                                                                                                                                                                                                                                                  33, 1, GETDATE()),
    ('AboutUs', 'journey_3_text',   NULL,                           'Launch of SAFORIX platform with admin & user dashboards',                                                                                                                                                                                             34, 1, GETDATE());

    PRINT 'About Us page content inserted.';
END
ELSE
    PRINT 'About Us page content already exists. Skipped.';
GO

-- ────────────────────────────────────────────────────────────
-- THREAT INTELLIGENCE PAGE  (PageKey = 'Threat')
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM [dbo].[PageContents] WHERE PageKey = 'Threat')
BEGIN
    INSERT INTO [dbo].[PageContents] (PageKey, SectionKey, Title, Body, SortOrder, IsActive, CreatedAt)
    VALUES
    -- Hero
    ('Threat', 'hero',              'Threat Intelligence & Security Awareness',  'Understand modern cyber threats and learn how SAFORIX protects you',   1, 1, GETDATE()),

    -- Threat Cards
    ('Threat', 'threat_1_title',    '🎣 Phishing Attacks',          'Fake websites and emails designed to steal login credentials, banking details, or personal data.',         2, 1, GETDATE()),
    ('Threat', 'threat_2_title',    '🦠 Malware & Ransomware',      'Malicious software that damages systems, steals information, or locks files for ransom.',                  3, 1, GETDATE()),
    ('Threat', 'threat_3_title',    '🔗 Scam URLs',                 'Dangerous links that redirect users to fake or harmful websites pretending to be legitimate.',             4, 1, GETDATE()),
    ('Threat', 'threat_4_title',    '🧠 Behaviour Anomalies',       'Unusual login times, devices, or locations that may indicate account compromise.',                         5, 1, GETDATE()),

    -- Protection
    ('Threat', 'protection_title',  'How SAFORIX Protects You',     NULL,                                                                                                       6, 1, GETDATE()),
    ('Threat', 'protection_1',      NULL,                           '✔ Real-time URL risk analysis with percentage scoring',                                                    7, 1, GETDATE()),
    ('Threat', 'protection_2',      NULL,                           '✔ Device security evaluation based on system configuration',                                               8, 1, GETDATE()),
    ('Threat', 'protection_3',      NULL,                           '✔ Behaviour monitoring to detect suspicious activity',                                                     9, 1, GETDATE()),
    ('Threat', 'protection_4',      NULL,                           '✔ Centralized scan history for tracking threats',                                                         10, 1, GETDATE()),

    -- Tips
    ('Threat', 'tips_title',        'Cyber Safety Tips',            NULL,                                                                                                       11, 1, GETDATE()),
    ('Threat', 'tip_1',             NULL,                           'Always verify website URLs before clicking',                                                               12, 1, GETDATE()),
    ('Threat', 'tip_2',             NULL,                           'Avoid unknown email attachments',                                                                          13, 1, GETDATE()),
    ('Threat', 'tip_3',             NULL,                           'Use strong and unique passwords',                                                                          14, 1, GETDATE()),
    ('Threat', 'tip_4',             NULL,                           'Keep OS and antivirus updated',                                                                            15, 1, GETDATE()),
    ('Threat', 'tip_5',             NULL,                           'Avoid public Wi-Fi for sensitive logins',                                                                  16, 1, GETDATE()),
    ('Threat', 'tip_6',             NULL,                           'Enable two-factor authentication',                                                                         17, 1, GETDATE());

    PRINT 'Threat Intelligence page content inserted.';
END
ELSE
    PRINT 'Threat page content already exists. Skipped.';
GO

-- ────────────────────────────────────────────────────────────
-- VERIFY: See all inserted data
-- ────────────────────────────────────────────────────────────
SELECT Id, PageKey, SectionKey, Title,
       LEFT(Body, 60) AS BodyPreview,
       SortOrder, IsActive
FROM [dbo].[PageContents]
WHERE PageKey IN ('Home', 'AboutUs', 'Threat')
ORDER BY PageKey, SortOrder;
