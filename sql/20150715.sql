CREATE TABLE MobileLogs
(
	id int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	unique_id nvarchar(255),
	user_id int,
	date_created datetime,
	error_msg ntext,
	url nvarchar(255),
	activity nvarchar(255),
	ip_address nvarchar(255)
);

ALTER TABLE APIKeys
ADD flag_debug int

UPDATE APIKeys
SET flag_debug = 0

AlTER TABLE MobileLogs
DROP COLUMN error_msg

ALTER TABLE MobileLogs
ADD request ntext,
	response ntext