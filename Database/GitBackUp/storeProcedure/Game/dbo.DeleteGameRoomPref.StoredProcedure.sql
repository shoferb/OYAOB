USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[DeleteGameRoomPref]    Script Date: 14/06/2017 15:33:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteGameRoomPref] @roomId int
AS
BEGIN 
     SET NOCOUNT ON 
DELETE 
FROM GameRoomPreferance
WHERE Roomid = @roomId 
END




GO
