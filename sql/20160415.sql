alter table [CampaignHistory]
add last_updated datetime

CREATE TABLE UserTrx
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	user_id int,
	[desc] nvarchar(500),
	debit_amount int,
	credit_amount int,
	balance int,
	created_date datetime,
	created_by int,
	transaction_month int,
	transaction_year int,
	account_from nvarchar(100),
	account_to nvarchar(100),
);

ALTER TABLE UserTrx  WITH CHECK ADD  CONSTRAINT FK_UserTrx_Users FOREIGN KEY(user_id)
REFERENCES Users (id);

alter table [CampaignHistory]
add created_date datetime

alter table [CampaignHistory]
add remarks nvarchar(200)