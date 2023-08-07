﻿using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;

namespace Natom.Petshop.Gestion.Entities.DTO.Ventas
{
    public class VentaDetalleDTO
    {
        [JsonProperty("encrypted_id")]
        public string EncryptedId { get; set; }

        [JsonProperty("venta_encrypted_id")]
        public string VentaEncryptedId { get; set; }

        [JsonProperty("producto_encrypted_id")]
        public string ProductoEncryptedId { get; set; }

        [JsonProperty("producto_descripcion")]
        public string ProductoDescripcion { get; set; }

        [JsonProperty("producto_peso_gramos")]
        public int? ProductoPesoGramos { get; set; }

        [JsonProperty("pedido_numero")]
        public string PedidoNumero { get; set; }

        [JsonProperty("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonProperty("deposito_encrypted_id")]
        public string DepositoEncryptedId { get; set; }

        [JsonProperty("deposito_descripcion")]
        public string DepositoDescripcion { get; set; }

        [JsonProperty("precio_lista_encrypted_id")]
        public string PrecioListaEncryptedId { get; set; }

        [JsonProperty("precio_descripcion")]
        public string PrecioDescripcion { get; set; }

        [JsonProperty("numero_remito")]
        public string NumeroRemito { get; set; }

        [JsonProperty("precio")]
        public decimal? Precio { get; set; }

        [JsonProperty("pedido_encrypted_id")]
        public string OrdenDePedidoEncryptedId { get; set; }

        [JsonProperty("pedido_detalle_encrypted_id")]
        public string OrdenDePedidoDetalleEncryptedId { get; set; }


        public VentaDetalleDTO From(VentaDetalle entity)
        {
            EncryptedId = EncryptionService.Encrypt(entity.VentaDetalleId);
            VentaEncryptedId = EncryptionService.Encrypt(entity.VentaId);
            ProductoEncryptedId = EncryptionService.Encrypt(entity.ProductoId);
            ProductoDescripcion = entity.Producto?.DescripcionCorta;
            ProductoPesoGramos = entity.PesoUnitarioEnGramos;
            Cantidad = entity.Cantidad;
            DepositoEncryptedId = EncryptionService.Encrypt(entity.DepositoId);
            DepositoDescripcion = entity.Deposito?.Descripcion;
            PedidoNumero = entity.OrdenDePedido?.NumeroPedido.ToString().PadLeft(8, '0');
            PrecioListaEncryptedId = EncryptionService.Encrypt(entity.ListaDePreciosId);
            PrecioDescripcion = entity.ListaDePrecios?.Descripcion ?? "";
            NumeroRemito = entity.NumeroRemito;
            Precio = entity.Precio;
            OrdenDePedidoEncryptedId = EncryptionService.Encrypt(entity.OrdenDePedidoId);
            OrdenDePedidoDetalleEncryptedId = EncryptionService.Encrypt(entity.OrdenDePedidoDetalleId);
            return this;
        }
    }
}