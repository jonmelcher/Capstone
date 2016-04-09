use jmelcher1
go

-- if tables exist, drop them...

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'VehicleInfo'
)
drop table VehicleInfo
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'Vehicles'
)
drop table Vehicles
go

-- now create tables...

CREATE TABLE Vehicles
(
	VehicleID char(10) NOT NULL PRIMARY KEY,

	Stored bit NOT NULL,
	Cell tinyint NOT NULL,
)

CREATE TABLE VehicleInfo
(
	VehicleID char(10) NOT NULL FOREIGN KEY REFERENCES Vehicles,
	
	Mileage int NOT NULL DEFAULT (0),
	ModelYear datetime,
	Make varchar(64),
	Model varchar(64),
	Colour varchar(32),
	Notes varchar(255)
)

GO

-- if the procedures exist, drop them...

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'InsertVehicle'
)
drop procedure InsertVehicle
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'GetVehicleRecord'
)
drop procedure GetVehicleRecord
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'GetVehicleInfoRecord'
)
drop procedure GetVehicleInfoRecord
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'UpdateVehicleInfoRecord'
)
drop procedure UpdateVehicleInfoRecord
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'AddRandomMileage'
)
drop procedure AddRandomMileage
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'MoveVehicle'
)
drop procedure MoveVehicle
go

if exists
(
	select	[name]
	from	jmelcher1.dbo.sysobjects
	where	[name] = 'GetPopulation'
)
drop procedure GetPopulation
go


-- now create procedures

CREATE PROCEDURE GetPopulation
AS
	SELECT COUNT(VehicleID) as 'Population'
	FROM Vehicles
	WHERE Stored = 1
GO

CREATE PROCEDURE InsertVehicle
	@id char(10),
	@stored bit,
	@cell tinyint,
	
	@mileage int = 0,
	@modelyear datetime = null,
	@make varchar(64) = null,
	@model varchar(64) = null,
	@colour varchar(32) = null,
	@notes varchar(255) = null
AS
	IF NOT EXISTS -- if the vehicle is a new vehicle
	(
		SELECT VehicleID
		FROM Vehicles
		WHERE VehicleID = @id
	)
	BEGIN
 		INSERT INTO Vehicles
 		VALUES (@id, @stored, @cell)
		INSERT INTO VehicleInfo
		VALUES	(@id, @mileage, @modelyear, @make, @model, @colour, @notes)
	END
GO

CREATE PROCEDURE GetVehicleRecord
	@id char(10)
AS
	SELECT
		VehicleID,
		Stored,
		Cell
	FROM
		Vehicles
	WHERE
		VehicleID = @id
GO

CREATE PROCEDURE GetVehicleInfoRecord
	@id char(10)
AS
	SELECT
		Mileage,
		ModelYear,
		Make,
		Model,
		Colour,
		Notes
	FROM
		VehicleInfo
	WHERE
		VehicleID = @id
GO

CREATE PROCEDURE UpdateVehicleInfoRecord
	@id char(10),
	@mileage int = 0,
	@colour varchar(32) = null,
	@notes varchar(255) = null
AS
	UPDATE VehiclesInfo
	SET
		Mileage = Mileage + @mileage,
		Colour = 
			CASE WHEN @colour IS NOT NULL
				THEN @colour
				ELSE Colour
			END,
		Notes =
			CASE WHEN @notes IS NOT NULL
				THEN @notes
				ELSE Notes
			END
	WHERE
		VehicleID = @id
GO

CREATE PROCEDURE AddRandomMileage
	@id char(10),
	@max int,
	@min int
AS
	if (@max < @min) -- if min is greater than max
		BEGIN
			EXECUTE AddRandomMileage @id, @min, @max  -- swap the values
			RETURN
		END

	DECLARE
		@rng int = ABS(CHECKSUM(NewId())) % (@max - @min) + @min  -- get a random mileage

	EXECUTE UpdateVehicleInfoRecord @id, @rng  -- and update the record
GO

CREATE PROCEDURE MoveVehicle
	@id char(10),
	@isGoingIn bit
AS
	UPDATE Vehicles
		SET
			Stored = @isGoingIn
		WHERE
			VehicleID = @id
GO

-- populate vehicles table

EXECUTE InsertVehicle '88002A0637', 0, 1
EXECUTE InsertVehicle '530020D5D5', 0, 2
EXECUTE InsertVehicle '88002A607D', 0, 3
EXECUTE InsertVehicle '88002C3EEE', 0, 4
EXECUTE InsertVehicle '8800299F6C', 0, 5
EXECUTE InsertVehicle '530020CDF4', 0, 6
EXECUTE InsertVehicle '8800298698', 0, 7

EXECUTE InsertVehicle '88002A5318', 0, 8
EXECUTE InsertVehicle '88002E3F2E', 0, 9
EXECUTE InsertVehicle '39009D3D68', 0, 10
EXECUTE InsertVehicle '88002A014F', 0, 11
EXECUTE InsertVehicle '39009D6EBF', 0, 12
EXECUTE InsertVehicle '88002AF110', 0, 13
EXECUTE InsertVehicle '530020737E', 0, 14
EXECUTE InsertVehicle '39009D6E90', 0, 15

EXECUTE InsertVehicle '88002C44F1', 0, 16
EXECUTE InsertVehicle '53001FDA0B', 0, 17
EXECUTE InsertVehicle '53001FD9FD', 0, 18
EXECUTE InsertVehicle '88002A06DC', 0, 19
EXECUTE InsertVehicle '39009D6FB0', 0, 20
EXECUTE InsertVehicle '88002AE935', 0, 21
EXECUTE InsertVehicle '5300207370', 0, 22
EXECUTE InsertVehicle '8800298192', 0, 23

GO
