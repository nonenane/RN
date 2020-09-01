SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-28
-- Description:	ѕроверка существовани€ данных на период
-- =============================================
CREATE PROCEDURE [CountRN].[spg_validateTSaveRN]		 
	@DateStart date,
	@DateEnd date,
	@isOptOtgruz bit,
	@isOnlyShipped bit,
	@isInventorySpis bit
AS
BEGIN
	SET NOCOUNT ON;

		
	select id from CountRN.j_tSaveRN t where t.DateStart = @DateStart and t.DateEnd = @DateEnd and t.isOptOtgruz = @isOptOtgruz and t.isInventorySpis  = @isInventorySpis and t.isOnlyShipped  = @isOnlyShipped
	
END
