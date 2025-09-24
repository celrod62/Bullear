CREATE TABLE [dbo].[Positions]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Status] NVARCHAR(50) NULL,
    [Spread] NVARCHAR(50) NULL,
    [Return] DECIMAL(18,2) NULL,
    [AvgEntryPrice] DECIMAL(18,2) NULL,
    [AvgExitPrice] DECIMAL(18,2) NULL,
    [ContractSize] INT NOT NULL,
    [PortfolioId] NVARCHAR(50) NULL,
    [OpenDateTime] DATETIME2 NOT NULL,
    [CloseDateTime] DATETIME2 NULL,
)
 