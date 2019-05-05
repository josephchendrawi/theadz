alter table Users
add referral_code varchar(10),
	referred_by varchar(10)
	
ALTER TABLE Users WITH CHECK ADD CONSTRAINT [UQ_Users_referral_code] UNIQUE(referral_code)

/*if there is data in Users table*/
/*
DECLARE @i int = 0
WHILE @i < 10 BEGIN
    SET @i = @i + 1
    UPDATE Users
    SET referral_code = ('AA00' + CAST(@i AS VARCHAR))
    WHERE id = @i
END

DECLARE @i int = 9
WHILE @i < 99 BEGIN
    SET @i = @i + 1
    UPDATE Users
    SET referral_code = ('AA0' + CAST(@i AS VARCHAR))
    WHERE id = @i
END
*/