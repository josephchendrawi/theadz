alter table [CampaignRules]
add monday bit,
tuesday bit,
wednesday bit,
thursday bit,
friday bit,
saturday bit,
sunday bit,
agegroup_start int,
agegroup_end int,
image_id int

ALTER TABLE CampaignRules WITH CHECK ADD CONSTRAINT [FK_CampaignRules_Images] FOREIGN KEY([image_id])
REFERENCES Images ([id])