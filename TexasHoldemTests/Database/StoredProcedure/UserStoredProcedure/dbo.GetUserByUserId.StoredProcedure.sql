USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[GetUserByUserId]    Script Date: 6/1/2017 9:46:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByUserId] @userId int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM UserTable
WHERE userId = @userId
END

GO
