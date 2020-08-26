USE [dbase1]
GO
/****** Object:  UserDefinedFunction [CountRN].[GetTovarRN1]    Script Date: 11/10/2010 11:46:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:  <Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description: <Вычисляет РН по товару @id_tovar за период с @date_start по @date_finish с остатками @remain_start и @remain_finish.>
-- =============================================
CREATE FUNCTION [CountRN].[GetTovarRN1]
(
	@id_tovar int,
	@date_start datetime,
	@date_finish datetime,
	@remain_start numeric(11,3),
	@remain_finish numeric(11,3)
)
RETURNS numeric(13,4)
AS
BEGIN
 DECLARE @rn numeric(13,4)

if @remain_start is NULL
			set @remain_start = 0
		if @remain_finish is NULL
			set @remain_finish = 0
		declare @prihod numeric (13,4), @otgruz numeric (13,4), @spis numeric (13,4), @vozvr numeric (13,4), @realiz numeric (13,4), @vozvkass numeric(13,4)
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
		select @realiz = sum(summa) from j_realiz
		where id_tovar = @id_tovar and drealiz >= @date_start and drealiz <= @date_finish
		if (@realiz is NULL)
			set @realiz = 0
		select @vozvkass = sum(netto*rcena)
		from j_vozvkass inner join j_allprihod on j_vozvkass.id_allprihod = j_allprihod.id
		where j_vozvkass.id_tovar = @id_tovar and dprihod >= @date_start and dprihod <= @date_finish
		if (@vozvkass is NULL)
			set @vozvkass = 0

		declare @daterem_start datetime
		set @daterem_start = dateadd(day, -1, @date_start)

		set @rn = @realiz - (CountRN.GetTovarRemainsSum(@id_tovar, @daterem_start, @remain_start) + @prihod - @otgruz - @vozvr - @spis - @vozvkass - CountRN.GetTovarRemainsSum(@id_tovar, @date_finish, @remain_finish))

 RETURN @rn
END

--select CountRN.GetTovarRN1(2590,'2010-11-01 23:59:59','2010-11-06 23:59:59', 1995.7380, 3050.4)






