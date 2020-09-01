SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-28
-- Description:	Сохранение тела РН 
-- =============================================
CREATE PROCEDURE [CountRN].[spg_setSaveRN]		 
	@id_tSaveRN int,
	@id_tovar int,
	@id_department int,
	@id_grp1 int,
	@id_grp2 int,

	@RestStart numeric(16,2),
	@RestStartSum numeric(16,2),
	@RestStop  numeric(16,2),
	@RestStopSum numeric(16,2),
	@PrihodSum numeric(16,2),
	@Prihod numeric(16,2),
	@OtgruzSum numeric(16,2),
	@Otgruz numeric(16,2),
	@VozvrSum numeric(16,2),
	@Vozvr numeric(16,2),
	@SpisSum numeric(16,2),
	@Spis numeric(16,2),
	@InventSpisSum numeric(16,2),
	@InventSpis numeric(16,2),
	@RealizSum numeric(16,2),
	@Realiz numeric(16,2),
	@OtgruzOptSum numeric(16,2),
	@OtgruzOpt numeric(16,2),
	@VozvrKassSum numeric(16,2),
	@VozvrKass numeric(16,2)
AS
BEGIN
	SET NOCOUNT ON;

BEGIN TRY 
		
		IF EXISTS(select id from CountRN.j_SaveRN t where t.id_tSaveRN = @id_tSaveRN and t.id_tovar = @id_tovar)
			BEGIN
				DECLARE @id int
					select @id = id from CountRN.j_SaveRN t where t.id_tSaveRN = @id_tSaveRN and t.id_tovar = @id_tovar


				UPDATE 
					CountRN.j_SaveRN 
				SET 
					id_tSaveRN = @id_tSaveRN,
					id_tovar = @id_tovar,
					id_department = @id_department,
					id_grp1 = @id_grp1,
					id_grp2 = @id_grp2,
					RestStart = @RestStart,
					RestStartSum = @RestStartSum,
					RestStop = @RestStop,
					RestStopSum = @RestStopSum,
					PrihodSum = @PrihodSum,
					Prihod	= @Prihod,
					OtgruzSum = @OtgruzSum,
					Otgruz = @Otgruz,
					VozvrSum = @VozvrSum,
					Vozvr = @Vozvr,
					SpisSum = @SpisSum,
					Spis = @Spis,
					InventSpisSum = @InventSpisSum,
					InventSpis = @InventSpis,
					RealizSum = @RealizSum,
					Realiz = @Realiz,
					OtgruzOptSum = @OtgruzOptSum,
					OtgruzOpt = @OtgruzOpt,
					VozvrKassSum = @VozvrKassSum,
					VozvrKass = VozvrKass
				WHERE 					
					id = @id

				SELECT @id as id
			END
		ELSE
			BEGIN
				INSERT INTO CountRN.j_SaveRN (id_tSaveRN,id_tovar,id_department,id_grp1,id_grp2,RestStart,RestStartSum,RestStop,RestStopSum,Prihod,PrihodSum,Otgruz,OtgruzSum,Vozvr,VozvrSum,Spis,SpisSum,InventSpis,InventSpisSum,Realiz,RealizSum,OtgruzOpt,OtgruzOptSum,VozvrKass,VozvrKassSum)
				VALUES (@id_tSaveRN,@id_tovar,@id_department,@id_grp1,@id_grp2,@RestStart,@RestStartSum,@RestStop,@RestStopSum,@Prihod,@PrihodSum,@Otgruz,@OtgruzSum,@Vozvr,@VozvrSum,@Spis,@SpisSum,@InventSpis,@InventSpisSum,@Realiz,@RealizSum,@OtgruzOpt,@OtgruzOptSum,@VozvrKass,@VozvrKassSum)				

				SELECT cast(SCOPE_IDENTITY() as int) as id
			END
END TRY 
BEGIN CATCH 
	SELECT -9999 as id
	return;
END CATCH
	
END
