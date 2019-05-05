CREATE TABLE Branch(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	merchant_id int,
	longitude [float] ,
	latitude [float]
);

ALTER TABLE Branch  WITH CHECK ADD  CONSTRAINT [FK_Branch_Merchants] FOREIGN KEY([merchant_id])
REFERENCES Merchants ([id])

alter table Branch
add address_line1 nvarchar(255),
address_line2 nvarchar(255),
city_id int,
postcode varchar(10),
last_created datetime,
last_updated datetime,
last_action varchar(1)

ALTER TABLE Branch  WITH CHECK ADD  CONSTRAINT FK_Branch_Cities FOREIGN KEY(city_id)
REFERENCES Cities (id);