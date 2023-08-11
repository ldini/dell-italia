CREATE TRIGGER [dbo].[trg_InsertarMovimientoCierreIndividualFuerte]
ON [dbo].[MovimientoCajaFuerte]
AFTER INSERT
AS
BEGIN
    -- Agrupar los datos por mes y año
    SELECT
        [Mes] = MONTH(i.[FechaHora]),
        [Ano] = YEAR(i.[FechaHora]),
		[dia] = DAY(i.[FechaHora]),
		[Impuestos] = SUM(CASE WHEN i.[Observaciones] = 'Impuestos' THEN i.[Importe] ELSE 0 END),
		[Gastos_Extra] = SUM(CASE WHEN i.[Observaciones] = 'Gastos' THEN i.[Importe] ELSE 0 END),
		[Compras] = SUM(CASE WHEN i.[Observaciones] = 'Compras' THEN i.[Importe] ELSE 0 END),
        [Sueldos] = SUM(CASE WHEN CHARINDEX('Sueldos', i.[Observaciones]) > 0 THEN i.[Importe] ELSE 0 END)
    INTO #TempData
    FROM inserted i
    GROUP BY MONTH(i.[FechaHora]), YEAR(i.[FechaHora]),DAY(i.[FechaHora]);

    -- Actualizar o insertar los datos en [MovimientoCajaCierre]
    MERGE [MovimientoCajaCierreIndividual] AS ot
    USING #TempData AS i ON ot.[Mes] = i.[Mes] AND ot.[Ano] = i.[Ano] AND ot.[dia] = i.[dia]
    WHEN MATCHED THEN
        UPDATE SET
			[Impuestos] += i.[Impuestos],
			[Gastos_Extra] += i.[Gastos_Extra],
			[Sueldos] += i.[Sueldos],
			[Compras] += i.[Compras],
			[Total] += i.[Impuestos] + i.[Gastos_Extra] + i.[Sueldos] + i.[Compras]
    WHEN NOT MATCHED THEN
        INSERT ([Mes], [Ano],[dia], [Impuestos], [Gastos_Extra], [Sueldos],[Compras],[Total])
        VALUES (i.[Mes], i.[Ano],i.[dia], i.[Impuestos], i.[Gastos_Extra], i.[Sueldos],i.[Compras],i.[Impuestos] + i.[Gastos_Extra] + i.[Sueldos] + i.[Compras]);

    -- Eliminar la tabla temporal
    DROP TABLE #TempData;
END;
