USE [master]
GO
/****** Object:  Database [DataBaseSadna]    Script Date: 6/1/2017 2:34:38 PM ******/
CREATE DATABASE [DataBaseSadna]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DataBaseSadna', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DataBaseSadna.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DataBaseSadna_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DataBaseSadna_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DataBaseSadna] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DataBaseSadna].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DataBaseSadna] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DataBaseSadna] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DataBaseSadna] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DataBaseSadna] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DataBaseSadna] SET ARITHABORT OFF 
GO
ALTER DATABASE [DataBaseSadna] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DataBaseSadna] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DataBaseSadna] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DataBaseSadna] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DataBaseSadna] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DataBaseSadna] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DataBaseSadna] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DataBaseSadna] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DataBaseSadna] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DataBaseSadna] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DataBaseSadna] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DataBaseSadna] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DataBaseSadna] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DataBaseSadna] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DataBaseSadna] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DataBaseSadna] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DataBaseSadna] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DataBaseSadna] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DataBaseSadna] SET  MULTI_USER 
GO
ALTER DATABASE [DataBaseSadna] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DataBaseSadna] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DataBaseSadna] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DataBaseSadna] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DataBaseSadna] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DataBaseSadna]
GO
/****** Object:  Table [dbo].[Card]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Card](
	[Card Value] [int] NOT NULL,
	[Card Shpe] [varchar](10) NOT NULL,
	[Card Real Value] [int] NOT NULL,
 CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED 
(
	[Card Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Deck]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deck](
	[index] [int] NOT NULL,
	[room Id] [int] NOT NULL,
	[game Id] [int] NOT NULL,
	[card value] [int] NOT NULL,
 CONSTRAINT [PK_Deck] PRIMARY KEY CLUSTERED 
(
	[index] ASC,
	[room Id] ASC,
	[game Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[logId] [int] NOT NULL,
	[msg] [varchar](150) NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[logId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GameMode]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GameMode](
	[Game mode value] [int] NOT NULL,
	[game mode name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_GameMode] PRIMARY KEY CLUSTERED 
(
	[Game mode value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GameReplay]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GameReplay](
	[room Id] [int] NOT NULL,
	[game Id] [int] NOT NULL,
	[index] [int] NOT NULL,
	[replay] [varchar](5000) NOT NULL,
 CONSTRAINT [PK_GameReplay] PRIMARY KEY CLUSTERED 
(
	[room Id] ASC,
	[game Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GameRoom]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameRoom](
	[room Id] [int] NOT NULL,
	[game id] [int] NOT NULL,
	[Dealer position] [int] NOT NULL,
	[Max Bet In Round] [int] NOT NULL,
	[Pot count] [int] NOT NULL,
	[Bb] [int] NOT NULL,
	[Sb] [int] NOT NULL,
	[is Active Game] [bit] NOT NULL,
	[curr Player] [int] NOT NULL,
	[Dealer Player] [int] NOT NULL,
	[Bb Player] [int] NOT NULL,
	[SB player] [int] NOT NULL,
	[hand step] [int] NOT NULL,
	[First Player In round] [int] NOT NULL,
	[curr player position] [int] NOT NULL,
	[first player in round position] [int] NOT NULL,
	[last rise in round] [int] NOT NULL,
	[league name] [int] NOT NULL,
 CONSTRAINT [PK_GameRoom_1] PRIMARY KEY CLUSTERED 
(
	[room Id] ASC,
	[game id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GameRoomPreferance]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameRoomPreferance](
	[room id] [int] NOT NULL,
	[is Spectetor] [bit] NULL,
	[Min player in room] [int] NULL,
	[max player in room] [int] NULL,
	[enter paying money] [int] NULL,
	[starting chip] [int] NULL,
	[Bb] [int] NULL,
	[Sb] [int] NULL,
	[League name] [int] NULL,
	[Game Mode] [int] NULL,
	[Game Id] [int] NOT NULL,
 CONSTRAINT [PK_GameRoomPreferance] PRIMARY KEY CLUSTERED 
(
	[room id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GamesReplays]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GamesReplays](
	[user Id] [int] NOT NULL,
	[room Id] [int] NOT NULL,
	[game Id] [int] NOT NULL,
 CONSTRAINT [PK_GamesReplays] PRIMARY KEY CLUSTERED 
(
	[user Id] ASC,
	[room Id] ASC,
	[game Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[HandStep]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HandStep](
	[hand Step value] [int] NOT NULL,
	[hand Step name] [varchar](10) NOT NULL,
 CONSTRAINT [PK_HandStep] PRIMARY KEY CLUSTERED 
(
	[hand Step value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LeagueName]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LeagueName](
	[League Value] [int] NOT NULL,
	[League Name] [varchar](10) NULL,
 CONSTRAINT [PK_LeagueName] PRIMARY KEY CLUSTERED 
(
	[League Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Log]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[LogId] [int] NOT NULL,
	[LogPriority] [int] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Players]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Players](
	[room Id] [int] NOT NULL,
	[user Id] [int] NOT NULL,
	[is player active] [bit] NOT NULL,
	[player name] [varchar](50) NOT NULL,
	[Total chip] [int] NOT NULL,
	[Round chip bet] [int] NOT NULL,
	[Player action the round] [bit] NOT NULL,
	[first card] [int] NOT NULL,
	[secund card] [int] NOT NULL,
	[Game Id] [int] NOT NULL,
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[room Id] ASC,
	[user Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PriorityLogEnum]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PriorityLogEnum](
	[PriorityValue] [int] NOT NULL,
	[ProprityName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_PriorityLogEnum] PRIMARY KEY CLUSTERED 
(
	[PriorityValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Public Cards]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Public Cards](
	[room Id] [int] NOT NULL,
	[card] [int] NOT NULL,
	[Game Id] [int] NOT NULL,
 CONSTRAINT [PK_Public Cards_1] PRIMARY KEY CLUSTERED 
(
	[room Id] ASC,
	[Game Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ReplayManager]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReplayManager](
	[roomId] [int] NOT NULL,
	[gameId] [int] NOT NULL,
	[userId] [int] NOT NULL,
 CONSTRAINT [PK_ReplayManager] PRIMARY KEY CLUSTERED 
(
	[roomId] ASC,
	[gameId] ASC,
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SpectetorGamesOfUser]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpectetorGamesOfUser](
	[userId] [int] NOT NULL,
	[roomId] [int] NOT NULL,
	[Game Id] [int] NOT NULL,
 CONSTRAINT [PK_SpectetorGamesOfUser] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemLog]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemLog](
	[logId] [int] NOT NULL,
	[msg] [varchar](150) NULL,
	[roomId] [int] NOT NULL,
	[game Id] [int] NOT NULL,
 CONSTRAINT [PK_SystemLog] PRIMARY KEY CLUSTERED 
(
	[logId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[userId] [int] NOT NULL,
	[username] [varchar](50) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[avatar] [varchar](150) NOT NULL,
	[points] [int] NOT NULL,
	[money] [int] NOT NULL,
	[gamesPlayed] [int] NOT NULL,
	[leagueName] [int] NOT NULL,
	[winNum] [int] NOT NULL,
	[HighestCashGainInGame] [int] NOT NULL,
	[TotalProfit] [int] NOT NULL,
	[inActive] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserActiveGames]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActiveGames](
	[userId] [int] NOT NULL,
	[roomId] [int] NOT NULL,
	[Game Id] [int] NOT NULL,
 CONSTRAINT [PK_UserActiveGames] PRIMARY KEY CLUSTERED 
(
	[userId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserReplaySavedGames]    Script Date: 6/1/2017 2:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserReplaySavedGames](
	[userId] [int] NOT NULL,
	[roomId] [int] NOT NULL,
	[gameId] [int] NOT NULL,
 CONSTRAINT [PK_UserReplaySavedGames] PRIMARY KEY CLUSTERED 
(
	[roomId] ASC,
	[gameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (1, N'Hearts', 1)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (2, N'Hearts', 2)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (3, N'Hearts', 3)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (4, N'Hearts', 4)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (5, N'Hearts', 5)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (6, N'Hearts', 6)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (7, N'Hearts', 7)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (8, N'Hearts', 8)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (9, N'Hearts', 9)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (10, N'Hearts', 10)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (11, N'Hearts', 11)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (12, N'Hearts', 12)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (13, N'Hearts', 13)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (14, N'Diamonds', 1)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (15, N'Diamonds', 2)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (16, N'Diamonds', 3)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (17, N'Diamonds', 4)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (18, N'Diamonds', 5)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (19, N'Diamonds', 6)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (20, N'Diamonds', 7)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (21, N'Diamonds', 8)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (22, N'Diamonds', 9)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (23, N'Diamonds', 10)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (24, N'Diamonds', 11)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (25, N'Diamonds', 12)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (26, N'Diamonds', 13)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (27, N'Spades', 1)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (28, N'Spades', 2)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (29, N'Spades', 3)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (30, N'Spades', 4)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (31, N'Spades', 5)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (32, N'Spades', 6)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (33, N'Spades', 7)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (34, N'Spades', 8)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (35, N'Spades', 9)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (36, N'Spades', 10)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (37, N'Spades', 11)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (38, N'Spades', 12)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (39, N'Spades', 13)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (40, N'Clubs', 1)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (41, N'Clubs', 2)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (42, N'Clubs', 3)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (43, N'Clubs', 4)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (44, N'Clubs', 5)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (45, N'Clubs', 6)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (46, N'Clubs', 7)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (47, N'Clubs', 8)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (48, N'Clubs', 9)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (49, N'Clubs', 10)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (50, N'Clubs', 11)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (51, N'Clubs', 12)
INSERT [dbo].[Card] ([Card Value], [Card Shpe], [Card Real Value]) VALUES (52, N'Clubs', 13)
INSERT [dbo].[GameMode] ([Game mode value], [game mode name]) VALUES (1, N'Limit')
INSERT [dbo].[GameMode] ([Game mode value], [game mode name]) VALUES (2, N'PotLimit')
INSERT [dbo].[GameMode] ([Game mode value], [game mode name]) VALUES (3, N'NoLimit')
INSERT [dbo].[HandStep] ([hand Step value], [hand Step name]) VALUES (1, N'Pre-Flop')
INSERT [dbo].[HandStep] ([hand Step value], [hand Step name]) VALUES (2, N'Flop')
INSERT [dbo].[HandStep] ([hand Step value], [hand Step name]) VALUES (3, N'Turn')
INSERT [dbo].[HandStep] ([hand Step value], [hand Step name]) VALUES (4, N'River')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (1, N'A')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (2, N'B')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (3, N'C')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (4, N'D')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (5, N'E')
INSERT [dbo].[LeagueName] ([League Value], [League Name]) VALUES (6, N'UnKnow')
INSERT [dbo].[PriorityLogEnum] ([PriorityValue], [ProprityName]) VALUES (1, N'Info')
INSERT [dbo].[PriorityLogEnum] ([PriorityValue], [ProprityName]) VALUES (2, N'Warnning')
INSERT [dbo].[PriorityLogEnum] ([PriorityValue], [ProprityName]) VALUES (3, N'Error')
/****** Object:  Index [IX_Log]    Script Date: 6/1/2017 2:34:38 PM ******/
CREATE NONCLUSTERED INDEX [IX_Log] ON [dbo].[Log]
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_User]    Script Date: 6/1/2017 2:34:38 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_User] ON [dbo].[User]
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Card]  WITH CHECK ADD  CONSTRAINT [FK_Card_Card] FOREIGN KEY([Card Value])
REFERENCES [dbo].[Card] ([Card Value])
GO
ALTER TABLE [dbo].[Card] CHECK CONSTRAINT [FK_Card_Card]
GO
ALTER TABLE [dbo].[Deck]  WITH CHECK ADD  CONSTRAINT [FK_Deck_Card] FOREIGN KEY([card value])
REFERENCES [dbo].[Card] ([Card Value])
GO
ALTER TABLE [dbo].[Deck] CHECK CONSTRAINT [FK_Deck_Card]
GO
ALTER TABLE [dbo].[Deck]  WITH CHECK ADD  CONSTRAINT [FK_Deck_GameRoom] FOREIGN KEY([room Id], [game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[Deck] CHECK CONSTRAINT [FK_Deck_GameRoom]
GO
ALTER TABLE [dbo].[ErrorLog]  WITH CHECK ADD  CONSTRAINT [FK_ErrorLog_Log] FOREIGN KEY([logId])
REFERENCES [dbo].[Log] ([LogId])
GO
ALTER TABLE [dbo].[ErrorLog] CHECK CONSTRAINT [FK_ErrorLog_Log]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_HandStep] FOREIGN KEY([hand step])
REFERENCES [dbo].[HandStep] ([hand Step value])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_HandStep]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_LeagueName] FOREIGN KEY([league name])
REFERENCES [dbo].[LeagueName] ([League Value])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_LeagueName]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_User_id_curr_player] FOREIGN KEY([curr Player])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_User_id_curr_player]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_User_id_dealer_player] FOREIGN KEY([Dealer Player])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_User_id_dealer_player]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_User_id_first_player_in_round] FOREIGN KEY([First Player In round])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_User_id_first_player_in_round]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_User_id_sb_player] FOREIGN KEY([SB player])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_User_id_sb_player]
GO
ALTER TABLE [dbo].[GameRoom]  WITH CHECK ADD  CONSTRAINT [FK_GameRoom_User_user_Id_BB_Player] FOREIGN KEY([Bb Player])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GameRoom] CHECK CONSTRAINT [FK_GameRoom_User_user_Id_BB_Player]
GO
ALTER TABLE [dbo].[GameRoomPreferance]  WITH CHECK ADD  CONSTRAINT [FK_GameRoomPreferance_GameMode] FOREIGN KEY([Game Mode])
REFERENCES [dbo].[GameMode] ([Game mode value])
GO
ALTER TABLE [dbo].[GameRoomPreferance] CHECK CONSTRAINT [FK_GameRoomPreferance_GameMode]
GO
ALTER TABLE [dbo].[GameRoomPreferance]  WITH CHECK ADD  CONSTRAINT [FK_GameRoomPreferance_LeagueName] FOREIGN KEY([League name])
REFERENCES [dbo].[LeagueName] ([League Value])
GO
ALTER TABLE [dbo].[GameRoomPreferance] CHECK CONSTRAINT [FK_GameRoomPreferance_LeagueName]
GO
ALTER TABLE [dbo].[GamesReplays]  WITH CHECK ADD  CONSTRAINT [FK_GamesReplays_GameRoom] FOREIGN KEY([room Id], [game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[GamesReplays] CHECK CONSTRAINT [FK_GamesReplays_GameRoom]
GO
ALTER TABLE [dbo].[GamesReplays]  WITH CHECK ADD  CONSTRAINT [FK_GamesReplays_User] FOREIGN KEY([user Id])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[GamesReplays] CHECK CONSTRAINT [FK_GamesReplays_User]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_PriorityLogEnum] FOREIGN KEY([LogPriority])
REFERENCES [dbo].[PriorityLogEnum] ([PriorityValue])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_PriorityLogEnum]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_Players_GameRoom] FOREIGN KEY([room Id], [Game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_Players_GameRoom]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_Players_User] FOREIGN KEY([user Id])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_Players_User]
GO
ALTER TABLE [dbo].[Public Cards]  WITH CHECK ADD  CONSTRAINT [FK_Public Cards_Cards] FOREIGN KEY([card])
REFERENCES [dbo].[Card] ([Card Value])
GO
ALTER TABLE [dbo].[Public Cards] CHECK CONSTRAINT [FK_Public Cards_Cards]
GO
ALTER TABLE [dbo].[Public Cards]  WITH CHECK ADD  CONSTRAINT [FK_Public Cards_GameRoom] FOREIGN KEY([room Id], [Game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[Public Cards] CHECK CONSTRAINT [FK_Public Cards_GameRoom]
GO
ALTER TABLE [dbo].[ReplayManager]  WITH CHECK ADD  CONSTRAINT [FK_ReplayManager_GameRoom] FOREIGN KEY([roomId], [gameId])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[ReplayManager] CHECK CONSTRAINT [FK_ReplayManager_GameRoom]
GO
ALTER TABLE [dbo].[ReplayManager]  WITH CHECK ADD  CONSTRAINT [FK_ReplayManager_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[ReplayManager] CHECK CONSTRAINT [FK_ReplayManager_User]
GO
ALTER TABLE [dbo].[SpectetorGamesOfUser]  WITH CHECK ADD  CONSTRAINT [FK_SpectetorGamesOfUser_GameRoom] FOREIGN KEY([roomId], [Game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[SpectetorGamesOfUser] CHECK CONSTRAINT [FK_SpectetorGamesOfUser_GameRoom]
GO
ALTER TABLE [dbo].[SpectetorGamesOfUser]  WITH CHECK ADD  CONSTRAINT [FK_SpectetorGamesOfUser_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[SpectetorGamesOfUser] CHECK CONSTRAINT [FK_SpectetorGamesOfUser_User]
GO
ALTER TABLE [dbo].[SystemLog]  WITH CHECK ADD  CONSTRAINT [FK_SystemLog_GameRoom] FOREIGN KEY([roomId], [game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[SystemLog] CHECK CONSTRAINT [FK_SystemLog_GameRoom]
GO
ALTER TABLE [dbo].[SystemLog]  WITH CHECK ADD  CONSTRAINT [FK_SystemLog_Log] FOREIGN KEY([logId])
REFERENCES [dbo].[Log] ([LogId])
GO
ALTER TABLE [dbo].[SystemLog] CHECK CONSTRAINT [FK_SystemLog_Log]
GO
ALTER TABLE [dbo].[UserActiveGames]  WITH CHECK ADD  CONSTRAINT [FK_UserActiveGames_GameRoom] FOREIGN KEY([roomId], [Game Id])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[UserActiveGames] CHECK CONSTRAINT [FK_UserActiveGames_GameRoom]
GO
ALTER TABLE [dbo].[UserActiveGames]  WITH CHECK ADD  CONSTRAINT [FK_UserActiveGames_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[UserActiveGames] CHECK CONSTRAINT [FK_UserActiveGames_User]
GO
ALTER TABLE [dbo].[UserReplaySavedGames]  WITH CHECK ADD  CONSTRAINT [FK_UserReplaySavedGames_GameRoom] FOREIGN KEY([roomId], [gameId])
REFERENCES [dbo].[GameRoom] ([room Id], [game id])
GO
ALTER TABLE [dbo].[UserReplaySavedGames] CHECK CONSTRAINT [FK_UserReplaySavedGames_GameRoom]
GO
ALTER TABLE [dbo].[UserReplaySavedGames]  WITH CHECK ADD  CONSTRAINT [FK_UserReplaySavedGames_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([userId])
GO
ALTER TABLE [dbo].[UserReplaySavedGames] CHECK CONSTRAINT [FK_UserReplaySavedGames_User]
GO
USE [master]
GO
ALTER DATABASE [DataBaseSadna] SET  READ_WRITE 
GO
