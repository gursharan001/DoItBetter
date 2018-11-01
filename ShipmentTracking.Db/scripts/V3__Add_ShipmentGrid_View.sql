create view vw_ShipmentGrid as
select 
	s.Id,
	Origin,
	Etd,
	Destination,
	Eta,
	ContainerNumber,
	ShipmentStatus
From Shipment s
inner join Container c on s.ContainerId = c.id