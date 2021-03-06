USE [dbase1]
GO
/****** Object:  StoredProcedure [rn].[CountPrihodRealizOptInv_inv]    Script Date: 01.09.2020 14:51:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <15.06.2010>
-- Description:	<Вычисляет приход и реализацию по отделу @id_otdel за период с @date_start по @date_finish с учётом количества дней @days от даты инвентаризации с учётом оптовых отгрузок.>
-- =============================================

ALTER PROCEDURE [rn].[CountPrihodRealizOptInv_inv]
	@id_otdel int,
	@date_start datetime,
	@date_finish datetime,
	@shipped bit
  --with recompile
AS
BEGIN
	SET NOCOUNT ON;
   create table #tmp1
(
	id int primary key,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

declare @daterem_start datetime
set @daterem_start = dateadd(day, -1, @date_start)

insert into #tmp1 (id, ean, cname, id_grp1, id_grp2, kol)
	exec CountRN.SelectDvigTovarRemains2Days @daterem_start, @id_otdel, 0

create table #tmp2
(
	id int primary key,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

insert into #tmp2 (id, ean, cname, id_grp1, id_grp2, kol)
	exec CountRN.SelectDvigTovarRemains2Days @date_finish, @id_otdel, 0

create table #alltmp
(
	id int primary key,
	ean char(13),
	cname varchar(200),
	id_grp2 int,
	id_grp1 int
)

insert into #alltmp (id, ean, cname, id_grp2,id_grp1) 
(select s_tovar.id, s_tovar.ean, isnull(s_ntovar.cname,''), s_tovar.id_grp2,s_tovar.id_grp1
from s_tovar left outer join s_ntovar on s_tovar.id = s_ntovar.id_tovar
and
s_ntovar.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar = s_tovar.id and s_ntovar.tdate_n<=@date_finish order by tdate_n desc)  
where s_tovar.id in
(
-- j_prihod
select distinct id_tovar
from j_prihod 
join s_tovar on j_prihod.id_tovar = s_tovar.id
join j_allprihod on j_prihod.id_allprihod = j_allprihod.id
where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel

union

-- j_otgruz
select distinct id_tovar 
from j_otgruz 
join s_tovar on j_otgruz.id_tovar = s_tovar.id
join j_allprihod on j_otgruz.id_allprihod = j_allprihod.id
where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel

union

-- j_vozvr
select distinct id_tovar 
from j_vozvr 
join s_tovar on j_vozvr.id_tovar = s_tovar.id
join j_allprihod on j_vozvr.id_allprihod = j_allprihod.id
where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel

union

-- j_vozvkass
select distinct id_tovar 
from j_vozvkass 
join s_tovar on j_vozvkass.id_tovar = s_tovar.id
join j_allprihod on j_vozvkass.id_allprihod = j_allprihod.id
where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel

union

-- j_spis
select distinct id_tovar 
from j_spis 
join s_tovar on j_spis.id_tovar = s_tovar.id
join j_allprihod on j_spis.id_allprihod = j_allprihod.id
where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel

union

-- j_realiz
select distinct id_tovar 
from j_realiz 
join s_tovar on j_realiz.id_tovar = s_tovar.id
where drealiz >= @date_start and drealiz <= @date_finish and s_tovar.id_otdel = @id_otdel
)
)

insert into #alltmp (id, ean, cname, id_grp2,id_grp1) 
SELECT id, ean, cname, id_grp2,id_grp1 FROM #tmp2 where id not in (select distinct id from #alltmp)

insert into #alltmp (id, ean, cname, id_grp2,id_grp1) 
SELECT id, ean, cname, id_grp2,id_grp1 FROM #tmp1 where id not in (select distinct id from #alltmp)

select 'id' = #alltmp.id, 
		'ean' = #alltmp.ean,
		'cname' = isnull(#alltmp.cname, ''), 
		'prihod_all' = round(
			isnull(prihod.sum, 0) - 
			(
				isnull(otgruz.sum, 0) - 
				isnull(opt.sumopt, 0)
			) - 
			isnull(vozvr.sum, 0) - 
			isnull(spis.sum,0)-
			isnull(vozvkass.sumi, 0)
			,2),
		'prihod' = round(isnull(prihod.sumn, 0),2),
		'otgruz' = round(isnull(otgruz.sumn, 0),2),
		'vozvr' = round(isnull( vozvr.sumn, 0),2),
		'spis' = round(isnull(spis.sumn, 0),2),
		'spis_inv' = round(
			isnull(prihod.sumi, 0) - 
			(isnull(otgruz.sumi, 0) - isnull(opt.sumopti,0)) - 
			isnull(vozvr.sumi, 0) - isnull(spis.sumi, 0) - isnull(vozvkass.sumi, 0),2),
		'realiz_all' = isnull(realiz.sum, 0) + isnull(opt.sumopt, 0) + isnull(vozvkass.sumn,0), 
		'realiz' = isnull(realiz.sum, 0),
		'realiz_opt' = isnull(opt.sumopt, 0),
		'vozvkass' = isnull(vozvkass.sumn, 0),
		'id_grp1' = #alltmp.id_grp1,
		'id_grp2' = #alltmp.id_grp2,
		'kol1' = isnull(#tmp1.kol,0),
		'kol2' = isnull(#tmp2.kol,0)
from
	#alltmp full join #tmp1 on #alltmp.id = #tmp1.id full join #tmp2 on #alltmp.id = #tmp2.id
				left join
						(select id_tovar, 
						sum(round(zcena*netto,2)) as sum,
						sum(round(zcena*case when InventSpis = 0 then netto else 0 end,2)) as sumn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumi
						 from j_allprihod left join j_prihod on j_prihod.id_allprihod = j_allprihod.id
						 where dprihod >= @date_start and dprihod <= @date_finish
						 group by id_tovar) as prihod on #alltmp.id = prihod.id_tovar
				left join
						(select id_tovar, 
						sum(round(zcena*netto,2)) as sum,
						sum(round(zcena*case when InventSpis = 0 then netto else 0 end,2)) as sumn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumi
						 from j_allprihod left join j_otgruz on j_otgruz.id_allprihod = j_allprihod.id
						 where dprihod >= @date_start and dprihod <= @date_finish
						 group by id_tovar) as otgruz on #alltmp.id = otgruz.id_tovar
				left join
						(select id_tovar, 
						sum(round(zcena*netto,2)) as sum,
						sum(round(zcena*case when InventSpis = 0 then netto else 0 end,2)) as sumn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumi
						 from j_allprihod left join j_vozvr on j_vozvr.id_allprihod = j_allprihod.id
						 where dprihod >= @date_start and dprihod <= @date_finish
						 group by id_tovar) as vozvr on #alltmp.id = vozvr.id_tovar
				left join
						(select id_tovar, 
						sum(round(zcena*netto,2)) as sum,
						sum(round(zcena*case when InventSpis = 0 then netto else 0 end,2)) as sumn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumi
						 from j_allprihod left join j_spis on j_spis.id_allprihod = j_allprihod.id
						 where dprihod >= @date_start and dprihod <= @date_finish
						 group by id_tovar) as spis on #alltmp.id = spis.id_tovar
				left join
						(select id_tovar, 
						sum(round(rcena*netto,2)) as sum,
						sum(round(rcena*case when InventSpis = 0 then netto else 0 end,2)) as sumn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumi
						 from j_allprihod left join j_vozvkass on j_vozvkass.id_allprihod = j_allprihod.id
						 where dprihod >= @date_start and dprihod <= @date_finish
						 group by id_tovar) as vozvkass on #alltmp.id = vozvkass.id_tovar
				left join
						(select id_tovar, sum(summa) as sum
						 from j_realiz
						 where drealiz >= @date_start and drealiz <= @date_finish
						 group by id_tovar) as realiz on #alltmp.id = realiz.id_tovar
				left join
						(select id_tovar, 
						sum(round(zcena*netto,2)) as sumopt,
						sum(round(zcena*case when InventSpis = 0 then netto else 0 end,2)) as sumoptn,
						sum(round(zcena*case when InventSpis = 1 then netto else 0 end,2)) as sumopti
						 from j_allprihod left join j_otgruz on j_otgruz.id_allprihod = j_allprihod.id
						 where j_allprihod.SubTypeOperand = 21 and dprihod >= @date_start and dprihod <= @date_finish and (@shipped = 0 or (@shipped = 1 and j_allprihod.shipped = 1))
						group by id_tovar) opt on opt.id_tovar = #alltmp.id
order by id
OPTION (MAXDOP 2)


drop table #tmp1
drop table #tmp2
drop table #alltmp

END

-- exec [CountRN].[CountRNDebugger] 3, '2011-05-01', '2011-05-31'
