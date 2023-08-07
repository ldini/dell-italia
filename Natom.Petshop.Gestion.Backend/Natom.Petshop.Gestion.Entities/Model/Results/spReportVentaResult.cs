﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.Model.Results
{
	[Keyless]
	public class spReportVentaResult
    {
		public string LetraComprobante { get; set; }
		public string VentaNumero { get; set; }
		public string NumeroComprobante { get; set; }
		public string Comprobante { get; set; }
		public DateTime FechaHora { get; set; }
		public string Cliente { get; set; }
		public string ClienteDocumento { get; set; }
		public string ClienteDomicilio { get; set; }
		public string ClienteLocalidad { get; set; }
		public decimal ClienteSaldo { get; set; }
		public string Remitos { get; set; }
		public string FacturadoPor { get; set; }
		public string Observaciones { get; set; }
		public string Anulado { get; set; }
		public decimal MontoTotal { get; set; }
		public decimal PesoTotalEnKilogramos { get; set; }
		public string DetalleCodigo { get; set; }
		public string DetalleDescripcion { get; set; }
		public string DetalleRemito { get; set; }
		public decimal DetalleCantidad { get; set; }
		public decimal DetallePesoUnitarioEnKilogramos { get; set; }
		public decimal DetallePrecioUnitario { get; set; }
		public decimal DetallePesoTotalEnKilogramos { get; set; }
		public decimal DetallePrecioTotal { get; set; }
		public string DetalleListaDePrecios { get; set; }
	}
}
