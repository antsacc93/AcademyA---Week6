CREATE DATABASE Ticketing

CREATE TABLE Ticket(
	ID INT IDENTITY(1,1) primary key,
	Description VARCHAR(500),
	Insert_date DATETIME,
	Username VARCHAR(100),
	Status VARCHAR(10),
	--constraint check_dates check (Insert_date >= GETDATE()),
	constraint status_value CHECK (status in ('NEW', 'ONGOING', 'RESOLVED'))
)



