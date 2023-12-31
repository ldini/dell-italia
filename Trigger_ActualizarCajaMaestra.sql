USE [Dall_Italia]
GO
/****** Object:  Trigger [dbo].[trg_ActualizarMoviemientoCierre]    Script Date: 21/08/2023 07:33:14 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[trg_ActualizarMoviemientoCierre]
ON [dbo].[MovimientoCajaDiaria]
AFTER INSERT
AS
BEGIN

    SELECT
        [Mes] = MONTH(i.[FechaHora]),
        [Ano] = YEAR(i.[FechaHora]),
        [Registradora1] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Registradora1' THEN i.[Importe] ELSE 0 END),
        [Registradora2] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Registradora2' THEN i.[Importe] ELSE 0 END),
        [FacturaA] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'FacturaA' THEN i.[Importe] ELSE 0 END),
        [Vales] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Vales' THEN i.[Importe] ELSE 0 END),
        [Gastos] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Gastos' THEN i.[Importe] ELSE 0 END),
        [Compras] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras' THEN i.[Importe] ELSE 0 END),
        [ComprasA] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras A' THEN i.[Importe] ELSE 0 END),
        [ComprasB] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras B' THEN i.[Importe] ELSE 0 END),
        [Diferencias] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Diferencias' THEN i.[Importe] ELSE 0 END),
        [Efectivo] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Efectivo' THEN i.[Importe] ELSE 0 END),
		[Cheques] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Cheques' THEN i.[Importe] ELSE 0 END),
        [Tarjeta/M.Pago] = SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Tarjeta/M.Pago' THEN i.[Importe] ELSE 0 END),
        [TurnoM] = (SUM(CASE WHEN CHARINDEX('//', i.[Observaciones]) > 0 AND i.[Observaciones] LIKE '%// Turno Ma%' THEN 1 ELSE 0 END)),
        [TurnoN] = (SUM(CASE WHEN CHARINDEX('//', i.[Observaciones]) > 0 AND i.[Observaciones] LIKE '%// Turno Tarde%' THEN 1 ELSE 0 END))
    INTO #TempData
    FROM inserted i
    GROUP BY MONTH(i.[FechaHora]), YEAR(i.[FechaHora]);

    MERGE [MovimientoCajaCierre] AS ot

    USING #TempData AS i ON ot.[Mes] = i.[Mes] AND ot.[Ano] = i.[Ano]

    WHEN MATCHED THEN

		
        UPDATE SET
            [Registradora1] += i.[Registradora1],
            [Registradora2] += i.[Registradora2],
            [FacturaA] += i.[FacturaA],
            [Vales] += i.[Vales],
            [Gastos] += i.[Gastos],
            [Compras] += i.[Compras],
            [ComprasA] += i.[ComprasA],
            [ComprasB] += i.[ComprasB],
            [Diferencias] += i.[Diferencias],
            [Efectivo] += i.[Efectivo],
			[Cheques] += i.[Cheques],
            [Tarjeta/M.Pago] += i.[Tarjeta/M.Pago],
	        [TurnoM] += CASE i.[TurnoM] WHEN 1 THEN (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Cheques]+i.[Efectivo]+i.[Tarjeta/M.Pago]) ELSE 0 END,
            [TurnoN] += CASE i.[TurnoN] WHEN 1 THEN (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Cheques]+i.[Efectivo]+i.[Tarjeta/M.Pago]) ELSE 0 END,
			[Total] += (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Efectivo]+i.[Cheques]+i.[Tarjeta/M.Pago])
    WHEN NOT MATCHED THEN
		
   INSERT ([Mes], [Ano], [Registradora1], [Registradora2], [FacturaA], [Vales], [Gastos],[Compras], [ComprasA], [ComprasB], [Diferencias], [Efectivo],[Cheques], [Tarjeta/M.Pago], [TurnoM], [TurnoN], [Total], [Cierre_Caja])
  VALUES (i.[Mes], i.[Ano], i.[Registradora1], i.[Registradora2], i.[FacturaA], i.[Vales], i.[Gastos],i.[Compras], i.[ComprasA], i.[ComprasB], i.[Diferencias], i.[Efectivo],i.[Cheques], i.[Tarjeta/M.Pago], 
        CASE i.[TurnoM] WHEN 1 THEN (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Cheques]+i.[Efectivo]+i.[Tarjeta/M.Pago]) ELSE 0 END, 
        CASE i.[TurnoN] WHEN 1 THEN (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Cheques]+i.[Efectivo]+i.[Tarjeta/M.Pago]) ELSE 0 END,
        (i.[Vales]+i.[Gastos]+i.[Compras]+i.[ComprasA]+i.[ComprasB]+i.[Cheques]+i.[Efectivo]+i.[Tarjeta/M.Pago])
        , NULL);
    -- Eliminar la tabla temporal
    DROP TABLE #TempData;
END;
		

