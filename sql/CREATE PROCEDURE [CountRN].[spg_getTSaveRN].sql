SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-28
-- Description:	Получение заголовка расчитанных данных по РН
-- =============================================
ALTER PROCEDURE [CountRN].[spg_getTSaveRN]		 
		@dateStart date,
		@dateEnd date
AS
BEGIN
	SET NOCOUNT ON;


select 
	s.id,
	convert(varchar,s.DateStart,104)+' - ' + convert(varchar,s.DateEnd,104) as namePeriod,
	s.DateStart,
	s.DateEnd,
	'Сохранённые' as typeCalc,
	s.isInventorySpis,
	s.isOnlyShipped,
	s.isOptOtgruz,
	s.TotalPrihod,
	s.TotalRealiz,
	s.TotalRestStart,
	s.TotalRestStop,
	s.TotalRN,
	s.TotalPercentRN,
	l.FIO,
	s.DateCreate
from 
	CountRN.j_tSaveRN s
		left join dbo.ListUsers l on l.id = s.id_Creator
where 
	s.DateStart=@dateStart and s.DateEnd = @dateEnd

		
	
END
