--DROP TABLE PushNotificationLog

CREATE TABLE PushNotificationLog(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	content nvarchar(max),
	creationdate datetime,
	status int,
	remarks nvarchar(max)
);