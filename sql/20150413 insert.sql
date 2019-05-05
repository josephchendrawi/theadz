insert into [RewardCriteria]([name])values('Food');
insert into [RewardCriteria]([name])values('Medical');
insert into [RewardCriteria]([name])values('Travel');
insert into [RewardCriteria]([name])values('Gadget');
insert into [RewardCriteria]([name])values('Coupon');

insert into [AdminRole]([name])values('Administrator');

insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,1,1,1,1,1);
insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,2,1,1,1,1);
insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,3,1,1,1,1);
insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,4,1,1,1,1);
insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,5,1,1,1,1);
insert into [AdminRoleAccess]([adminrole_id],[module_id],[is_viewable],[is_editable],[is_addable],[is_deleteable])values(1,6,1,1,1,1);

insert into [Admins]([first_name],[last_name],[email],[password],[password_salt],[last_action],[role_id])values('FName','LName','admin','f743a0139fe17a0b475d0448d83d11b2ef077dbb08b364cc982d4bc0116f1f0f','ffe807755b9aa54be1a7a4047b410e13477a2d72640a4ad9e8647e5d4c593a0d','1',1);
