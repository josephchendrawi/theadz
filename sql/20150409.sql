alter table [AdminRole] 
drop column [readable],
column [writeable]

CREATE TABLE AdminModule(
	id  int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	name nvarchar(255)
);

CREATE TABLE AdminRoleAccess(
	adminrole_id int,
	module_id int,
	is_accessable bit,
	CONSTRAINT PK_AdminRoleAccess PRIMARY KEY (adminrole_id,module_id)
);
ALTER TABLE AdminRoleAccess WITH CHECK ADD  CONSTRAINT [FK_AdminRoleAccess_AdminRole] FOREIGN KEY([adminrole_id])
REFERENCES AdminRole ([id])
ALTER TABLE AdminRoleAccess WITH CHECK ADD  CONSTRAINT [FK_AdminRoleAccess_AdminModule] FOREIGN KEY([module_id])
REFERENCES AdminModule ([id])