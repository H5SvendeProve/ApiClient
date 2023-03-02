
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'VaskeriDb')
BEGIN
  CREATE DATABASE VaskeriDb;
END;




CREATE TABLE ElectricityPrices (
    DKKPerKWh FLOAT NOT NULL,
    EURPerKWh FLOAT NOT NULL,
    Exr FLOAT NOT NULL,
    TimeStart DATETIME NOT NULL,
    TimeEnd DATETIME NOT NULL,
    Location nvarchar(50) NOT NULL
	constraint PK_ElectricityPrices PRIMARY KEY (Exr, TimeStart, TimeEnd, Location)
);



CREATE PROCEDURE InsertElectricityPrice
    @DKKPerKWh FLOAT,
    @EURPerKWh FLOAT,
    @Exr FLOAT,
    @TimeStart DATETIME,
    @TimeEnd DATETIME,
    @Location nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;

	if not exists (select * from ElectricityPrices where Exr = @Exr and TimeStart = @TimeStart and TimeEnd = @TimeEnd and Location = @Location)

	begin

    INSERT INTO ElectricityPrices (DKKPerKWh, EURPerKWh, Exr, TimeStart, TimeEnd, Location)
    VALUES (@DKKPerKWh, @EURPerKWh, @Exr, @TimeStart, @TimeEnd, @Location);

	end;
END;

create procedure PriceExistsInDb
    @TimeStart DATETIME,
    @TimeEnd DATETIME,
    @Location nvarchar(50)

as
    begin
    set nocount on;

    select 1 from ElectricityPrices where Location = @Location and convert(DATE, TimeStart) = convert(DATE, @TimeStart) and convert(DATE, TimeEnd) = convert(DATE, @TimeEnd)

    end
GO
