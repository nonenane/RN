SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sporykhin G.Y.
-- Create date: 2020-08-27
-- Description:	Получение данных по товару для сохранения в РН
-- =============================================
CREATE PROCEDURE [CountRN].[spg_getTovarDataToSaveRN]		 
	@id_tovar int, 
	@dateStart date, 
	@dateStop date
AS
BEGIN
	SET NOCOUNT ON;
	

DECLARE @id_ttost int, @dttost date
select TOP(1)  @id_ttost= id,@dttost = dttost from dbo.j_ttost where promeg = 0 and dttost<= @dateStart order by dttost desc

DECLARE @RestStartSum numeric(16,2)

select @RestStartSum =  sum(a.netto)  from(
select 
	isnull(sum(isnull(o.netto,0)),0) as netto 
from 
	dbo.j_tost t inner join dbo.j_ost o on o.id_tost = t.id 
where t.id_ttost = @id_ttost and o.id_tovar = @id_tovar
UNION
select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart and  p.id_tovar = @id_tovar
UNION 
select 
	ABS(isnull(sum(p.netto),0)) as netto
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStart and  p.id_tovar = @id_tovar
UNION 
select
	-1*isnull(sum(r.netto),0) as netto
from
	dbo.j_realiz r
where 
	DATEADD(day,1,@dttost)<=r.drealiz and r.drealiz<=@dateStart and  r.id_tovar = @id_tovar
)as a 

select TOP(1) @id_ttost= id,@dttost = dttost from dbo.j_ttost where promeg = 0 and dttost<= @dateStop order by dttost desc

DECLARE @RestStopSum numeric(16,2)

select @RestStopSum =  sum(a.netto)  from(
select 
	isnull(sum(isnull(o.netto,0)),0) as netto 
from 
	dbo.j_tost t inner join dbo.j_ost o on o.id_tost = t.id 
where t.id_ttost = @id_ttost and o.id_tovar = @id_tovar
UNION
select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar
UNION 
select 
	-1*isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar
UNION 
select 
	ABS(isnull(sum(p.netto),0)) as netto
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where DATEADD(day,1,@dttost)<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar
UNION 
select
	-1*isnull(sum(r.netto),0) as netto
from
	dbo.j_realiz r
where 
	DATEADD(day,1,@dttost)<=r.drealiz and r.drealiz<=@dateStop and  r.id_tovar = @id_tovar
)as a 


DECLARE @Prihod numeric(16,2)

select 
	@Prihod = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis  = 0

DECLARE @Otgruz numeric(16,2)

select 
	@Otgruz = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		inner join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis  = 0


DECLARE @Vozvr numeric(16,2)

select 
	@Vozvr = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		inner join dbo.j_vozvr p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis  = 0


DECLARE @Spis numeric(16,2)

select 
	@Spis = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		inner join dbo.j_spis p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis  = 0


DECLARE @VozvrKass numeric(16,2)

select 
	@VozvrKass = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		inner join dbo.j_vozvkass p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis  = 0


DECLARE @Realiz numeric(16,2)

select
	@Realiz = isnull(sum(r.netto),0)
from
	dbo.j_realiz r
where 
	@dateStart<=r.drealiz and r.drealiz<=@dateStop and  r.id_tovar = @id_tovar




DECLARE @InventSpis numeric(16,2)

select @InventSpis =  sum(a.netto)  from(

select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		inner join dbo.j_prihod p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis = 1
UNION 
select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_spis p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis = 1
UNION 
select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_vozvr p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis = 1
UNION 
select 
	isnull(sum(p.netto),0) as netto 
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis = 1
UNION 
select 
	isnull(sum(p.netto),0) as netto
from 
	dbo.j_allprihod a
		left join dbo.j_vozvkass p on p.id_allprihod = a.id		
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.InventSpis = 1
)as a 

DECLARE @OtgruzOpt numeric(16,2)

select 
	@OtgruzOpt = isnull(sum(p.netto),0)
from 
	dbo.j_allprihod a
		left join dbo.j_otgruz p on p.id_allprihod = a.id
where @dateStart<=a.dprihod and a.dprihod<=@dateStop and  p.id_tovar = @id_tovar and a.SubTypeOperand in (21) and a.shipped = 1 

select @RestStartSum as RestStartSum, @RestStopSum as RestStopSum, @Prihod as Prihod, @Otgruz-@OtgruzOpt as Otgruz, @Vozvr as Vozvr,@Spis as Spis, @VozvrKass as VozvrKass, @Realiz as Realiz,@InventSpis as InventSpis, @OtgruzOpt as OtgruzOpt

END
