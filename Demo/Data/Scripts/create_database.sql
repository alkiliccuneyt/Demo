CREATE DATABASE [set-yazilim]
GO

USE [set-yazilim]
GO
-- Create Schemas
CREATE SCHEMA constants;
GO

CREATE SCHEMA finances;
GO

-- Create Tables
CREATE TABLE constants.PayTypes (
    Id int IDENTITY(1,1) NOT NULL,
    Deffination nvarchar(256) NOT NULL,
    ShortDeffination nvarchar(256) NOT NULL,
    CONSTRAINT pk_PayTypes PRIMARY KEY ( Id )
);
GO

CREATE TABLE constants.Periods (
    Id int IDENTITY(1,1) NOT NULL,
    Deffination nvarchar(256) NOT NULL,
    CONSTRAINT pk_Periods PRIMARY KEY ( Id )
);
GO

CREATE TABLE finances.Personals (
    IdentityNumber bigint NOT NULL,
    Name nvarchar(64) NOT NULL,
    Surname nvarchar(64) NOT NULL,
    CONSTRAINT pk_Personals PRIMARY KEY ( IdentityNumber )
);

CREATE UNIQUE INDEX idx_Personals_Name ON finances.Personals (Name ASC);
GO

CREATE UNIQUE INDEX idx_Personals_Surname ON finances.Personals (Surname ASC);
GO

CREATE TABLE finances.PersonalPayTypes (
    PersonalId bigint NOT NULL
               CONSTRAINT fk_PersonalPayTypes_Personals_PersonalId
               REFERENCES finances.Personals,
    PayTypeId int NOT NULL
              CONSTRAINT fk_PersonalPayTypes_PayTypes_PayTypeId
              REFERENCES constants.PayTypes
    CONSTRAINT pk_PersonalPayTypes PRIMARY KEY ( PersonalId, PayTypeId )
);
GO

CREATE TABLE finances.PersonalPayRolls (
    Id int IDENTITY(1,1) NOT NULL,
    PersonalId bigint NOT NULL
               CONSTRAINT fk_PersonalPayRolls_Personals_PersonalId
               REFERENCES finances.Personals,
    PayTypeId int NOT NULL
              CONSTRAINT fk_PersonalPayRolls_PayTypes_PayTypeId
              REFERENCES constants.PayTypes,
    PeriodId  int NOT NULL
              CONSTRAINT fk_PersonalPayRolls_Periods_PeriodId
              REFERENCES constants.PayTypes,
    Quantity   int            DEFAULT 1 NOT NULL,
    Amount     decimal(18, 3) DEFAULT 0 NOT NULL,
    Pay        decimal(18, 3) DEFAULT 0 NOT NULL,
    CONSTRAINT pk_PersonalPayRolls PRIMARY KEY ( Id )
);
GO

-- Create Table Types
CREATE TYPE constants.PayTypeEntity AS TABLE
(
    Id               int          NOT NULL,
    Deffination      nvarchar(64) NOT NULL,
    ShortDeffination nvarchar(32) NOT NULL
);
GO

CREATE TYPE constants.PeriodEntity AS TABLE
(
    Id          int          NOT NULL,
    Deffination nvarchar(64) NOT NULL
);
GO

CREATE TYPE finances.PersonalEntity AS TABLE
(
    IdentityNumber bigint       NOT NULL,
    Name           nvarchar(64) NOT NULL,
    Surname        nvarchar(64) NOT NULL
);
GO

CREATE TYPE finances.PersonalPayTypeEntity AS TABLE
(
    PersonalId bigint NOT NULL,
    PayTypeId  int    NOT NULL
);
GO

CREATE TYPE finances.PersonalPayRollEntity AS TABLE
(
    Id         int            NOT NULL,
    PersonalId bigint         NOT NULL,
    PayTypeId  int            NOT NULL,
    PeriodId   int            NOT NULL,
    Quantity   int            NOT NULL,
    Amount     decimal(18, 3) NOT NULL,
    Pay        decimal(18, 3) DEFAULT 0 NOT NULL
);
GO

-- Create Stored Procedures and Functions
CREATE PROCEDURE constants.GetPayTypes(
    @id INT = NULL
)
AS
BEGIN
    SELECT p.Id,
           p.Deffination,
           p.ShortDeffination
    FROM constants.PayTypes p WITH (NOLOCK)
    WHERE p.Id = IIF(@id IS NULL, p.Id, @id)
END;
GO

CREATE PROCEDURE constants.GetPeriods(
    @id INT = NULL
)
AS
BEGIN
    SELECT p.Id,
           p.Deffination
    FROM constants.Periods p WITH (NOLOCK)
    WHERE p.Id = IIF(@id IS NULL, p.Id, @id)
END;
GO

CREATE PROCEDURE constants.CreateOrUpdatePayTypes(
    @entity constants.PayTypeEntity READONLY
)
AS
BEGIN
    BEGIN TRANSACTION
        UPDATE pt
        SET pt.Deffination      = e.Deffination,
            pt.ShortDeffination = e.ShortDeffination
        FROM constants.PayTypes pt WITH ( NOLOCK )
        JOIN @entity e ON e.Id = pt.Id
        WHERE pt.Id = e.Id
    COMMIT TRANSACTION;

    BEGIN TRANSACTION
        INSERT INTO constants.PayTypes (Deffination, ShortDeffination)
        SELECT e.Deffination, e.ShortDeffination
        FROM @entity e
        LEFT OUTER JOIN constants.PayTypes pt WITH ( NOLOCK ) ON pt.Id = e.Id
        WHERE pt.Id IS NULL
    COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE constants.CreateOrUpdatePeriods(
    @entity constants.PeriodEntity READONLY
)
AS
BEGIN
    BEGIN TRANSACTION
        UPDATE p
        SET p.Deffination      = e.Deffination
        FROM constants.Periods p WITH ( NOLOCK )
        JOIN @entity e ON e.Id = p.Id
        WHERE p.Id = e.Id
    COMMIT TRANSACTION;

    BEGIN TRANSACTION
        INSERT INTO constants.Periods (Deffination)
        SELECT e.Deffination
        FROM @entity e
        LEFT OUTER JOIN constants.Periods p WITH ( NOLOCK ) ON p.Id = e.Id
        WHERE p.Id IS NULL
    COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE finances.CreateOrUpdatePersonals(
    @entity finances.PersonalEntity READONLY
)
AS
BEGIN
    BEGIN TRANSACTION
        UPDATE p
        SET p.Name      = e.Name,
            p.Surname   = e.Surname
        FROM finances.Personals p WITH ( NOLOCK )
        JOIN @entity e ON e.IdentityNumber = p.IdentityNumber
        WHERE p.IdentityNumber = e.IdentityNumber
    COMMIT TRANSACTION;

    BEGIN TRANSACTION
        INSERT INTO finances.Personals (IdentityNumber, Name, Surname)
        SELECT e.IdentityNumber, e.Name, e.Surname
        FROM @entity e
        LEFT OUTER JOIN finances.Personals p WITH ( NOLOCK ) ON p.IdentityNumber = e.IdentityNumber
        WHERE p.IdentityNumber IS NULL
    COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE finances.CreatePersonalPayTypes(
    @entity finances.PersonalPayTypeEntity READONLY
)
AS
BEGIN
    BEGIN TRANSACTION
        INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId)
        SELECT e.PersonalId, e.PayTypeId
        FROM @entity e
        LEFT OUTER JOIN finances.PersonalPayTypes ppt WITH ( NOLOCK ) ON ppt.PersonalId = e.PersonalId AND ppt.PayTypeId = e.PayTypeId
        WHERE ppt.PersonalId IS NULL AND ppt.PayTypeId IS NULL
    COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE finances.CreateOrUpdatePersonalPayRolls(
    @entity finances.PersonalPayRollEntity READONLY
)
AS
BEGIN
    BEGIN TRANSACTION
        UPDATE pp
        SET pp.Quantity = e.Quantity,
            pp.Amount   = e.Amount,
            pp.Pay      = IIF(e.Pay = 0, (e.Quantity * e.Amount), e.Pay)
        FROM finances.PersonalPayrolls pp WITH ( NOLOCK )
        JOIN @entity e ON e.Id = pp.Id
        WHERE pp.Id = e.Id
    COMMIT TRANSACTION;

    BEGIN TRANSACTION
        INSERT INTO finances.PersonalPayrolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay)
        SELECT e.PersonalId,
               e.PayTypeId,
               e.PeriodId,
               e.Quantity,
               e.Amount,
               IIF(e.Pay = 0, (e.Quantity * e.Amount), e.Pay)
        FROM @entity e
        LEFT OUTER JOIN finances.PersonalPayrolls pp WITH ( NOLOCK ) ON pp.Id = e.Id
        WHERE pp.Id IS NULL
    COMMIT TRANSACTION;
END;
GO

CREATE PROCEDURE finances.GetPersonals(
    @identityNumber BIGINT = NULL
)
AS
BEGIN
    SELECT p.IdentityNumber,
           p.Name,
           p.Surname
    FROM finances.Personals p WITH (NOLOCK)
    WHERE p.IdentityNumber = IIF(@identityNumber IS NULL, p.IdentityNumber, @identityNumber)
END;
GO

CREATE PROCEDURE finances.GetPersonalPayTypes(
    @identityNumber BIGINT = NULL,
    @payTypeId INT = NULL
)
    AS
BEGIN
SELECT ppt.PersonalId,
       ppt.PayTypeId,
       CAST(p.Name AS VARCHAR(64)) +
       ' ' +
       CAST(p.Surname AS VARCHAR(64)) Personal,
       pt.Deffination PayType
FROM finances.PersonalPayTypes ppt WITH (NOLOCK)
    JOIN finances.Personals p ON p.IdentityNumber = ppt.PersonalId
    JOIN constants.PayTypes pt ON pt.Id = ppt.PayTypeId
WHERE ppt.PersonalId = IIF(@identityNumber IS NULL, ppt.PersonalId, @identityNumber)
  AND ppt.PayTypeId = IIF(@payTypeId IS NULL, ppt.PayTypeId, @payTypeId)
END;
GO

CREATE PROCEDURE finances.GetPersonalPayRolls(
    @identityNumber BIGINT = NULL,
    @payTypeId INT = NULL,
    @period INT = NULL
)
    AS
BEGIN
SELECT ppr.Id,
       ppr.PersonalId,
       ppr.PayTypeId,
       ppr.PeriodId,
       ppr.Quantity,
       ppr.Amount,
       ppr.Pay
FROM finances.PersonalPayRolls ppr WITH (NOLOCK)
WHERE ppr.PersonalId = IIF(@identityNumber IS NULL, ppr.PersonalId, @identityNumber)
  AND ppr.PayTypeId = IIF(@payTypeId IS NULL, ppr.PayTypeId, @payTypeId)
  AND ppr.PeriodId = IIF(@period IS NULL, ppr.PeriodId, @period)
END;
GO

CREATE PROCEDURE finances.GetPersonalPayrollSummaries(
    @identityNumber BIGINT = NULL,
    @period INT = NULL
)
AS
BEGIN
    SELECT T.FullName, T.PersonalId, T.Period, T.PeriodId, SUM(T.Pay) TotalPay
    From (
        SELECT CAST(per.Name AS VARCHAR(64)) +
               ' ' +
               CAST(per.Surname AS VARCHAR(64)) FullName,
               per.IdentityNumber PersonalId,
               p.Deffination                    Period,
               p.Id                             PeriodId,
               ppr.Pay
        FROM finances.PersonalPayrolls ppr WITH (NOLOCK)
        JOIN constants.Periods p ON p.Id = ppr.PeriodId
        JOIN finances.Personals per ON per.IdentityNumber = ppr.PersonalId
        WHERE ppr.PersonalId = IIF(@identityNumber IS NULL, ppr.PersonalId, @identityNumber)
        AND ppr.PeriodId = IIF(@period IS NULL, ppr.PeriodId, @period)
    ) T
    GROUP BY T.FullName, T.PersonalId, T.Period, T.PeriodId
    ORDER BY T.PeriodId, T.FullName ASC
END;
GO

CREATE FUNCTION finances.XmlPersonalPayRollPeriodPays(
    @identityNumber BIGINT,
    @period INT = NULL
)
RETURNS XML
AS
BEGIN
    DECLARE @xmlData XML;
    DECLARE @tempTable TABLE (PayId int, PayType text, Amount decimal(18,3), Quantity text, Pay decimal(18,3));
    INSERT INTO @tempTable (PayId, PayType, Amount, Quantity, Pay)
        SELECT ppr.Id PayId,
               pt.Deffination PayType,
               ppr.Amount,
               CAST(ppr.Quantity AS VARCHAR(5)) + ' ' + CAST(pt.ShortDeffination AS VARCHAR(32)) Quantity,
               ppr.Pay
        FROM finances.PersonalPayrolls ppr WITH (NOLOCK)
        JOIN finances.PersonalPayTypes ppt ON ppt.PersonalId = ppr.PersonalId AND ppt.PayTypeId = ppr.PayTypeId
        JOIN constants.PayTypes pt ON pt.Id = ppt.PayTypeId
        WHERE ppr.PersonalId = @identityNumber
        AND ppr.PeriodId = @period
    SET @xmlData = (SELECT(
    SELECT  t.PayId                 '@Id',
            t.PayType               '@PayType',
            t.Amount                '@Amount',
            t.Quantity              '@Quantity',
            t.Pay                   '@Pay'
    From @tempTable t
    FOR XML PATH ('Pay'), root ('Pays')) AS XmlData)
    RETURN @xmlData
END;
GO

CREATE FUNCTION finances.XmlPersonalPayRollPeriods(
    @identityNumber BIGINT
)
RETURNS XML
AS
BEGIN
    DECLARE @xmlData XML;
    DECLARE @tempTable TABLE (PeriodId int, PeriodName text, TotalPay decimal(18,3));
    INSERT INTO @tempTable (PeriodId, PeriodName, TotalPay)
        SELECT  p.Id,
                p.Deffination,
                SUM(pp.Pay) TotalPay
        From constants.Periods p WITH (NOLOCK)
        JOIN finances.PersonalPayrolls pp WITH ( NOLOCK ) ON pp.PeriodId = p.Id
        WHERE pp.PersonalId = @identityNumber
        GROUP BY p.Id, p.Deffination
        ORDER BY p.Id ASC
    SET @xmlData = (SELECT(
    SELECT  t.PeriodId              '@Id',
            t.PeriodName            '@Name',
            t.TotalPay              '@Pay',
            finances.XmlPersonalPayRollPeriodPays(@identityNumber, t.PeriodId) '*'
    From @tempTable t
    FOR XML PATH ('Period'), root ('Periods')) AS XmlData)
    RETURN @xmlData
END;
GO

CREATE PROCEDURE finances.XmlPersonalPayRollsExpanded(
    @identityNumber BIGINT = NULL
)
AS
BEGIN
    DECLARE @tempTable TABLE (Personal text, IdentityNumber bigint, Pay decimal(18,3));
    INSERT INTO @tempTable (Personal, IdentityNumber, Pay)
        SELECT T.Personal, T.IdentityNumber, SUM(T.Pay) TotalPay
        From (
            SELECT CAST(per.Name AS VARCHAR(64)) +
                   ' ' +
                   CAST(per.Surname AS VARCHAR(64)) Personal,
                   per.IdentityNumber,
                   ppr.Pay
            FROM finances.PersonalPayrolls ppr WITH (NOLOCK)
            JOIN finances.Personals per ON per.IdentityNumber = ppr.PersonalId
            WHERE ppr.PersonalId = IIF(@identityNumber IS NULL, ppr.PersonalId, @identityNumber)
        ) T
        GROUP BY T.Personal, T.IdentityNumber
    SELECT  t.Personal              '@Personal',
            t.IdentityNumber        '@IdentityNumber',
            t.Pay                   '@TotalPay',
            finances.XmlPersonalPayRollPeriods(t.IdentityNumber) '*'
    From @tempTable t
    FOR XML PATH ('Payroll'), root ('Payrolls')
END;
GO


-- Create Dummy Data For PayTypes
INSERT INTO constants.PayTypes (Deffination, ShortDeffination) VALUES ('Sabit Ücret', 'Ay')
INSERT INTO constants.PayTypes (Deffination, ShortDeffination) VALUES ('Günlük Ücret', 'Gün')
INSERT INTO constants.PayTypes (Deffination, ShortDeffination) VALUES ('Fazla Mesai Ücreti', 'Saat')

-- Create Dummy Data For Periods
INSERT INTO constants.Periods (Deffination) VALUES ('Ocak')
INSERT INTO constants.Periods (Deffination) VALUES ('Şubat')
INSERT INTO constants.Periods (Deffination) VALUES ('Mart')
INSERT INTO constants.Periods (Deffination) VALUES ('Nisan')
INSERT INTO constants.Periods (Deffination) VALUES ('Mayıs')
INSERT INTO constants.Periods (Deffination) VALUES ('Haziran')
INSERT INTO constants.Periods (Deffination) VALUES ('Temmuz')
INSERT INTO constants.Periods (Deffination) VALUES ('Ağustos')
INSERT INTO constants.Periods (Deffination) VALUES ('Eylül')
INSERT INTO constants.Periods (Deffination) VALUES ('Ekim')
INSERT INTO constants.Periods (Deffination) VALUES ('Kasım')
INSERT INTO constants.Periods (Deffination) VALUES ('Aralık')

-- Create Dummy Data For Personals
INSERT INTO finances.Personals (IdentityNumber, Name, Surname) VALUES (20353897823, 'Cüneyt', 'Alkılıç')
INSERT INTO finances.Personals (IdentityNumber, Name, Surname) VALUES (58313975223, 'Efe', 'Bilmemne')
INSERT INTO finances.Personals (IdentityNumber, Name, Surname) VALUES (98737836858, 'Selvi', 'Günebakan')
INSERT INTO finances.Personals (IdentityNumber, Name, Surname) VALUES (75422489105, 'Ata', 'Yediveren')

-- Create Dummy Data For PersonalPayTypes
INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId) VALUES (20353897823, 1)
INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId) VALUES (20353897823, 3)
INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId) VALUES (58313975223, 2)
INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId) VALUES (98737836858, 2)
INSERT INTO finances.PersonalPayTypes (PersonalId, PayTypeId) VALUES (75422489105, 1)

-- Create Dummy Data For PersonalPayRoles
INSERT INTO finances.PersonalPayRolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay) VALUES (20353897823, 1, 1, 1, 10000, (1 * 10000))
INSERT INTO finances.PersonalPayRolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay) VALUES (20353897823, 3, 1, 14, 125.75, (14 * 125.75))
INSERT INTO finances.PersonalPayRolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay) VALUES (58313975223, 2, 1, 21, 200, (21 * 200))
INSERT INTO finances.PersonalPayRolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay) VALUES (98737836858, 2, 1, 18, 185, (18 * 185))
INSERT INTO finances.PersonalPayRolls (PersonalId, PayTypeId, PeriodId, Quantity, Amount, Pay) VALUES (75422489105, 1, 1, 1, 15000, (1 * 15000))


