SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-27
-- Description:	Получение данных по товару для сохранения в РН
-- =============================================
CREATE PROCEDURE [CountRN].[spg_getTovarDataToSaveRN_new]		 
	@dateStart date, 
	@dateStop date
AS
BEGIN
	SET NOCOUNT ON;
	
BEGIN --@RestStartSum
DECLARE @id_ttost int, @dttost date
select TOP(1)  @id_ttost= id,@dttost = dttost from dbo.j_ttost where promeg = 0 and dttost<= @dateStart order by dttost desc


select isnull(sum(isnull(a.netto,0)),0) as RestStartSum,a.id_tovar INTO #RestStartSum  from(
select 
	isnull(o.netto,0) as netto ,o.id_tovar
from 
	dbo.j_tost t inner join dbo.j_ost o on o.id_tost = t.id 
where t.id_ttost = @id_ttost --and o.id_tovar = @id_tovar
UNION
select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart --and  p.id_tovar = @id_tovar
UNION 
select 
	ABS(isnull(p.netto,0)) as netto,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart --and  p.id_tovar = @id_tovar
UNION 
select
	-1*isnull(r.netto,0) as netto,id_tovar
from
	dbo.j_realiz r
where 
	DATEADD(day,1,@dttost)<=r.drealiz and r.drealiz<=@dateStart --and  r.id_tovar = @id_tovar
)as a GROUP BY id_tovar 

END

BEGIN --@RestStopSum
select TOP(1) @id_ttost= id,@dttost = dttost from dbo.j_ttost where promeg = 0 and dttost<= @dateStop order by dttost desc


select isnull(sum(isnull(a.netto,0)),0) as RestStopSum,id_tovar INTO #RestStopSum from(
select 
	isnull(o.netto,0) as netto ,id_tovar
from 
	dbo.j_tost t inner join dbo.j_ost o on o.id_tost = t.id 
where t.id_ttost = @id_ttost --and o.id_tovar = @id_tovar
UNION
select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto,id_tovar 
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop --and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop-- and  p.id_tovar = @id_tovar
UNION 
select 
	ABS(isnull(p.netto,0)) as netto,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop --and  p.id_tovar = @id_tovar
UNION 
select
	-1*isnull(r.netto,0) as netto,id_tovar
from
	dbo.j_realiz r
where 
	DATEADD(day,1,@dttost)<=r.drealiz and r.drealiz<=@dateStop --and  r.id_tovar = @id_tovar
)as a group by id_tovar

END

BEGIN


select 
	isnull(sum(p.netto),0) as prihod, id_tovar INTO #Prihod
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  a.InventSpis  = 0
GROUP BY id_tovar



select 
	isnull(sum(p.netto),0) as otgruz,id_tovar INTO #Otgruz
from 
	dbo.j_allprihod a
		inner join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and   a.InventSpis  = 0
GROUP BY id_tovar


select 
	isnull(sum(p.netto),0) as vozvr,id_tovar INTO #Vozvr
from 
	dbo.j_allprihod a
		inner join dbo.j_vozvr p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop  and a.InventSpis  = 0
group by id_tovar


select 
	isnull(sum(p.netto),0) as spis, id_tovar INTO #Spis
from 
	dbo.j_allprihod a
		inner join dbo.j_spis p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis  = 0
group by id_tovar


select 
	isnull(sum(p.netto),0) as vozvrKass,id_tovar INTO #VozvrKass 
from 
	dbo.j_allprihod a
		inner join dbo.j_vozvkass p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis  = 0
GROUP BY id_tovar


select
	isnull(sum(r.netto),0) as realiz, id_tovar INTO #Realiz
from
	dbo.j_realiz r
where 
	@dateStart<=r.drealiz and r.drealiz<=@dateStop
GROUP BY id_tovar


select sum(a.netto) as inventSpis,id_tovar INTO #InventSpis  from(

select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis = 1
UNION 
select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis = 1
UNION 
select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis = 1
UNION 
select 
	isnull(p.netto,0) as netto ,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis = 1
UNION 
select 
	isnull(p.netto,0) as netto,id_tovar
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.InventSpis = 1
)as a 
GROUP BY id_tovar

select 
	isnull(sum(p.netto),0) as OtgruzOpt,id_tovar INTO #OtgruzOpt
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and a.SubTypeOperand in (21) and a.shipped = 1 
GROUP BY id_tovar

END

select 
	id_tovar,
	sum(RestStartSum) as RestStartSum,
	sum(RestStopSum) as RestStopSum,
	sum(Prihod) AS Prihod,
	sum(Otgruz)-sum(OtgruzOpt)  as Otgruz,
	sum(Vozvr) as Vozvr,
	sum(Spis) as Spis,
	sum(VozvrKass) as VozvrKass,
	sum(Realiz) as Realiz,
	sum(InventSpis) as InventSpis,
	sum(OtgruzOpt) as OtgruzOpt  
from(
select id_tovar,RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #RestStartSum
union all
select id_tovar,0.0 as RestStartSum,RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #RestStopSum
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,prihod AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #Prihod
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,otgruz as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #Otgruz
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,vozvr as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #Vozvr
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,spis as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #Spis
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,vozvrKass as VozvrKass,0.0 as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #VozvrKass
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,realiz as Realiz,0.0 as InventSpis,0.0 as OtgruzOpt  from #Realiz
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,inventSpis as InventSpis,0.0 as OtgruzOpt  from #InventSpis
union all
select id_tovar,0.0 as RestStartSum,0.0 as RestStopSum,0.0 AS Prihod,0.0 as Otgruz,0.0 as Vozvr,0.0 as Spis,0.0 as VozvrKass,0.0 as Realiz,0.0 as InventSpis,OtgruzOpt as OtgruzOpt  from #OtgruzOpt
) as a where id_tovar is not null
group by id_tovar

DROP TABLE #RestStartSum,#RestStopSum,#Prihod,#Otgruz,#Vozvr,#Spis,#VozvrKass,#Realiz,#InventSpis,#OtgruzOpt

END