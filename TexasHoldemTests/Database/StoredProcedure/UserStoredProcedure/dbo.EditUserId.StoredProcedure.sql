USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[EditUserId]    Script Date: 6/1/2017 9:46:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserId]
       @NewUserId  int ,
       @oldUserId int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             userId   = @NewUserId
      FROM   dbo.UserTable
      WHERE  
      userId    = @oldUserId                  

END

GO
