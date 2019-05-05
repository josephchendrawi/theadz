EXEC sp_rename 'Prize', 'Reward'
EXEC sp_rename 'PrizeImages', 'RewardImages'

alter table Reward
add criteria_id int

CREATE TABLE RewardCriteria(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255)
);

ALTER TABLE Reward WITH CHECK ADD  CONSTRAINT [FK_Reward_RewardCriteria] FOREIGN KEY([criteria_id])
REFERENCES RewardCriteria ([id])

EXEC sp_RENAME 'RewardImages.prize_id', 'reward_id', 'COLUMN'
ALTER TABLE RewardImages WITH CHECK ADD  CONSTRAINT [FK_RewardImages_Reward] FOREIGN KEY([reward_id])
REFERENCES Reward ([id])