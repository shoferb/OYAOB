USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[GetLeagueNameByVal]    Script Date: 03/06/2017 15:24:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetLeagueNameByVal] @Val int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM LeagueName
WHERE [League Value] = @Val
END


GO
