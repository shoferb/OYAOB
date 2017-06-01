USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[AddNewUser]    Script Date: 6/1/2017 9:46:46 PM ******/
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
