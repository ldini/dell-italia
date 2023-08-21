export class MovimientoCajaMaestraIndividualDTO {
    public Id: number;
    public dia:number;
    public Mes: number;
    public Ano: number;
    public Registradora1: number;
    public Registradora2: number;
    public FacturaA: number;
    public Vales: number;
    public Gastos: number;
    public Compras: number;
    public ComprasA: number;
    public ComprasB: number;
    public Diferencias: number;
    public Efectivo: number;
    public Tarjeta_M_Pago: number;
    public Total: number;
    public TurnoN: number;
    public TurnoM: number;
    public Cierre_Caja: Date;
    public Impuestos:number;
    public Sueldos:number;
    public Referencia:string;
    public Gastos_Extra:number;
    public Cheques:number;
}
