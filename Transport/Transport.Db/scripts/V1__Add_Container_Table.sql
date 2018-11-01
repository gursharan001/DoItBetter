CREATE TABLE [dbo].[Container](
	[Id] [int] NOT NULL,
	ContainerNumber [nvarchar](255) NULL,
	ContainerType int NULL,
	Weight decimal(18,2) NULL,
	Volume decimal(18,2) NULL,
	NumberOfPackages int NULL,
	UnitOfMeasure int NULL,
	StorageDate datetime null,
	CONSTRAINT [PK_Container] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)