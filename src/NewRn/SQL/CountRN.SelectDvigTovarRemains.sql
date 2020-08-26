USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[SelectDvigTovarRemains2]    Script Date: 11/10/2010 11:43:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description:	<Получает остатки по товарам на дату @dendp по отделу @id_otdel.>
-- =============================================
CREATE PROCEDURE [CountRN].[SelectDvigTovarRemains2]
	@dendp datetime =	null,
	@id_otdel int
AS
BEGIN
	SET NOCOUNT ON;

set @dendp = dateadd(hour, 23, @dendp)
set @dendp = dateadd(minute, 59, @dendp)
set @dendp = dateadd(second, 59, @dendp)

    -----date_inv
declare @date_invt datetime ,@date_inv1 datetime,@date_inv datetime, @dend datetime 
declare @pr_kol int --признак как считать количество

--declare @dendp datetime
--set @dendp='2010-06-25'
--------------последняя инвентаризация
--select distinct top 1 @date_invt=dtost,@date_inv=dateadd(dd,1,dtost)
--	from j_tost
--	where dtost= (select max(dtost) from j_tost)
------------ближайшая инвентаризация
select  distinct @date_invt=dtost,@date_inv=dateadd(dd,1,dtost)
from j_tost	
where abs(datediff(dd,dtost,@dendp))=(select min(abs(datediff(dd,dtost,@dendp))) as interv
								 from j_tost	where  promeg=0)
-----------------
---определяем интервал
if @dendp<@date_invt
begin
	set @dend=@date_invt
	set @date_inv=@dendp
	set @pr_kol=1	
end
else
begin
	set @dend=@dendp
	set @pr_kol=2
end
--select @date_invt as date_invt,@date_inv as date_inv,@dendp as dendp,@dend as dend,@pr_kol as pr_kol

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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc)) 
		left outer join j_prihod  as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod
	    left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dprihod>=@date_inv and c.dprihod<=@dend) and de.if_comm=1 and de.ldeyst=1
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_vozvr as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
        left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dprihod>=@date_inv and c.dprihod<=@dend) and de.if_comm=1 and de.ldeyst=1
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))		
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_vozvkass as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
        left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dprihod>=@date_inv and c.dprihod<=@dend) and de.if_comm=1 and de.ldeyst=1 
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_spis as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dprihod>=@date_inv and c.dprihod<=@dend) and de.if_comm=1 and de.ldeyst=1
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))	
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_otgruz as b on b.id_tovar=a.id
		left outer join j_allprihod as c on c.id=b.id_allprihod	
		left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dprihod>=@date_inv and c.dprihod<=@dend) and de.if_comm=1 and de.ldeyst=1
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_realiz as b on b.id_tovar=a.id 
        left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (b.drealiz>=@date_inv and  b.drealiz<=@dend)and de.if_comm=1 and de.ldeyst=1 		  
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
					where s_ntovar.id_tovar= a.id and s_ntovar.tdate_n<=@dend order by tdate_n desc))
		left outer join s_rcena as d on 
		(d.id_tovar=a.id and 
		 d.tdate_n=(select top (1) tdate_n as tdate_n from s_rcena 
					where s_rcena.id_tovar= a.id and s_rcena.tdate_n<=@dend order by tdate_n desc))
		left outer join j_ost  as b on b.id_tovar=a.id
		left outer join j_tost as c on c.id=b.id_tost 
        left outer join departments as de on de.id=a.id_otdel
		where a.id_otdel = @id_otdel and (c.dtost=@date_invt) and de.if_comm=1 and de.ldeyst=1   --- and c.dtost<=@dend)				
		group by a.id,a.ean,a.id_otdel,ltrim(rtrim(an.cname)),an.ntypetovar,a.id_grp1,a.id_grp2,d.rcena) as dvig
	group by id,ean,cname,ntypetovar,id_grp1,id_grp2,rcena	
	order by id,ean,cname,id_grp1,id_grp2
END




