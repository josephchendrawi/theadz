CREATE TABLE Campaign(
id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
merchant_id int,
name varchar(255),
reference_code varchar(255),
start_date datetime,
end_date datetime,
description ntext,
last_created datetime,
last_updated datetime,
last_action varchar(1)
);
ALTER TABLE Campaign  WITH CHECK ADD  CONSTRAINT FK_Campaign_Merchants FOREIGN KEY(merchant_id)
REFERENCES Merchants (id);