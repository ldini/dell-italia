USE [Dall_Italia]
GO

ALTER TABLE [dbo].[MovimientoCajaCierre] ADD [Cheques] [decimal](18, 2) NOT NULL DEFAULT ((0));