# Demo code for "Evolving a Monolith with a Microservice"

1. To restore nuget packages, in root run .paket\paket.exe restore
2. Create following empty databases on localhost -
* ShipmentTracking - also add schema nsb (owned by dbo)
* ShipmentTracking.Tests
* Transport
* Transport.Tests
3. Database migrations are using Evolve in .Db projects. So build solution and then 
* ShipmentTrack.db\bin\debug\ShipmentTracking.db.exe --d ShipmentTracking
* ShipmentTrack.db\bin\debug\ShipmentTracking.db.exe --d ShipmentTracking.Tests
* Transport\Transport.db\bin\debug\Transport.db.exe --d Transport
* Transport\Transport.db\bin\debug\Transport.db.exe --d Transport.Tests
