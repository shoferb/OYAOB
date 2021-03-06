USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[AddNewSpectetorGamesOfUser]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create procedure [dbo].[AddNewSpectetorGamesOfUser]
(  @userId int,
	@roomId int,
	@gameId int
)  
as  
begin  
   Insert into SpectetorGamesOfUser values(@userId,
										@roomId,
										@gameId
	)  
End 



GO
/****** Object:  StoredProcedure [dbo].[AddNewUser]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create procedure [dbo].[AddNewUser]  
(  @userId int,
	@username varchar(50),
	@name varchar(50),
	@email varchar(50),
	@password varchar(50), 
	@avatar varchar(50),
	@points int,
	@money int,
	@gamesPlayed int,	
	@leagueName int,
	@winNum int,
	@highestCashGainInGame int,
	@totalProfit int,
	@isActive bit
)  
as  
begin  
   Insert into UserTable values(@userId,
								@username,
								@name,
								@email,
								@password, 
								@avatar,
								@points,
								@money,
								@gamesPlayed,	
								@leagueName,
								@winNum,
								@highestCashGainInGame,
								@totalProfit,
								@isActive 
	)  
End 



GO
/****** Object:  StoredProcedure [dbo].[AddUserActiveGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create procedure [dbo].[AddUserActiveGame]
(  @userId int,
	@roomId int,
	@gameId int
)  
as  
begin  
   Insert into UserActiveGames values(@userId,
										@roomId,
										@gameId
	)  
End 



GO
/****** Object:  StoredProcedure [dbo].[DeleteErrorLogById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DeleteErrorLogById]
       @LogId  int 
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.ErrorLog
      FROM   dbo.ErrorLog
      WHERE  
      LogId = @LogId                  

END


GO
/****** Object:  StoredProcedure [dbo].[DeleteGameRoom]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteGameRoom] @roomId int, @gameid int
AS
BEGIN 
     SET NOCOUNT ON 
DELETE 
FROM GameRoom
WHERE Roomid = @roomId AND GameId = @gameid
END




GO
/****** Object:  StoredProcedure [dbo].[DeleteGameRoomPref]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteLogById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DeleteLogById]
       @LogId  int 
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.Log
      FROM   dbo.Log
      WHERE  
      LogId = @LogId                  

END


GO
/****** Object:  StoredProcedure [dbo].[DeleteSystemLogById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DeleteSystemLogById]
       @LogId  int 
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.SystemLog
      FROM   dbo.SystemLog
      WHERE  
      LogId = @LogId                  

END


GO
/****** Object:  StoredProcedure [dbo].[DeleteUserActiveGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DeleteUserActiveGame]
      @userId int,
	   @roomId  int,
	   @gameId int
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.UserActiveGames
      FROM   dbo.UserActiveGames
      WHERE  
      roomId = @roomId AND 
	  [Game Id] = @gameId AND 
	  userId = @userId                 

END


GO
/****** Object:  StoredProcedure [dbo].[DeleteUserById]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteUserByUserName]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteUserSpectetorGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[DeleteUserSpectetorGame]
      @userId int,
	   @roomId  int,
	   @gameId int
       
AS
BEGIN 
      SET NOCOUNT ON 

      DELETE dbo.SpectetorGamesOfUser
      FROM   dbo.SpectetorGamesOfUser
      WHERE  
      roomId = @roomId AND 
	  [Game Id] = @gameId AND 
	  userId = @userId                 

END


GO
/****** Object:  StoredProcedure [dbo].[EditAvatar]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditAvatar]
       @UserId  int ,
       @newAvatar varchar(50)
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             avatar   = @newAvatar
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditEmail]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditName]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditName]
       @UserId  int ,
       @newName varchar(50)
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             name   = @newName
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditPassword]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditPassword]
       @UserId  int ,
       @newPassword varchar(50)
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
              password  = @newPassword
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserHighestCashGainInGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserHighestCashGainInGame]
       @UserId  int ,
       @newHighestCashGainInGame int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             HighestCashGainInGame   = @newHighestCashGainInGame
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserId]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserIsActive]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserIsActive]
       @UserId  int ,
       @newIsActive bit
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             inActive   = @newIsActive
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserLeagueName]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserLeagueName]
       @UserId  int ,
       @newLeague int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             leagueName   = @newLeague
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserMoney]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserMoney]
       @UserId  int ,
       @newMoney int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             money   = @newMoney
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUsername]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUsername]
       @UserId  int ,
       @newUserName varchar(50)
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             username   = @newUserName
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserNumOfGamesPlayed]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserNumOfGamesPlayed]
       @UserId  int ,
       @newNumOfGame int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             gamesPlayed   = @newNumOfGame
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserPoints]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserPoints]
       @UserId  int ,
       @newPoints int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             points   = @newPoints
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[EditUserTotalProfit]    Script Date: 6/20/2017 12:32:04 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserWinNum]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[EditUserWinNum]
       @UserId  int ,
       @newWinNum int
AS
BEGIN 
      SET NOCOUNT ON 

      UPDATE dbo.UserTable
      SET 
             winNum   = @newWinNum
      FROM   dbo.UserTable
      WHERE  
      userId    = @UserId                  

END


GO
/****** Object:  StoredProcedure [dbo].[GetAllActiveGameRooms]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllActiveGameRooms]
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM GameRoom
WHERE isActive = 1
END



GO
/****** Object:  StoredProcedure [dbo].[GetAllGameRooms]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllGameRooms]
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM GameRoom

END





GO
/****** Object:  StoredProcedure [dbo].[GetAllSpectableGameRooms]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllSpectableGameRooms] @canSpec bit
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.CanSpectate = @canSpec
END



GO
/****** Object:  StoredProcedure [dbo].[GetAllUser]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllUser]
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM UserTable

END


GO
/****** Object:  StoredProcedure [dbo].[GetAllUserActiveGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetAllUserActiveGame]
@userId int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM UserActiveGames
Where userId = @userId
END


GO
/****** Object:  StoredProcedure [dbo].[GetErrorLogById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetErrorLogById]
        @LogId int
       
AS
BEGIN 
      SET NOCOUNT ON 

      SELECT *
      FROM   dbo.ErrorLog
      WHERE  
      LogId = @LogId                

END


GO
/****** Object:  StoredProcedure [dbo].[GetGameModeNameByVal]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameModeNameByVal] @Val int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT [game mode name]
FROM GameMode
WHERE [Game mode value] = @Val
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameModeValByName]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameModeValByName] @name varchar(10)
AS
BEGIN 
     SET NOCOUNT ON 
SELECT [Game mode value]
FROM GameMode
WHERE [game mode name] = @name
END





GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomById] @roomid int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM GameRoom
WHERE RoomId = @roomid
ORDER BY GameId DESC
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomPrefById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomPrefById] @roomid int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM GameRoomPreferance
WHERE RoomId = @roomId 
END




GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomReplyById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomReplyById] @id int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT RoomId, Replay
FROM GameRoom
WHERE RoomId = @id
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByBuyInPolicy]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByBuyInPolicy] @biPol int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.BuyInPolicy = @biPol
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByGameMode]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByGameMode] @mode int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.GameMode = @mode
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByMaxPlayers]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByMaxPlayers] @max int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.MaxPlayers = @max
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByMinBet]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByMinBet] @min int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.MinBet = @min
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByMinPlayers]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByMinPlayers] @min int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.MinPlayers = @min
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByPotSize]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByPotSize] @potSize int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.PotSize = @potSize
END



GO
/****** Object:  StoredProcedure [dbo].[GetGameRoomsByStaringChip]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetGameRoomsByStaringChip] @scpol int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT GameRoom.RoomId, GameRoom.GameXML
FROM GameRoomPreferance join GameRoom on GameRoomPreferance.Roomid = GameRoom.RoomId
WHERE GameRoomPreferance.EnterGamePolicy = @scpol
END



GO
/****** Object:  StoredProcedure [dbo].[GetLeageValByName]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetLeageValByName] @name varchar(10)
AS
BEGIN 
     SET NOCOUNT ON 
SELECT [League Value]
FROM LeagueName
WHERE [League Name] = @name
END




GO
/****** Object:  StoredProcedure [dbo].[GetSystemLogById]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetSystemLogById]
        @LogId int
       
AS
BEGIN 
      SET NOCOUNT ON 

      SELECT *
      FROM   dbo.SystemLog
      WHERE  
      LogId = @LogId                

END


GO
/****** Object:  StoredProcedure [dbo].[GetUserByUserId]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByUserId] @userId int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM UserTable
WHERE userId = @userId
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserByUserName]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByUserName] @username varchar(50)
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM UserTable
WHERE username = @username
END


GO
/****** Object:  StoredProcedure [dbo].[GetUserSpectetorsGame]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[GetUserSpectetorsGame]
@userId int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM SpectetorGamesOfUser
Where userId = @userId
END


GO
/****** Object:  StoredProcedure [dbo].[HasThisActiveGamebool]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[HasThisActiveGamebool]
@userId int,
@roomId int,
@gameId int
AS
BEGIN 
     SET NOCOUNT ON 
 Select case		
		When EXISTS (Select * 
					From UserActiveGames
					 where userId = @userId AND
							roomId = @roomId AND
							[Game Id] = @gameId)
				then Cast( 1 as bit)
		ELSE 
				 Cast( 0 as bit)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[HasThisSpectetorGamebool]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[HasThisSpectetorGamebool]
@userId int,
@roomId int,
@gameId int
AS
BEGIN 
     SET NOCOUNT ON 
 Select case		
		When EXISTS (Select * 
					From SpectetorGamesOfUser
					 where userId = @userId AND
							roomId = @roomId AND
							[Game Id] = @gameId)
				then Cast( 1 as bit)
		ELSE 
				 Cast( 0 as bit)
		END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertGameRoomToDb]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertGameRoomToDb] @roomid int, @gameid int, @replay varchar(MAX),
                        @gameXML XML, @isActive bit
AS
BEGIN 
      Insert into GameRoom values(@roomid, @gameid, @replay, @gameXML, @isActive)
END





GO
/****** Object:  StoredProcedure [dbo].[InsertPrefToDb]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertPrefToDb] @room_id int, @is_Spectetor bit, @Min_player_in_room int,
                        @max_player_in_room int, @BuyInPolicy int, @starting_chip int,
                       @minBet int, @League_name int, @Game_Mode int, @Game_Id int, @potSize int
AS
BEGIN 
      Insert into GameRoomPreferance values( @room_id , @is_Spectetor, @Min_player_in_room,
                        @max_player_in_room , @BuyInPolicy , @starting_chip ,
                        @minBet, @League_name , @Game_Mode , @Game_Id, @potSize )
END





GO
/****** Object:  StoredProcedure [dbo].[UpdateGameRoom]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateGameRoom] @roomId int, @gameid int, @newXML XML, @newIsActive bit, @newRep varchar(MAX)
AS
BEGIN 
     SET NOCOUNT ON 
UPDATE GameRoom
SET GameXML = @newXML, isActive = @newIsActive, Replay = @newRep
WHERE Roomid = @roomId AND GameId = @gameid
END




GO
/****** Object:  StoredProcedure [dbo].[UpdateGameRoomPotSize]    Script Date: 6/20/2017 12:32:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateGameRoomPotSize] @newPotSize int, @id int
AS
BEGIN 
     SET NOCOUNT ON 
UPDATE GameRoomPreferance
SET PotSize = @newPotSize
WHERE Roomid = @id
END




GO
