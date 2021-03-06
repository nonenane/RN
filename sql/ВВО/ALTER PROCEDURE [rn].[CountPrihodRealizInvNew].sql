USE [dbase1]
GO
/****** Object:  StoredProcedure [rn].[CountPrihodRealizInvNew]    Script Date: 01.09.2020 11:32:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			<Kondratyeva N.>
-- Create date:		<15.06.2010>
-- Description:		<Вычисляет приход и реализацию по отделу @id_otdel за период с @date_start по @date_finish с учётом количества дней @days от даты инвентаризации.>
-- Modified:		12-01-2015
-- Modified by:		Butakov I.A.
-- Description:		<Удален вызов процедур, тело вызываемых процедур перенесено сюда. Для того, чтобы при вызове текущей процедуры через Exec sql не падал на insert into>
-- =============================================
/*
	declare @id_otdel int = 2
	declare @d1 datetime = '01-09-2014'
	declare @d2 datetime = '01-09-2014'
	
	exec [rn].[CountPrihodRealizInv1] @id_otdel, @d1, @d2
*/

ALTER PROCEDURE [rn].[CountPrihodRealizInvNew]
	@id_otdel int,
	@date_start datetime,
	@date_finish datetime
--with recompile
AS
BEGIN
	SET NOCOUNT ON;

declare @daterem_start datetime
set @daterem_start = dateadd(day, -1, @date_start)

declare @dendp datetime = @daterem_start
set @dendp = dateadd(hour, 23, @dendp)
set @dendp = dateadd(minute, 59, @dendp)
set @dendp = dateadd(second, 59, @dendp)

-----date_inv
declare @date_invt datetime, @date_inv1 datetime, @date_inv datetime 
declare @pr_kol int --признак как считать количество

declare @days_par int
set @days_par = [dbo].[GetDaysAfterInventory]() -- параметр из prog_config

select top 1 @date_inv = dateadd(dd, 1, dttost) , @date_invt = dateadd(dd, 0, @dendp)
	from j_ttost	
	where (promeg = 0)
		and datediff(day,dttost,getdate())>=@days_par
		and datediff(day,dttost,@dendp)>=0
	order by dttost desc

-----------------

set @pr_kol=2
	
   create table #tmp1
(
	id int primary key,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

insert into #tmp1 (id, ean, cname, id_grp1, id_grp2, kol)
	--exec CountRN.SelectDvigTovarRemains2Days @daterem_start, @id_otdel, 0
	select id,
		   ean,
		   case ntypetovar 
				when 0 then ''
				when 1 then 'РЕЗЕРВ '
				when 2 then 'УЦЕНКА '
				when 3 then 'РЕЗЕРВ УЦЕНКА '
			end
		   +' '+ltrim(rtrim(isnull(cname,''))) as cname,
		   id_grp1,
		   id_grp2,
		   case @pr_kol
				when 1 then  sum(netto_ost-netto_p+netto_r+netto_v+netto_vk+netto_s+netto_o)
				when 2 then  sum(netto_ost+netto_p-netto_r-netto_v-netto_vk-netto_s-netto_o)  
		   end as kol
	from
	---j_prihod-1
		(select 	a.id,a.ean,
		ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,sum(b.netto) as netto_p,
		0000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_prihod  as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			   and a.id_otdel=@id_otdel	  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union
	---j_vozvr-2
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000  as netto_p,
		sum(b.netto) as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_vozvr as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			   and a.id_otdel=@id_otdel	 		  	 
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union
	---j_vozvkass-3
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,sum(b.netto) as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_vozvkass as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt) 	
			   and a.id_otdel=@id_otdel	 	  	
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union 
	---j_spis-4
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,00000000000.000 as netto_vk,sum(b.netto) as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_spis as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			  and a.id_otdel=@id_otdel		 
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union 
	---j_otgruz-5
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		sum(b.netto) as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_otgruz as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			  and a.id_otdel=@id_otdel		  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena		
	union 
	---j_realiz-6
	select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
			00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
			00000000000.000 as netto_o,sum(b.netto) as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_realiz as b on b.id_tovar=a.id 
		where (b.drealiz>=@date_inv and  b.drealiz<=@date_invt) 
			   and a.id_otdel=@id_otdel	 		  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena		
	union
	---j_ost-j_tost--7
	select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
			00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
			00000000000.000 as netto_o,00000000000.000 as netto_r,sum(b.netto) as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_ost  as b on b.id_tovar=a.id
		left outer join j_tost as c on c.id=b.id_tost 
		where 	(c.dtost=@date_inv-1) and c.dtost<=@date_invt
				and a.id_otdel=@id_otdel			
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena) as dvig
	group by id,ean,id_otdel,cname,ntypetovar,id_grp1,id_grp2,rcena	
	order by id,ean,id_otdel,cname,ntypetovar,id_grp1,id_grp2,rcena
	OPTION (MAXDOP 1)	

create table #tmp2
(
	id int primary key,
	ean char(13),
	cname varchar(100),
	id_grp1 int,
	id_grp2 int,
	kol numeric(11,4)
)

set @dendp = @date_finish
set @dendp = dateadd(hour, 23, @dendp)
set @dendp = dateadd(minute, 59, @dendp)
set @dendp = dateadd(second, 59, @dendp)

-----date_inv

set @days_par = [dbo].[GetDaysAfterInventory]() -- параметр из prog_config

select top 1 @date_inv = dateadd(dd, 1, dttost) , @date_invt = dateadd(dd, 0, @dendp)
	from j_ttost	
	where (promeg = 0)
		and datediff(day,dttost,getdate())>=@days_par
		and datediff(day,dttost,@dendp)>=0
	order by dttost desc

-----------------

insert into #tmp2 (id, ean, cname, id_grp1, id_grp2, kol)
	--exec CountRN.SelectDvigTovarRemains2Days @date_finish, @id_otdel, 0
	select id,
		   ean,
		   case ntypetovar 
				when 0 then ''
				when 1 then 'РЕЗЕРВ '
				when 2 then 'УЦЕНКА '
				when 3 then 'РЕЗЕРВ УЦЕНКА '
			end
		   +' '+ltrim(rtrim(isnull(cname,''))) as cname,
		   id_grp1,
		   id_grp2,
		   case @pr_kol
				when 1 then  sum(netto_ost-netto_p+netto_r+netto_v+netto_vk+netto_s+netto_o)
				when 2 then  sum(netto_ost+netto_p-netto_r-netto_v-netto_vk-netto_s-netto_o)  
		   end as kol
	from
	---j_prihod-1
		(select 	a.id,a.ean,
		ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,sum(b.netto) as netto_p,
		0000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_prihod  as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			   and a.id_otdel=@id_otdel	  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union
	---j_vozvr-2
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000  as netto_p,
		sum(b.netto) as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_vozvr as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			   and a.id_otdel=@id_otdel	 		  	 
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union
	---j_vozvkass-3
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,sum(b.netto) as netto_vk,00000000000.000 as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_vozvkass as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt) 	
			   and a.id_otdel=@id_otdel	 	  	
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union 
	---j_spis-4
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,00000000000.000 as netto_vk,sum(b.netto) as netto_s,
		00000000000.000 as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_spis as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			  and a.id_otdel=@id_otdel		 
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena	
	union 
	---j_otgruz-5
		select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
		00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
		sum(b.netto) as netto_o,00000000000.000 as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_otgruz as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		where (c.dprihod>=@date_inv and c.dprihod<=@date_invt)
			  and a.id_otdel=@id_otdel		  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena		
	union 
	---j_realiz-6
	select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
			00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
			00000000000.000 as netto_o,sum(b.netto) as netto_r,00000000000.000 as netto_ost
		from s_tovar as a
		left outer join  s_ntovar as an on
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_realiz as b on b.id_tovar=a.id 
		where (b.drealiz>=@date_inv and  b.drealiz<=@date_invt) 
			   and a.id_otdel=@id_otdel	 		  
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena		
	union
	---j_ost-j_tost--7
	select 	a.id,a.ean,ltrim(rtrim(an.cname)) as cname,an.ntypetovar,a.id_otdel,a.id_grp1,a.id_grp2,d.rcena,00000000000.000 as netto_p,
			00000000000.000 as netto_v,00000000000.000 as netto_vk,00000000000.000 as netto_s,
			00000000000.000 as netto_o,00000000000.000 as netto_r,sum(b.netto) as netto_ost
		from s_tovar as a 
		left outer join  s_ntovar as an on 
		(an.id_tovar=a.id and 
		 an.tdate_n=(select top (1) tdate_n as tdate_n from s_ntovar
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@date_invt order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@date_invt order by tdate_n desc))
		left outer join j_ost  as b on b.id_tovar=a.id
		left outer join j_tost as c on c.id=b.id_tost 
		where 	(c.dtost=@date_inv-1) and c.dtost<=@date_invt
				and a.id_otdel=@id_otdel			
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena) as dvig
	group by id,ean,id_otdel,cname,ntypetovar,id_grp1,id_grp2,rcena	
	order by id,ean,id_otdel,cname,ntypetovar,id_grp1,id_grp2,rcena
	OPTION (MAXDOP 1)
	
	
create table #alltmp
(
	id int primary key,
	ean char(13),
	cname varchar(200),
	id_grp1 int,
	id_grp2 int
)

insert into #alltmp (id, ean, cname, id_grp1,id_grp2) 
(select s_tovar.id, s_tovar.ean, isnull(s_ntovar.cname,''), s_tovar.id_grp1,s_tovar.id_grp2
from s_tovar left outer join s_ntovar on s_tovar.id = s_ntovar.id_tovar and
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

insert into #alltmp (id, ean, cname, id_grp1,id_grp2) 
SELECT id, ean, cname, id_grp1,id_grp2 FROM #tmp2 where id not in (select distinct id from #alltmp)

insert into #alltmp (id, ean, cname, id_grp1,id_grp2) 
SELECT id, ean, cname, id_grp1,id_grp2 FROM #tmp1 where id not in (select distinct id from #alltmp)

select 'id' = #alltmp.id, 
		'ean' = #alltmp.ean,
		'cname' = isnull(#alltmp.cname, ''), 
		'prihod_all' = round(
		isnull(prihod.sum, 0) - 
		isnull(otgruz.sum, 0) - 
		isnull(vozvr.sum, 0) - 
		isnull(spis.sum,0)-
		isnull(vozvkass.sumi, 0)
		,2),
		'prihod' = round(isnull(prihod.sumn, 0),2),
		'otgruz' = round(isnull(otgruz.sumn, 0),2),
		'vozvr' = round(isnull(vozvr.sumn, 0),2),
		'spis' = round(isnull(spis.sumn, 0),2),		
		'spis_inv' = ISNULL(prihod.sumi, 0) - 
					isnull(otgruz.sumi, 0) - 
					isnull(spis.sumi, 0) - 
					isnull(vozvkass.sumi, 0),
		'realiz_all' = isnull(realiz.sum, 0) + isnull(vozvkass.sumn , 0),					
		'realiz' = isnull(realiz.sum, 0),
		'realiz_opt' = 0,
		'vozvkass' = isnull(vozvkass.sumn,0),
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
order by id


--select * from #tmp1
--select * from #tmp2
--select * from #alltmp
drop table #tmp1
drop table #tmp2
drop table #alltmp

END

