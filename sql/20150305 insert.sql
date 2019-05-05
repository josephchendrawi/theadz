insert into Status(name,last_action)values('Premium','1');
insert into Status(name,last_action)values('Pro','1');
insert into Status(name,last_action)values('Inactive','1');

insert into Currencies(name,code,last_action)values('Ringgit','RM','1');
insert into Currencies(name,code,last_action)values('Dollar','$','1');

insert into Countries(name,last_action)values('Malaysia','1');

insert into Cities(name,country_id,currency_id,last_action)values('Cheras',1,1,'1');
insert into Cities(name,country_id,currency_id,last_action)values('Puchong',1,1,'1');
insert into Cities(name,country_id,currency_id,last_action)values('Damansara',1,1,'1');