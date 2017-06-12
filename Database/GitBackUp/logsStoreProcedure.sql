USE [DataBaseSadna]
GO
/****** Object:  StoredProcedure [dbo].[DeleteErrorLogById]    Script Date: 6/12/2017 10:45:45 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteLogById]    Script Date: 6/12/2017 10:45:45 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DeleteSystemLogById]    Script Date: 6/12/2017 10:45:45 PM ******/
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
