CREATE TRIGGER [dbo].[trg_InsertarMovimientoCierreIndividualFuerte]
ON [dbo].[MovimientoCajaFuerte]
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
		0,0,0,0,0,0,0,0,0,0,0,0,0,0,
		SUM(CASE WHEN i.[Observaciones] = 'Impuestos' THEN i.[Importe] ELSE 0 END),
        SUM(CASE WHEN i.[Observaciones] = 'Sueldos' THEN i.[Importe] ELSE 0 END),
        MAX(CASE WHEN ISNULL(NULLIF(i.[Referencia], ''), '') <> '' THEN i.[Referencia] ELSE '' END),
        SUM(CASE WHEN i.[Observaciones] = 'Gastos_Extra' THEN i.[Importe] ELSE 0 END)
    FROM inserted i
    GROUP BY MONTH(i.[FechaHora]), YEAR(i.[FechaHora]);

END;
