USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[CountRN2]    Script Date: 11/10/2010 11:40:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description:	<Вычисляет РН по отделу @id_otdel за период с @date_start по @date_finish.>
-- =============================================
CREATE PROCEDURE [CountRN].[CountRN2]
	@id_otdel int,
	@date_start datetime,
	@date_finish datetime
AS
BEGIN
	SET NOCOUNT ON;

   create table #tmp1
(
	id int,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

declare @daterem_start datetime
set @daterem_start = dateadd(day, -1, @date_start)

insert into #tmp1 (id, ean, cname, id_grp1, id_grp2, kol)
	exec CountRN.SelectDvigTovarRemains2 @daterem_start, @id_otdel

create table #tmp2
(
	id int,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

insert into #tmp2 (id, ean, cname, id_grp1, id_grp2, kol)
	exec CountRN.SelectDvigTovarRemains2 @date_finish, @id_otdel

create table #alltmp
(
	id int,
	ean char(13),
	cname varchar(200),
	id_grp1 int
)

insert into #alltmp (id, ean, cname, id_grp1) 
(select s_tovar.id, s_tovar.ean, s_ntovar.cname, s_tovar.id_grp1
from s_tovar left outer join s_ntovar on s_tovar.id = s_ntovar.id_tovar
where 
s_ntovar.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar = s_tovar.id and s_ntovar.tdate_n<=@date_finish order by tdate_n desc)  
and s_tovar.id in
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

declare @diffid1 int, @diffean1 char(13), @diffcname1 varchar(256), @diffgrp1 int
declare @diffid2 int, @diffean2 char(13), @diffcname2 varchar(256), @diffgrp2 int

-- записи из ост2
declare tmp2diff cursor for
select id, ean, cname, id_grp1 
from #tmp2
where id not in (select distinct id from #alltmp)

open tmp2diff
fetch next from tmp2diff into @diffid2, @diffean2, @diffcname2, @diffgrp2
while @@fetch_status = 0
begin
insert into #alltmp values (@diffid2, @diffean2, @diffcname2, @diffgrp2)
fetch next from tmp2diff into @diffid2, @diffean2, @diffcname2, @diffgrp2
end
close tmp2diff

deallocate tmp2diff

-- записи из ост1
declare tmp1diff cursor for
select id, ean, cname, id_grp1 
from #tmp1
where id not in (select distinct id from #alltmp)

open tmp1diff
fetch next from tmp1diff into @diffid1, @diffean1, @diffcname1, @diffgrp1
while @@fetch_status = 0
begin
insert into #alltmp values (@diffid1, @diffean1, @diffcname1, @diffgrp1)
fetch next from tmp1diff into @diffid1, @diffean1, @diffcname1, @diffgrp1
end
close tmp1diff

deallocate tmp1diff


declare cc cursor for
select #alltmp.id, #alltmp.ean, #alltmp.cname, t1.realiz as realiz, t2.rn as rn, #alltmp.id_grp1
from #alltmp
left outer join
(select id_tovar as id, sum(summa) as realiz
from j_realiz
where drealiz >= @date_start and drealiz <= @date_finish
group by id_tovar) as t1 on #alltmp.id = t1.id 
left outer join
(select #alltmp.id as id, CountRN.GetTovarRN1(#alltmp.id, @date_start, @date_finish, #tmp1.kol, #tmp2.kol) as rn
from #alltmp full join #tmp1 on #alltmp.id = #tmp1.id full join #tmp2 on #alltmp.id = #tmp2.id) 
as t2 on #alltmp.id = t2.id

create table #tmpres
(
	id int,
	ean char(13),
	cname varchar(100),
	prihod numeric(13,4),
	realiz numeric(13,4),
	rn numeric(13,4),
	id_grp1 int
)

declare @id int, @ean char(13), @cname varchar(100), @realiz numeric(13,4), @rn numeric(13,4), @id_grp1 int
open cc
fetch next from cc into @id, @ean, @cname, @realiz, @rn, @id_grp1
while @@fetch_status = 0
begin
	if @cname is NULL or @cname = NULL
		select @cname = cname from s_ntovar where id_tovar = @id
	if @cname is NULL or @cname = NULL
		set @cname = ''
	if @realiz is NULL or @realiz = NULL
		set @realiz = 0
	--declare @vkass numeric(13,4)
	--select @vkass = sum(netto*rcena)
	--from j_vozvkass join j_allprihod on j_vozvkass.id_allprihod = j_allprihod.id 
	--where id_tovar = @id and dprihod >= @date_start and dprihod <= @date_finish
	--if (@vkass != NULL)
		--set @realiz = @realiz + @vkass

	insert into #tmpres values (@id, @ean, @cname, CountRN.GetTovarPrihod(@id, @date_start, @date_finish), @realiz, @rn, @id_grp1)
	fetch next from cc into @id, @ean, @cname, @realiz, @rn, @id_grp1
end
close cc

deallocate cc

--select #tmpres.id, #tmpres.ean, #tmpres.cname, #tmp1.kol, round(CountRN.GetTovarRemainsSum(#tmpres.id, @daterem_start, #tmp1.kol, @date_finish), 2)
--from #tmpres join #tmp1 on #tmpres.id = #tmp1.id
--order by #tmpres.ean

--select #tmpres.id, #tmpres.ean, #tmpres.cname, #tmp2.kol, round(CountRN.GetTovarRemainsSum(#tmpres.id, @date_finish, #tmp2.kol, @date_finish), 2)
--from #tmpres join #tmp2 on #tmpres.id = #tmp2.id
--order by #tmpres.ean

--select sum(round(CountRN.GetTovarRemainsSum(#tmpres.id, @daterem_start, #tmp1.kol), 2))
--from #tmpres join #tmp1 on #tmpres.id = #tmp1.id

--select #tmpres.id_grp1, sum(round(CountRN.GetTovarRemainsSum(#tmpres.id, @date_finish, #tmp2.kol), 2))
--from #tmpres join #tmp2 on #tmpres.id = #tmp2.id
--group by #tmpres.id_grp1

select * from #tmpres
order by ean

drop table #tmp1
drop table #tmp2
drop table #tmpres
drop table #alltmp
END

--exec CountRN.CountRN2 3, '2010-11-01', '2010-11-07'

--select CountRN.GetTovarRN1(2590,'2010-11-01 23:59:59', '2010-11-06 23:59:59',1995.7380,3050.4)

--select CountRN.GetTovarRemainsSum(152135, '2010-09-30', -10, '2010-09-010')




