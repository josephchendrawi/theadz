CREATE TABLE CampaignImages(
campaign_id int,
image_id int,
image_url nvarchar(255),
CONSTRAINT PK_kzBVDImages PRIMARY KEY (campaign_id,image_id)
);
ALTER TABLE CampaignImages  WITH CHECK ADD  CONSTRAINT FK_CampaignImages_Campaign FOREIGN KEY(campaign_id)
REFERENCES Campaign (id);