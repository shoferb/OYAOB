USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[InserPubCardToDb]    Script Date: 04/06/2017 13:52:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InserPubCardToDb] @roomId int, @card int, @gameId int
AS
BEGIN 
      Insert into [Public Cards] values(@roomId,@card, @gameId)
END




GO
