-- ============================================================
-- SAFORIX - ALTER TABLE Script
-- Add UserId (FK → Registration.Id) to 3 Scan History Tables
-- Run this in SQL Server Management Studio (SSMS)
-- ============================================================

-- ────────────────────────────────────────────────────────────
-- 1. UrlScanHistories
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[UrlScanHistories]')
      AND name = 'UserId'
)
BEGIN
    ALTER TABLE [dbo].[UrlScanHistories]
        ADD [UserId] INT NULL;

    ALTER TABLE [dbo].[UrlScanHistories]
        ADD CONSTRAINT [FK_UrlScanHistories_Registration]
        FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Registration]([Id])
        ON DELETE SET NULL;

    PRINT 'UrlScanHistories: UserId column + FK added.';
END
ELSE
BEGIN
    PRINT 'UrlScanHistories: UserId already exists. Skipped.';
END

GO

-- ────────────────────────────────────────────────────────────
-- 2. BehaviourScanHistories
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[BehaviourScanHistories]')
      AND name = 'UserId'
)
BEGIN
    ALTER TABLE [dbo].[BehaviourScanHistories]
        ADD [UserId] INT NULL;

    ALTER TABLE [dbo].[BehaviourScanHistories]
        ADD CONSTRAINT [FK_BehaviourScanHistories_Registration]
        FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Registration]([Id])
        ON DELETE SET NULL;

    PRINT 'BehaviourScanHistories: UserId column + FK added.';
END
ELSE
BEGIN
    PRINT 'BehaviourScanHistories: UserId already exists. Skipped.';
END

GO

-- ────────────────────────────────────────────────────────────
-- 3. DeviceScanHistories
-- ────────────────────────────────────────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[DeviceScanHistories]')
      AND name = 'UserId'
)
BEGIN
    ALTER TABLE [dbo].[DeviceScanHistories]
        ADD [UserId] INT NULL;

    ALTER TABLE [dbo].[DeviceScanHistories]
        ADD CONSTRAINT [FK_DeviceScanHistories_Registration]
        FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Registration]([Id])
        ON DELETE SET NULL;

    PRINT 'DeviceScanHistories: UserId column + FK added.';
END
ELSE
BEGIN
    PRINT 'DeviceScanHistories: UserId already exists. Skipped.';
END

GO

-- ────────────────────────────────────────────────────────────
-- VERIFY: Check columns added successfully
-- ────────────────────────────────────────────────────────────
SELECT
    t.name  AS TableName,
    c.name  AS ColumnName,
    tp.name AS DataType,
    c.is_nullable AS IsNullable
FROM sys.tables t
JOIN sys.columns c ON c.object_id = t.object_id
JOIN sys.types tp ON tp.user_type_id = c.user_type_id
WHERE t.name IN ('UrlScanHistories','BehaviourScanHistories','DeviceScanHistories')
  AND c.name = 'UserId'
ORDER BY t.name;
