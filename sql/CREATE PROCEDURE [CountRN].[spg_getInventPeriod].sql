SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-27
-- Description:	Проверка вхождения в инвент период
-- =============================================
CREATE PROCEDURE [CountRN].[spg_getInventPeriod]		 
	@dateStart date,
	@dateStop date
AS
BEGIN
	SET NOCOUNT ON;
	
IF EXISTS (select TOP(1) id from dbo.j_ttost where promeg = 0 and dateadd(day,1,dttost) = @dateStart)
	BEGIN
		DECLARE @tmpDate date
			select top(1) @tmpDate = dttost from dbo.j_ttost where dateadd(day,1,dttost) > @dateStart and promeg = 0 order by dttost asc
		IF @tmpDate = @dateStop 
			select 1 as id
		else 
			select 0 as id
	END
ELSE 
	select 0 as id

END
