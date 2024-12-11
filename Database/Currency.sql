CREATE TABLE [dbo].[Currency]
(
	[Type]				VARCHAR(3) NOT NULL,
	[Name]				NVARCHAR(10) NULL,
	[LastUpdateDate]	DATETIMEOFFSET NULL, 
    CONSTRAINT [PK_Currency] PRIMARY KEY ([Type] ASC)
)
