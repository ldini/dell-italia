﻿using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Clientes.CtaCte
{
    public class ClienteCtaCteMovimientoDTO
    {
        [JsonProperty("encrypted_id")]
        public string EncryptedId { get; set; }

        [JsonProperty("fechaHora")]
        public DateTime FechaHora { get; set; }

        [JsonProperty("usuarioNombre")]
        public string UsuarioNombre { get; set; }

        [JsonProperty("encrypted_cliente_id")]
        public string EncryptedClienteId { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("observaciones")]
        public string Observaciones { get; set; }

        public ClienteCtaCteMovimientoDTO From(MovimientoCtaCteCliente entity)
        {
            EncryptedId = EncryptionService.Encrypt(entity.MovimientoCtaCteClienteId);
            FechaHora = entity.FechaHora;
            UsuarioNombre = entity.Usuario?.Nombre ?? "Admin";
            Tipo = entity.Tipo.Equals("C") ? "Ingreso" : "Egreso";
            Importe = entity.Importe;
            Observaciones = entity.Observaciones;
            EncryptedClienteId = EncryptionService.Encrypt(entity.ClienteId);

            return this;
        }
    }
}
