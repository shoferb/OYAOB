USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserById]    Script Date: 04/06/2017 16:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteUserById]
       @UserId  int 
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.UserTable
      FROM   dbo.UserTable
      WHERE  
      userId = @UserId                  

END



GO
