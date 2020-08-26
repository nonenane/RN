USE [dbase1]
GO
/****** Object:  StoredProcedure [CountRN].[GetVozvratKass]    Script Date: 11/10/2010 11:43:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Kondratyeva N.>
-- Create date: <09.11.2010>
-- Description:	<Вычисляет возврат с касс по отделу @id_otdel за период с @date_start по @date_finish и НДС 18% и 10%.>
-- =============================================
CREATE PROCEDURE [CountRN].[GetVozvratKass]
	@id_otdel int,
	@date_start datetime,
	@date_finish datetime
AS
BEGIN
	SET NOCOUNT ON;

    declare vcurs cursor for
		select id_tovar, (0 - sum(netto*rcena)) as vkass
		from j_vozvkass join j_allprihod on j_vozvkass.id_allprihod = j_allprihod.id
			            join s_tovar on j_vozvkass.id_tovar = s_tovar.id
		where dprihod >= @date_start and dprihod <= @date_finish and s_tovar.id_otdel = @id_otdel
		group by id_tovar

	declare @id_tovar int, @vkass numeric(13,4), @vkass18 numeric(13,4), @vkass10 numeric(13,4), @vkassSum numeric(13,4)
	set @vkass18 = 0
	set @vkass10 = 0
	set @vkassSum = 0
	open vcurs
	fetch next from vcurs into @id_tovar, @vkass
	while @@fetch_status = 0
	begin
		declare @nds numeric(2,0)
		select @nds = s_nds.nds
		from s_tovar join s_nds on s_tovar.id_nds = s_nds.id
		where s_tovar.id = @id_tovar
		if @nds = 18
			set @vkass18 = @vkass18 + @vkass
		if @nds = 10
			set @vkass10 = @vkass10 + @vkass
		set @vkassSum = @vkassSum + @vkass
		fetch next from vcurs into @id_tovar, @vkass
	end
	close vcurs
	deallocate vcurs

	select @vkassSum as vkass, @vkass18 as vkass18, @vkass10 as vkass10
END

--exec CountRN.GetVozvratKass 1, '2010-11-02', '2010-11-07'


