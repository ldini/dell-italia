CREATE TRIGGER [dbo].[trg_InsertarMovimientoCierreIndividual]
ON [dbo].[MovimientoCajaDiaria]
AFTER INSERT
AS
BEGIN
    -- Insertar un registro en MovimientoCajaCierreIndividual
    INSERT INTO [dbo].[MovimientoCajaCierreIndividual] (
        [Mes], [Ano],
        [Registradora1], [Registradora2], [FacturaA], [Vales], [Gastos], [Compras], [ComprasA], [ComprasB],
        [Diferencias], [Efectivo], [Tarjeta/M.Pago], [TurnoN], [TurnoM], [Total],
        [Impuestos], [Sueldos], [Referencia], [Gastos_Extra]
    )
    SELECT
        MONTH(i.[FechaHora]) AS [Mes],
        YEAR(i.[FechaHora]) AS [Ano],
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Registradora1' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Registradora2' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'FacturaA' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Vales' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Gastos' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras A' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Compras B' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Diferencias' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Efectivo' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN SUBSTRING(i.[Observaciones], 1, CHARINDEX('//', i.[Observaciones]) - 1) = 'Tarjeta/M.Pago' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN CHARINDEX('//', i.[Observaciones]) > 0 AND i.[Observaciones] LIKE '%// Turno Tarde%' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN CHARINDEX('//', i.[Observaciones]) > 0 AND i.[Observaciones] LIKE '%// Turno Ma%' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN CHARINDEX('//', i.[Observaciones]) > 0 AND i.[Observaciones] NOT LIKE '%// Turno Ma%' AND i.[Observaciones] NOT LIKE '%// Turno Tarde%' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN i.[Observaciones] = 'Impuestos' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN i.[Observaciones] = 'Sueldos' THEN i.[Importe] ELSE 0 END),
        MAX(CASE WHEN ISNULL(NULLIF(i.[Referencia], ''), '') <> '' THEN i.[Referencia] ELSE '' END),
        SUM(CASE WHEN i.[Observaciones] = 'Gastos_Extra' THEN i.[Importe] ELSE 0 END)
    FROM inserted i
    GROUP BY MONTH(i.[FechaHora]), YEAR(i.[FechaHora]);

END;
