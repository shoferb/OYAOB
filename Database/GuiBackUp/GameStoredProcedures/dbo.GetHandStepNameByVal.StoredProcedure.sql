USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[GetHandStepNameByVal]    Script Date: 04/06/2017 12:22:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetHandStepNameByVal] @Val int
AS
BEGIN 
     SET NOCOUNT ON 
SELECT *
FROM HandStep
WHERE [hand Step value] = @Val
END



GO
