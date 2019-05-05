alter table Redemptions
add [address_line1] [nvarchar](255),
	[address_line2] [nvarchar](255),
	[city] [nvarchar](255),
	[state] [nvarchar](255),
	[country] [nvarchar](255),
	[postcode] [varchar](10)

alter table Redemptions
drop column delivery_address

EXEC sp_rename 'Redemptions.bank_account', 'bank_name', 'COLUMN'
EXEC sp_rename 'Redemptions.bank_type', 'bank_account_num', 'COLUMN'
EXEC sp_rename 'Redemptions.identity_num', 'bank_account_name', 'COLUMN'

alter table Users
add [point_balance] [int]

alter table Reward
add [point_requirement] [int]

alter table Redemptions
add [reward_point_requirement] [int],
	[reward_name] [nvarchar](255)