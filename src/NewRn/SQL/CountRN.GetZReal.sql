USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[GetZReal]    Script Date: 11/10/2010 11:43:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <09.11.2010>
-- Description:	<Вычисляет реализацию в закупочных ценах по отделу @id_otdel за период с @date_start и @date_finish, а также НДС 18% и 10%.>
-- =============================================
CREATE PROCEDURE [CountRN].[GetZReal]
	@id_otdel int,
	@date_start datetime,
	@date_finish datetime
AS
BEGIN
	SET NOCOUNT ON;

	create table #tmp1 -- временная таблица ПРОШЛЫХ остатков по отделу
	(
		id int,
		ean char(13),
		cname varchar(100),
		id_grp1 int,
		id_grp2 int,
		kol numeric(11,4)
	)

	-- дата расчёта остатков на начало должна быть на 1 день меньше
	declare @daterem_start datetime
	set @daterem_start = dateadd(day, -1, @date_start)

	-- заполнение #tmp1
	insert into #tmp1 (id, ean, cname, id_grp1, id_grp2, kol)
		exec CountRN.SelectDvigTovarRemains2 @daterem_start, @id_otdel

	create table #tmp2 -- временная таблица ТЕКУЩИХ остатков по отделу
	(
		id int,
		ean char(13),
		cname varchar(100),
		id_grp1 int,
		id_grp2 int,
		kol numeric(11,4)
	)

	-- заполнение #tmp2
	insert into #tmp2 (id, ean, cname, id_grp1, id_grp2, kol)
		exec CountRN.SelectDvigTovarRemains2 @date_finish, @id_otdel

	-- вычисляем реализацию в закупочных ценах
    declare realcurs cursor for
		select id_tovar, 
--				CountRN.GetTovarRemainsSum(id_tovar, @daterem_start, #tmp1.kol),
--				CountRN.GetTovarPrihodWithoutVkass(id_tovar, @date_start, @date_finish),
--				CountRN.GetTovarRemainsSum(id_tovar, @date_finish, #tmp2.kol),
			   (CountRN.GetTovarRemainsSum(id_tovar, @daterem_start, #tmp1.kol) +
			   CountRN.GetTovarPrihodWithoutVkass(id_tovar, @date_start, @date_finish) -
			   CountRN.GetTovarRemainsSum(id_tovar, @date_finish, #tmp2.kol)) as zreal
		from (select distinct id_tovar from j_realiz join s_tovar on j_realiz.id_tovar = s_tovar.id where drealiz >= @date_start and drealiz <= @date_finish and s_tovar.id_otdel = @id_otdel) as realiz
					  left outer join #tmp1 on realiz.id_tovar = #tmp1.id
		              left outer join #tmp2 on realiz.id_tovar = #tmp2.id
		order by id_tovar

	-- вычисляем НДС и реализацию без НДС
	declare @id_tovar int, @zreal numeric(13,4), @zreal18 numeric(13,4), @zreal10 numeric(13,4), @zrealSum numeric(13,4)
	set @zreal18 = 0
	set @zreal10 = 0
	set @zrealSum = 0
	open realcurs
	fetch next from realcurs into @id_tovar, @zreal
	while @@fetch_status = 0
	begin
		declare @nds numeric(2,0)
		select @nds = s_nds.nds
		from s_tovar join s_nds on s_tovar.id_nds = s_nds.id
		where s_tovar.id = @id_tovar
		if @nds = 18
			set @zreal18 = @zreal18 + @zreal
		if @nds = 10
			set @zreal10 = @zreal10 + @zreal
		set @zrealSum = @zrealSum + @zreal
		fetch next from realcurs into @id_tovar, @zreal
	end
	close realcurs
	deallocate realcurs

	select @zrealSum as zreal, @zreal18 as zreal18, @zreal10 as zreal10

	drop table #tmp1
	drop table #tmp2
END

--exec CountRN.GetZReal 1, '2010-11-02', '2010-11-07'

--select CountRN.GetTovarPrihod(3631, '2010-11-02', '2010-11-07')


