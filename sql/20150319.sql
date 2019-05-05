CREATE TABLE APIKeys(
	unique_id nvarchar(255) NOT NULL,
	token nvarchar(255),
	token_salt nvarchar(255),
	token_expire datetime,
	os nvarchar(255),
	os_version  nvarchar(255),
	model nvarchar(255),
	user_id int NOT NULL,
	last_created datetime,
	PRIMARY KEY (unique_id, user_id)
);
ALTER TABLE APIKeys WITH CHECK ADD  CONSTRAINT [FK_APIKeys_Users] FOREIGN KEY([user_id])
REFERENCES Users ([id])

alter table Users 
drop column [token],
[token_salt],
[token_expire]