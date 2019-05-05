alter table [AdminRoleAccess] 
drop column [is_accessable]

alter table [AdminRoleAccess]
add is_viewable bit,
is_editable bit,
is_addable bit,
is_deleteable bit
