alter table CampaignHistory
add longitude [float] ,
	latitude [float] ,
	gender int
	
EXEC sp_rename 'Rules', 'CampaignRules'