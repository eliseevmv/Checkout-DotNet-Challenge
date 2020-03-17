﻿CREATE TABLE [dbo].Payments
(
	[PaymentIdentifier] NVARCHAR(100) NOT NULL PRIMARY KEY,
	[Amount] DECIMAL(19,4) NOT NULL,
	[Currency] VARCHAR(3) NOT NULL,
	[MaskedCardNumber] VARCHAR(16) NOT NULL,
	[ExpiryMonthAndDate] VARCHAR(4) NOT NULL,
	[Cvv] VARCHAR(3) NOT NULL,
	[Created] DATETIME NOT NULL,
	[MerchantId] UNIQUEIDENTIFIER NOT NULL

)
GO

-- CREATE CLUSTERED INDEX IX_Payments_PaymentIdentifier ON dbo.Payments(PaymentIdentifier)