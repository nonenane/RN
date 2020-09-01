SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-28
-- Description:	Подтверждение или аннуляция съезда
-- =============================================
CREATE PROCEDURE [CountRN].[spg_setTSaveRN]		 
	@DateStart date,
	@DateEnd date,
	@isOptOtgruz bit,
	@isOnlyShipped bit,
	@isInventorySpis bit,
	@TotalPrihod numeric(16,2),
	@TotalRealiz numeric(16,2),
	@TotalRestStart numeric(16,2),
	@TotalRestStop numeric(16,2),
	@TotalRN numeric(16,2),
	@TotalPercentRN numeric(16,2),
	@id_user int

AS
BEGIN
	SET NOCOUNT ON;

BEGIN TRY 
		
		IF EXISTS(select id from CountRN.j_tSaveRN t where t.DateStart = @DateStart and t.DateEnd = @DateEnd and t.isOptOtgruz = @isOptOtgruz and t.isInventorySpis  = @isInventorySpis and t.isOnlyShipped  = @isOnlyShipped)
			BEGIN
				DECLARE @id int
					select @id = id from CountRN.j_tSaveRN t where t.DateStart = @DateStart and t.DateEnd = @DateEnd and t.isOptOtgruz = @isOptOtgruz and t.isInventorySpis  = @isInventorySpis and t.isOnlyShipped  = @isOnlyShipped
				UPDATE 
					CountRN.j_tSaveRN 
				SET 
					TotalPrihod = @TotalPrihod,
					TotalRealiz = @TotalRealiz,
					TotalRestStart = @TotalRestStart,
					TotalRestStop = @TotalRestStop,
					TotalRN = @TotalRN,
					TotalPercentRN = @TotalPercentRN,
					id_Creator = @id_user,
					DateCreate = GETDATE()
				WHERE 
					--DateStart = @DateStart and DateEnd = @DateEnd and isOptOtgruz = @isOptOtgruz and isInventorySpis  = @isInventorySpis and isOnlyShipped  = @isOnlyShipped
					id = @id

				SELECT @id as id
			END
		ELSE
			BEGIN
				INSERT INTO CountRN.j_tSaveRN (DateStart,DateEnd,isOptOtgruz,isOnlyShipped,isInventorySpis,TotalPrihod,TotalRealiz,TotalRestStart,TotalRestStop,TotalRN,TotalPercentRN,id_Creator,DateCreate)
				VALUES (@DateStart,@DateEnd,@isOptOtgruz,@isOnlyShipped,@isInventorySpis,@TotalPrihod,@TotalRealiz,@TotalRestStart,@TotalRestStop,@TotalRN,@TotalPercentRN,@id_user,GETDATE())				

				SELECT cast(SCOPE_IDENTITY() as int) as id
			END
END TRY 
BEGIN CATCH 
	SELECT -9999 as id
	return;
END CATCH
	
END
