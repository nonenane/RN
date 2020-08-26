USE [dbase1]
GO
/****** Object:  UserDefinedFunction [CountRN].[GetTovarPrihod]    Script Date: 11/10/2010 11:44:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <07.11.2010>
-- Description:	<Вычисляет полный приход по товару @id_tovar за период с @date_start по @date_finish.>
-- =============================================
CREATE FUNCTION [CountRN].[GetTovarPrihod]
(
	@id_tovar int,
	@date_start datetime,
	@date_finish datetime
)
RETURNS numeric(13,4)
AS
BEGIN
	DECLARE @result numeric(13,4)

	declare @prihod numeric (13,4), @otgruz numeric (13,4), @spis numeric (13,4), @vozvr numeric (13,4), @vozvkass numeric(13,4)
		select @prihod = sum(zcena*netto)
		from j_prihod inner join j_allprihod on j_prihod.id_allprihod = j_allprihod.id
		where j_prihod.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@prihod is NULL)
			set @prihod = 0
		select @otgruz = sum(zcena*netto)
		from j_otgruz inner join j_allprihod on j_otgruz.id_allprihod = j_allprihod.id
		where j_otgruz.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@otgruz is NULL)
			set @otgruz = 0
		select @spis = sum(zcena*netto)
		from j_spis inner join j_allprihod on j_spis.id_allprihod = j_allprihod.id
		where j_spis.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@spis is NULL)
			set @spis = 0
		select @vozvr = sum(zcena*netto)
		from j_vozvr inner join j_allprihod on j_vozvr.id_allprihod = j_allprihod.id
		where j_vozvr.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@vozvr is NULL)
			set @vozvr = 0
		select @vozvkass = sum(netto*rcena)
		from j_vozvkass inner join j_allprihod on j_vozvkass.id_allprihod = j_allprihod.id
		where j_vozvkass.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@vozvkass is NULL)
			set @vozvkass = 0
	set @result = @prihod - @otgruz - @vozvr - @spis - @vozvkass

	RETURN @result

END


