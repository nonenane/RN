CREATE TABLE [CountRN].[j_tSaveRN](
	[id] [int] IDENTITY(1,1) NOT NULL,	
	[DateStart] [date] NOT NULL,
	[DateEnd] [date] NULL,
	[isOptOtgruz] bit not null,
	[isOnlyShipped] bit not null,
	[isInventorySpis] bit not null,
	[TotalPrihod] [numeric](16, 2) NOT NULL,
	[TotalRealiz] [numeric](16, 2) NOT NULL,
	[TotalRestStart] [numeric](16, 2) NOT NULL,
	[TotalRestStop] [numeric](16, 2) NOT NULL,
	[TotalRN] [numeric](16, 2) NOT NULL,
	[TotalPercentRN] [numeric](16, 2) NOT NULL,		
	[id_Creator] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
 CONSTRAINT [PK_j_tSaveRN] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [CountRN].[j_tSaveRN]  WITH CHECK ADD  CONSTRAINT [FK_j_tSaveRN_id_Creator] FOREIGN KEY([id_Creator])
REFERENCES [dbo].[ListUsers] ([id])
GO




CREATE TABLE [CountRN].[j_SaveRN](
	[id] [int] IDENTITY(1,1) NOT NULL,	
	[id_tSaveRN] int not null,
	[id_tovar] int not null,
	[id_department] int not null,
	[id_grp1] int not null,
	[id_grp2] int not null,
	[RestStart] [numeric](16, 2) NOT NULL,
	[RestStartSum] [numeric](16, 2) NOT NULL,
	[BalanceStop] [numeric](16, 2) NOT NULL,
	[BalanceStopSum] [numeric](16, 2) NOT NULL,
	[Prihod] [numeric](16, 2) NOT NULL,
	[PrihodSum] [numeric](16, 2) NOT NULL,
	[Otgruz] [numeric](16, 2) NOT NULL,
	[OtgruzSum] [numeric](16, 2) NOT NULL,
	[Vozvr] [numeric](16, 2) NOT NULL,
	[VozvrSum] [numeric](16, 2) NOT NULL,
	[Spis] [numeric](16, 2) NOT NULL,
	[SpisSum] [numeric](16, 2) NOT NULL,
	[InventSpis] [numeric](16, 2) NOT NULL,
	[InventSpisSum] [numeric](16, 2) NOT NULL,
	[Realiz] [numeric](16, 2) NOT NULL,
	[RealizSum] [numeric](16, 2) NOT NULL,
	[OtgruzOpt] [numeric](16, 2) NOT NULL,
	[OtgruzOptSum] [numeric](16, 2) NOT NULL,
	[VozvrKass] [numeric](16, 2) NOT NULL,
	[VozvrKassSum] [numeric](16, 2) NOT NULL,
 CONSTRAINT [PK_j_SaveRN] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = ON, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [CountRN].[j_SaveRN]  WITH CHECK ADD  CONSTRAINT [FK_j_SaveRN_id_tSaveRN] FOREIGN KEY([id_tSaveRN])
REFERENCES [CountRN].[j_tSaveRN] ([id])
GO