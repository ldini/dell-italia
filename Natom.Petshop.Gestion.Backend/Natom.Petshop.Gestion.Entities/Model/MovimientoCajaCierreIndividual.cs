using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.Model
{
	[Table("MovimientoCajaCierreIndividual")]
	public class MovimientoCajaCierreIndividual
    {
		[Key]
		public int Id { get; set; }
		public int Mes { get; set; }
		public int Ano { get; set; }
		public int dia { get; set; }
		public decimal Registradora1 { get; set; }
		public decimal Registradora2 { get; set; }
		public decimal FacturaA { get; set; }
		public decimal Vales { get; set; }
		public decimal Gastos { get; set; }
		public decimal Compras { get; set; }
		public decimal ComprasA { get; set; }
		public decimal ComprasB { get; set; }
		public decimal Diferencias { get; set; }
		public decimal Efectivo { get; set; }
		[Column("Tarjeta/M.Pago")]
		public decimal Tarjeta_M_Pago { get; set; }
		public decimal Total { get; set; }
		public decimal TurnoN { get; set; }
		public decimal TurnoM { get; set; }
		public DateTime? Cierre_Caja { get; set; }
		public decimal Impuestos { get; set; }
		public decimal Sueldos { get; set; }
		public string? Referencia { get; set; }
		public decimal Gastos_Extra { get; set; }

		public decimal Cheques { get; set; }


	}
}
