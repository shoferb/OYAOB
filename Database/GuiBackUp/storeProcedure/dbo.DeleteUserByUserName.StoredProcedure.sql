USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserByUserName]    Script Date: 04/06/2017 16:17:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteUserByUserName]
       @UserName  varchar(50)
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.UserTable
      FROM   dbo.UserTable
      WHERE  
      username = @UserName                 

END



GO
