--DROP TABLE PromotionTrx

CREATE TABLE PromotionTrx(
	promo_id int,
	user_id int,
	last_created datetime,
	remarks nvarchar(max),
	
	primary key (promo_id, user_id)	
);

ALTER TABLE PromotionTrx  WITH CHECK ADD  CONSTRAINT FK_PromotionTrx_Promotion FOREIGN KEY(promo_id)
REFERENCES Promotion (id);

ALTER TABLE PromotionTrx  WITH CHECK ADD  CONSTRAINT FK_PromotionTrx_Users FOREIGN KEY(user_id)
REFERENCES Users (id);


----

Alter table Promotion
add description nvarchar(max) 