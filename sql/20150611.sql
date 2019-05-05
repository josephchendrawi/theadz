CREATE TABLE Tag(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255),
	last_action varchar(1),
	last_created datetime,
	last_updated datetime,
);

CREATE TABLE CampaignPrimaryTag(
	campaign_id int,
	tag_id int,
	last_action varchar(1),
	CONSTRAINT PK_CampaignPrimaryTag PRIMARY KEY (campaign_id,tag_id)
);
ALTER TABLE CampaignPrimaryTag WITH CHECK ADD  CONSTRAINT [FK_CampaignPrimaryTag_Campaign] FOREIGN KEY([campaign_id])
REFERENCES Campaign ([id])
ALTER TABLE CampaignPrimaryTag WITH CHECK ADD  CONSTRAINT [FK_CampaignPrimaryTag_Tag] FOREIGN KEY([tag_id])
REFERENCES Tag ([id])

CREATE TABLE CampaignSecondaryTag(
	campaign_id int,
	tag_id int,
	last_action varchar(1),
	CONSTRAINT PK_CampaignSecondaryTag PRIMARY KEY (campaign_id,tag_id)
);
ALTER TABLE CampaignSecondaryTag WITH CHECK ADD  CONSTRAINT [FK_CampaignSecondaryTag_Campaign] FOREIGN KEY([campaign_id])
REFERENCES Campaign ([id])
ALTER TABLE CampaignSecondaryTag WITH CHECK ADD  CONSTRAINT [FK_CampaignSecondaryTag_Tag] FOREIGN KEY([tag_id])
REFERENCES Tag ([id])

alter table Campaign 
drop column [reference_code]

DROP TABLE Tags