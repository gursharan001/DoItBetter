CREATE TABLE [dbo].[TransportJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	RequestedDeliveryAddress [nvarchar](255) NULL,
	ContainerId int NULL,
	RequestedDeliveryDate datetime null,
	DeliveredTimestamp datetime null,
	CONSTRAINT [PK_TransportJob] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)