USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[InsertSpecToDb]    Script Date: 04/06/2017 15:08:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertSpecToDb]  @user_Id int,@roomId int, @Game_Id int
AS
BEGIN 
      Insert into SpectetorGamesOfUser values(@user_Id ,@roomId, @Game_Id )
END




GO
