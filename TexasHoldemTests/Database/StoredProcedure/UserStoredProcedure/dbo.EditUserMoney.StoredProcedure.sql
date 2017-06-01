USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[EditUserMoney]    Script Date: 6/1/2017 9:46:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserMoney]
       @UserId  int ,
       @newMoney int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             money   = @newMoney
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END

GO
