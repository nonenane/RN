SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-28
-- Description:	Получение расчитанных данных по РН
-- =============================================
ALTER PROCEDURE [CountRN].[spg_getSaveRN]		 
	@id_tSaveRN int
AS
BEGIN
	SET NOCOUNT ON;

		
	select 
	   [id]
      ,[id_tSaveRN]
      ,[id_tovar]
      ,[id_department]
      ,[id_grp1]
      ,[id_grp2]
      ,[RestStart]
      ,[RestStartSum]
      ,[RestStop]
      ,[RestStopSum]
      ,[Prihod]
      ,[PrihodSum]
      ,[Otgruz]
      ,[OtgruzSum]
      ,[Vozvr]
      ,[VozvrSum]
      ,[Spis]
      ,[SpisSum]
      ,[InventSpis]
      ,[InventSpisSum]
      ,[Realiz]
      ,[RealizSum]
      ,[OtgruzOpt]
      ,[OtgruzOptSum]
      ,[VozvrKass]
      ,[VozvrKassSum] 
	  ,[PrihodAll]
	  ,[RealizAll]
	from 
		CountRN.j_SaveRN t 
	where 
		id_tSaveRN = @id_tSaveRN 
		
	
END
