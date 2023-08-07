﻿using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Productos
{
    public class ProductoListDTO
    {
        [JsonProperty("encrypted_id")]
        public string EncryptedId { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; }

        [JsonProperty("peso_unitario_gramos")]
        public int PesoUnitarioGramos { get; set; }

        [JsonProperty("activo")]
        public bool Activo { get; set; }

        public ProductoListDTO From(Producto entity)
        {
            EncryptedId = EncryptionService.Encrypt(entity.ProductoId);
            Codigo = entity.Codigo;
            Descripcion = entity.DescripcionCorta;
            Marca = entity.Marca?.Descripcion;
            Activo = entity.Activo;
            Categoria = entity.CategoriaProducto?.Descripcion;
            PesoUnitarioGramos = Convert.ToInt32(entity.PesoUnitario * entity.UnidadPeso.Gramos);

            return this;
        }
    }
}
