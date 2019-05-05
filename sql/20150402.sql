CREATE TABLE AdminRole(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255),
	readable bit,
	writeable bit
);

alter table Admins
add role_id int

ALTER TABLE Admins WITH CHECK ADD  CONSTRAINT [FK_Admins_AdminRole] FOREIGN KEY([role_id])
REFERENCES AdminRole ([id])