-- 第一次使用時，請直接執行此指令建立所有初始資料

PRINT N'建立資料庫 [Coindesk]...'
CREATE DATABASE [Coindesk]
GO

USE [Coindesk];
GO

PRINT N'建立資料表 [Currency]...'
CREATE TABLE [dbo].[Currency]
(
	[Type]				VARCHAR(3) NOT NULL,
	[Name]				NVARCHAR(10) NULL,
	[LastUpdateDate]	DATETIMEOFFSET NULL, 
    CONSTRAINT [PK_Currency] PRIMARY KEY ([Type] ASC)
)
GO

PRINT N'建立資料...'
INSERT INTO [dbo].[Currency] ([Type] ,[Name] ,[LastUpdateDate]) VALUES ('USD' ,N'美金' ,SYSDATETIMEOFFSET())
INSERT INTO [dbo].[Currency] ([Type] ,[Name] ,[LastUpdateDate]) VALUES ('EUR' ,N'歐元' ,SYSDATETIMEOFFSET())
INSERT INTO [dbo].[Currency] ([Type] ,[Name] ,[LastUpdateDate]) VALUES ('GBP' ,N'英鎊' ,SYSDATETIMEOFFSET())
INSERT INTO [dbo].[Currency] ([Type] ,[Name] ,[LastUpdateDate]) VALUES ('JPY' ,N'日幣' ,SYSDATETIMEOFFSET())
GO

PRINT N'執行完成'