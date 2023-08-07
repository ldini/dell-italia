﻿using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Pedidos
{
    public class PedidoListDetalleDTO
    {
        [JsonProperty("encrypted_id")]
        public string EncryptedId { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("deposito")]
        public string Deposito { get; set; }

        [JsonProperty("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonProperty("entregado")]
        public decimal? Entregado { get; set; }

        public PedidoListDetalleDTO From(OrdenDePedidoDetalle entity)
        {
            EncryptedId = EncryptionService.Encrypt(entity.OrdenDePedidoDetalleId);
            Codigo = entity.Producto.Codigo;
            Descripcion = entity.Producto.DescripcionCorta;
            Deposito = entity.Deposito.Descripcion;
            Cantidad = entity.Cantidad;
            Entregado = entity.CantidadEntregada ?? entity.Cantidad;

            return this;
        }
    }
}
