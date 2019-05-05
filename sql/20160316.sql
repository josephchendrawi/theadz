CREATE TABLE APILogs
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	unique_id nvarchar(255),
	api_key nvarchar(255),
	user_id int,
	date_created datetime,
	request_header ntext,
	request ntext,
	response ntext,
	url nvarchar(255),
	ip_address nvarchar(255)
);