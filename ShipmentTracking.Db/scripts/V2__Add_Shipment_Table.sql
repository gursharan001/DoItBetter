CREATE TABLE [dbo].[Shipment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	Origin [nvarchar](255) NULL,
	Etd DateTime NULL,
	Destination nvarchar(255) NULL,
	Eta datetime NULL,
	ContainerId int NULL,
	ShipmentStatus int NULL,
	CONSTRAINT [PK_Shipment] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)