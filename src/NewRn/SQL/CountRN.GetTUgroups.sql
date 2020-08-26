USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[GetTUgroups]    Script Date: 11/10/2010 11:42:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description:	<Получает список действующих групп для отдела @id_otdel.>
-- =============================================
CREATE PROCEDURE [CountRN].[GetTUgroups]
	@id_otdel int
AS
BEGIN
	SET NOCOUNT ON;

	select id, cname
	from s_grp1
	where id_otdel = @id_otdel and ldeystv = 1
END
