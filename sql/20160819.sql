Alter table Reward
add flag_one_time bit

Alter table Reward
add number_of_stock int

alter table Promotion
drop column valid_date

alter table Promotion
add start_datetime datetime,
	end_datetime datetime,
	flag_on_schedule bit