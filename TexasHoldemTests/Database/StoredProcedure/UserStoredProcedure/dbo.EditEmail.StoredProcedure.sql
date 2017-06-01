USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[EditEmail]    Script Date: 6/1/2017 9:46:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditEmail]
       @UserId  int ,
       @newEmail varchar(50)
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             email   = @newEmail
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END

GO
