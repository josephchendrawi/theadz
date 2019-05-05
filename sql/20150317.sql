alter table Rules
add last_created datetime,
last_updated datetime,
last_action varchar(1)

alter table Rules 
drop column [time]

alter table Rules
add start_time datetime,
end_time datetime