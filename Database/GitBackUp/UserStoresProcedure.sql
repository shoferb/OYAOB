USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[AddNewUser]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteUserById]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteUserByUserName]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditAvatar]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditEmail]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditName]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditPassword]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserHighestCashGainInGame]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserId]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserIsActive]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserLeagueName]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserMoney]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUsername]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserNumOfGamesPlayed]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserPoints]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserTotalProfit]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[EditUserWinNum]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetAllUser]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserByUserId]    Script Date: 6/4/2017 1:39:05 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetUserByUserName]    Script Date: 6/4/2017 1:39:05 PM ******/
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
