CREATE TABLE [dbo].[FidelityUpload]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [RunDate] DATETIME2 NULL,
    [Action] NVARCHAR(200) NULL,
    [Symbol] NVARCHAR(50) NULL,
    [Description] NVARCHAR(200) NULL,
    [Type] NVARCHAR(50) NULL,
    [Quantity] INT NULL,
    [Price] DECIMAL(18,2) NULL,
    [Commission] DECIMAL(18,2) NULL,
    [Fees] DECIMAL(18,2) NULL,
    [AccruedInterest] DECIMAL(18,2) NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [CashBalance] DECIMAL(18,2) NULL,
    [SettlementDate] DATETIME2 NULL
)
