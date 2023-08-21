using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Cajas
{
    public class MovimientoCajaCierreIndividualDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Mes")]
        public int Mes { get; set; }
        [JsonProperty("Ano")]
        public int dia { get; set; }
        [JsonProperty("dia")]
        public int Ano { get; set; }
        [JsonProperty("Registradora1")]
        public decimal Registradora1 { get; set; }
        [JsonProperty("Registradora2")]
        public decimal Registradora2 { get; set; }
        [JsonProperty("FacturaA")]
        public decimal FacturaA { get; set; }
        [JsonProperty("Vales")]
        public decimal Vales { get; set; }
        [JsonProperty("Gastos")]
        public decimal Gastos { get; set; }
        [JsonProperty("Compras")]
        public decimal Compras { get; set; }
        [JsonProperty("ComprasA")]
        public decimal ComprasA { get; set; }
        [JsonProperty("ComprasB")]
        public decimal ComprasB { get; set; }
        [JsonProperty("Diferencias")]
        public decimal Diferencias { get; set; }
        [JsonProperty("Efectivo")]
        public decimal Efectivo { get; set; }
        [JsonProperty("Tarjeta_M_Pago")]
        public decimal Tarjeta_M_Pago { get; set; }
        [JsonProperty("Total")]
        public decimal Total { get; set; }
        [JsonProperty("TurnoM")]
        public decimal TurnoM { get; set; }
        [JsonProperty("TurnoN")]
        public decimal TurnoN { get; set; }
        [JsonProperty("Cierre_Caja")]
        public DateTime Cierre_Caja { get; set; }
        [JsonProperty("Impuestos")]
        public decimal Impuestos { get; private set; }
        [JsonProperty("Sueldos")]
        public decimal Sueldos { get; private set; }
        [JsonProperty("Referencia")]
        public string Referencia { get; private set; }
        [JsonProperty("Gastos_Extra")]
        public decimal Gastos_Extra { get; private set; }
        [JsonProperty("Cheques")]
        public decimal Cheques { get; private set; }

        public MovimientoCajaCierreIndividualDTO From(MovimientoCajaCierreIndividual entity)
        {
            Id = entity.Id;
            Mes = entity.Mes;
            Ano = entity.Ano;
            dia = entity.dia;
            Registradora1 = entity.Registradora1;
            Registradora2 = entity.Registradora2;
            FacturaA = entity.FacturaA;
            Vales = entity.Vales;
            Gastos = entity.Gastos;
            Compras = entity.Compras;
            ComprasA = entity.ComprasA;
            ComprasB = entity.ComprasB;
            Diferencias = entity.Diferencias;
            Efectivo = entity.Efectivo;
            Tarjeta_M_Pago = entity.Tarjeta_M_Pago;
            Total = entity.Total;
            TurnoN = entity.TurnoN;
            TurnoM = entity.TurnoM;
            Cierre_Caja = entity.Cierre_Caja.GetValueOrDefault();
            Impuestos = entity.Impuestos;
            Sueldos = entity.Sueldos;
            Referencia = entity.Referencia;
            Gastos_Extra = entity.Gastos_Extra;

            return this;
        }
    }
}
