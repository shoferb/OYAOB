USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[GetPlayerByRoomIdAndUserId]    Script Date: 03/06/2017 15:24:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPlayerByRoomIdAndUserId] @UserId int, @RoomId int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM Players
WHERE [user Id] = @UserId AND [room Id]=@RoomId
END


GO
