USE [Dall_Italia]
GO

/****** Object:  Table [dbo].[MovimientoCajaCierre]    Script Date: 07/08/2023 08:28:00 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MovimientoCajaCierre](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Mes] [int] NOT NULL,
	[Ano] [int] NOT NULL,
	[Registradora1] [decimal](18, 2) NOT NULL,
	[Registradora2] [decimal](18, 2) NOT NULL,
	[FacturaA] [decimal](18, 2) NOT NULL,
	[Vales] [decimal](18, 2) NOT NULL,
	[Gastos] [decimal](18, 2) NOT NULL,
	[Compras] [decimal](18, 2) NOT NULL,
	[ComprasA] [decimal](18, 2) NOT NULL,
	[ComprasB] [decimal](18, 2) NOT NULL,
	[Diferencias] [decimal](18, 2) NOT NULL,
	[Efectivo] [decimal](18, 2) NOT NULL,
	[Tarjeta/M.Pago] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[TurnoN] [decimal](18, 2) NOT NULL,
	[TurnoM] [decimal](18, 2) NOT NULL,
	[Cierre_Caja] [datetime] NULL,
	[Impuestos] [decimal](18, 2) NOT NULL,
	[Sueldos] [decimal](18, 2) NOT NULL,
	[Referencia] [varchar](100) NULL,
	[Gastos_Extra] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_MovimientoCajaCierre] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Registradora1]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Registradora2]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [FacturaA]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Vales]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Gastos]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Compras]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [ComprasA]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [ComprasB]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Diferencias]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Efectivo]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Tarjeta/M.Pago]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Total]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [TurnoN]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [TurnoM]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Impuestos]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Sueldos]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT (NULL) FOR [Referencia]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD  DEFAULT ((0)) FOR [Gastos_Extra]
GO


