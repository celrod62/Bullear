CREATE TABLE [dbo].[Trades]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [TradeDateTime] DATETIME2 NULL,
    [Symbol] NVARCHAR(30) NULL,
    [Action] NVARCHAR(5) NULL,
    [Quantity] INT NULL,
    [Price] DECIMAL(18,2) NULL,
    [Spread] NVARCHAR(50) NULL,
    [Expiration] DATETIME2 NULL,
    [StrikePrice] DECIMAL(18,2) NULL,
    [OptionType] NVARCHAR(50) NULL,
    [Commission] DECIMAL(18,2) NULL,
    [Fee] DECIMAL(18,2)  NULL,
    [PortfolioId] NVARCHAR(50) NULL
)