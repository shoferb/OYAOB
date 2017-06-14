USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomById]    Script Date: 14/06/2017 16:04:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomById] @roomid int, @gameid int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM GameRoom
WHERE RoomId = @roomId AND GameId = @gameid
END



GO
