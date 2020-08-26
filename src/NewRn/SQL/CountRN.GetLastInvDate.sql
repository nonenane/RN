USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[GetLastInvDate]    Script Date: 11/10/2010 11:42:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description:	<Получает последнюю дату основной инвентаризации.>
-- =============================================
CREATE PROCEDURE [CountRN].[GetLastInvDate]
AS
BEGIN
	SET NOCOUNT ON;

    select top 1 dttost from j_ttost
	where promeg = 0
	order by dttost desc
END
