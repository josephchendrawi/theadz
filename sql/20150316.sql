alter table [Campaign]
add LINK_URL nvarchar(255)

CREATE TABLE Rules(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	longitude [float] ,
	latitude [float] ,
	gender int,
	time datetime,
	numberofviews int,
    campaign_id int
);
ALTER TABLE Rules WITH CHECK ADD  CONSTRAINT [FK_Rules_Campaign] FOREIGN KEY([campaign_id])
REFERENCES Campaign ([id])

