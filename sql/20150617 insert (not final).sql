insert into [MobileOperator]([name])values('Maxis');
insert into [MobileOperator]([name])values('Celcom');
insert into [MobileOperator]([name])values('DiGi');

insert into [RedemptionStatus]([name])values('Submitted');
insert into [RedemptionStatus]([name])values('Processing');
insert into [RedemptionStatus]([name])values('Completed');

insert into [RewardType]([name],[delivery],[money_transfer],[mobile])values('Physical Item','True','False','False');
insert into [RewardType]([name],[delivery],[money_transfer],[mobile])values('Cash','False','True','False');
insert into [RewardType]([name],[delivery],[money_transfer],[mobile])values('Mobile_Credit','False','False','True');
insert into [RewardType]([name],[delivery],[money_transfer],[mobile])values('Item + Cash','True','True','False');