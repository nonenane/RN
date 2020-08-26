USE [dbase1]
GO
/****** Object:  UserDefinedFunction [CountRN].[GetTovarRemainsSum]    Script Date: 11/10/2010 11:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO











-- =============================================
-- Author:  <Kondratyeva N.>
-- Create date: <02.11.2010>
-- Description: <Вычисляет сумму остатка в закуп. цене для товара @id_tovar на дату @date и по кол-ву остатка @remain.>
-- =============================================
CREATE FUNCTION  [CountRN].[GetTovarRemainsSum]
(
	@id_tovar int,
	@date datetime,
	@remain numeric(13,3)
)
RETURNS numeric(13,4)
AS
BEGIN
 DECLARE @sum numeric(13,4), @minus bit

 if @remain is NULL or @remain = 0
			return 0
 set @minus = 0
 if @remain < 0
	begin
		set @remain = 0 - @remain
		set @minus = 1
	end
		set @date = dateadd(hour, 23, @date)
		set @date = dateadd(minute, 59, @date)
		set @date = dateadd(second, 59, @date)
		if (exists(select zcena, netto
		from j_prihod inner join j_allprihod on j_prihod.id_allprihod = j_allprihod.id
		where j_prihod.id_tovar = @id_tovar and j_allprihod.dprihod <= @date))
			begin
				declare cur cursor for
				select zcena, netto
				from j_prihod inner join j_allprihod on j_prihod.id_allprihod = j_allprihod.id
				where j_prihod.id_tovar = @id_tovar and j_allprihod.dprihod <= @date
				order by dprihod desc, zcena
				
				open cur

				declare @zcena numeric(13,4), @netto numeric(13,3)
				set @sum = 0.0
				fetch next from cur into @zcena, @netto

				while (@netto < @remain and @@fetch_status = 0)
				begin
					set @sum = round(@sum,2) + @netto*round(@zcena,2)
					set @remain = @remain - @netto
					fetch next from cur into @zcena, @netto
				end
				
				if (@@fetch_status != 0 and @netto < @remain)
					begin
						if (exists(select Price from Remains.j_ProcurementPrices where id_tovar = @id_tovar and DateIn <= @date))
							begin
								declare @price1 numeric(13,4)
								select @price1 = Price
								from Remains.j_ProcurementPrices
								where DateIn<@date and id_tovar=@id_tovar and 
								DateIn = (select max(DateIn)from Remains.j_ProcurementPrices where id_tovar = @id_tovar and DateIn < @date)
								set @sum = round(@sum,2) + @remain*round(@price1,2)
							end
					end
				else
					set @sum = round(@sum,2) + @remain*round(@zcena,2)

			close cur
			deallocate cur
			end
		else
			begin
				if (exists(select Price from Remains.j_ProcurementPrices where id_tovar = @id_tovar and DateIn <= @date))
				begin
					declare @price numeric(13,4)
					select @price = Price
					from Remains.j_ProcurementPrices
					where DateIn<@date and id_tovar=@id_tovar and 
					DateIn = (select max(DateIn)from Remains.j_ProcurementPrices where id_tovar = @id_tovar and DateIn < @date)
					set @sum = @remain*round(@price,2)
				end
				else
					set @sum = 0
			end

if @minus = 1
	set @sum = 0 - @sum

 RETURN round(@sum,2)
END











