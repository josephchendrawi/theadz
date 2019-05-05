CREATE TABLE RewardType
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255),
	delivery bit,
	money_transfer bit,
	mobile bit
);

alter table Reward
add reward_type_id int

ALTER TABLE Reward WITH CHECK ADD CONSTRAINT [FK_Reward_RewardType] FOREIGN KEY([reward_type_id])
REFERENCES RewardType ([id])

CREATE TABLE MobileOperator
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255)
);

CREATE TABLE RedemptionStatus
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255)
);

CREATE TABLE Redemptions
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	user_id int,
	reward_id int,
	reward_type_id int,
	name nvarchar(255),
	delivery_address nvarchar(255),
	bank_account nvarchar(255),
	bank_type nvarchar(255),
	identity_num nvarchar(255),
	mobile_operator_id int,
	mobile_acc_num nvarchar(255),
	redemption_status_id int,
	redemption_date datetime
);
ALTER TABLE Redemptions WITH CHECK ADD CONSTRAINT [FK_Redemptions_Users] FOREIGN KEY([user_id])
REFERENCES Users ([id])
ALTER TABLE Redemptions WITH CHECK ADD CONSTRAINT [FK_Redemptions_Reward] FOREIGN KEY([reward_id])
REFERENCES Reward ([id])
ALTER TABLE Redemptions WITH CHECK ADD CONSTRAINT [FK_Redemptions_MobileOperator] FOREIGN KEY([mobile_operator_id])
REFERENCES MobileOperator ([id])
ALTER TABLE Redemptions WITH CHECK ADD CONSTRAINT [FK_Redemptions_RedemptionStatus] FOREIGN KEY([redemption_status_id])
REFERENCES RedemptionStatus ([id])

CREATE TABLE Review
(
	redemption_id int,
	review_date datetime,
	rating int,
	message nvarchar(255),
	last_action varchar(1)
	CONSTRAINT PK_Review PRIMARY KEY (redemption_id)
);
ALTER TABLE Review WITH CHECK ADD CONSTRAINT [FK_Review_Redemptions] FOREIGN KEY([redemption_id])
REFERENCES Redemptions ([id])