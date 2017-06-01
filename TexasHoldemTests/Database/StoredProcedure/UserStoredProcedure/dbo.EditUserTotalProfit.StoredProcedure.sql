USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[EditUserTotalProfit]    Script Date: 6/1/2017 9:46:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserTotalProfit]
       @UserId  int ,
       @newTotalProfit int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             TotalProfit   = @newTotalProfit
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END

GO
