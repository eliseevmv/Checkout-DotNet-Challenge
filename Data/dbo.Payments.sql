
-- PaymentId is GUID and is a primary key. Clustered index is on a different column (int identity) in order to avoid performance issues 
-- https://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega/11938495

CREATE TABLE [dbo].[Payments] (
    [PaymentId] UNIQUEIDENTIFIER NOT NULL,        
    [AcquringBankPaymentId]  NVARCHAR (100) NULL,
    [StatusCode]		 INT NOT NULL,        
	[Amount]             DECIMAL (19, 4)  NOT NULL,
    [Currency]           VARCHAR (3)      NOT NULL,
    [MaskedCardNumber]   VARCHAR (16)     NOT NULL,
    [ExpiryMonthAndDate] VARCHAR (4)      NOT NULL,
    [Cvv]                VARCHAR (3)      NOT NULL,
    [MerchantId]         UNIQUEIDENTIFIER NOT NULL,
	
    [CreatedDate] DATETIME CONSTRAINT [DF_Payments_CreatedDate] DEFAULT (getutcdate()) NOT NULL,      
    [ClusterID] INT NOT NULL IDENTITY,
    CONSTRAINT [PK_Payments] PRIMARY KEY NonCLUSTERED ([PaymentID] ASC)
);
GO

CREATE UNIQUE CLUSTERED INDEX [IX_Payments_ClusterID] ON [dbo].[Payments] ([ClusterID])
GO
