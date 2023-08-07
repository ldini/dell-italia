CREATE TRIGGER [dbo].[trg_ActualizarMovimientoCierreFuerte]
ON [dbo].[MovimientoCajaFuerte]
AFTER INSERT
AS
BEGIN
    -- Agrupar los datos por mes y año
    SELECT
        [Mes] = MONTH(i.[FechaHora]),
        [Ano] = YEAR(i.[FechaHora]),
		[Impuestos] = SUM(CASE WHEN i.[Observaciones] = 'Impuestos' THEN i.[Importe] ELSE 0 END),
		[Gastos_Extra] = SUM(CASE WHEN i.[Observaciones] = 'Gastos_Extra' THEN i.[Importe] ELSE 0 END),
        [Sueldos] = SUM(CASE WHEN CHARINDEX('Sueldos', i.[Observaciones]) > 0 THEN i.[Importe] ELSE 0 END)
    INTO #TempData
    FROM inserted i
    GROUP BY MONTH(i.[FechaHora]), YEAR(i.[FechaHora]);

    -- Actualizar o insertar los datos en [MovimientoCajaCierre]
    MERGE [MovimientoCajaCierre] AS ot
    USING #TempData AS i ON ot.[Mes] = i.[Mes] AND ot.[Ano] = i.[Ano]
    WHEN MATCHED THEN
        UPDATE SET
			[Impuestos] += i.[Impuestos],
			[Gastos_Extra] += i.[Gastos_Extra],
			[Sueldos] += i.[Sueldos],
			[Total] += i.[Impuestos] + i.[Gastos_Extra] + i.[Sueldos]
    WHEN NOT MATCHED THEN
        INSERT ([Mes], [Ano], [Impuestos], [Gastos_Extra], [Sueldos],[Total])
        VALUES (i.[Mes], i.[Ano], i.[Impuestos], i.[Gastos_Extra], i.[Sueldos],i.[Impuestos] + i.[Gastos_Extra] + i.[Sueldos]);

    -- Eliminar la tabla temporal
    DROP TABLE #TempData;
END;
