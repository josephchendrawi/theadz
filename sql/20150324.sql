CREATE TABLE CampaignHistory(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	campaign_id int,
	view_date datetime,
	user_id int
);
ALTER TABLE CampaignHistory WITH CHECK ADD CONSTRAINT [FK_CampaignHistory_Campaign] FOREIGN KEY([campaign_id])
REFERENCES Campaign ([id])
ALTER TABLE CampaignHistory WITH CHECK ADD CONSTRAINT [FK_CampaignHistory_Users] FOREIGN KEY([user_id])
REFERENCES Users ([id])