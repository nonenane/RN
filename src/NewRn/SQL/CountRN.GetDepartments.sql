USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[GetDepartments]    Script Date: 11/10/2010 11:41:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description:	<Получает список действующих коммерческих отделов.>
-- =============================================
CREATE PROCEDURE [CountRN].[GetDepartments]
AS
BEGIN
	SET NOCOUNT ON;

	select id, name 
	from departments
	where if_comm = 1 and ldeyst = 1 and id != 9
END

