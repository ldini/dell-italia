﻿using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Proveedores
{
    public class ProveedorDTO
    {
		[JsonProperty("encrypted_id")]
		public string EncryptedId { get; set; }

		[JsonProperty("nombre")]
		public string Nombre { get; set; }

		[JsonProperty("apellido")]
		public string Apellido { get; set; }

		[JsonProperty("razonSocial")]
		public string RazonSocial { get; set; }

		[JsonProperty("nombreFantasia")]
		public string NombreFantasia { get; set; }

		[JsonProperty("tipoDocumento_encrypted_id")]
		public string TipoDocumentoEncryptedId { get; set; }

		[JsonProperty("tipoDocumento")]
		public string TipoDocumento { get; set; }

		[JsonProperty("numeroDocumento")]
		public string NumeroDocumento { get; set; }

		[JsonProperty("domicilio")]
		public string Domicilio { get; set; }

		[JsonProperty("entreCalles")]
		public string EntreCalles { get; set; }

		[JsonProperty("localidad")]
		public string Localidad { get; set; }

		[JsonProperty("esEmpresa")]
		public bool EsEmpresa { get; set; }

		[JsonProperty("esPresupuesto")]
		public bool EsPresupuesto { get; set; }

		[JsonProperty("contactoTelefono1")]
		public string ContactoTelefono1 { get; set; }

		[JsonProperty("contactoTelefono2")]
		public string ContactoTelefono2 { get; set; }

		[JsonProperty("contactoEmail1")]
		public string ContactoEmail1 { get; set; }

		[JsonProperty("contactoEmail2")]
		public string ContactoEmail2 { get; set; }

		[JsonProperty("contactoObservaciones")]
		public string ContactoObservaciones { get; set; }

		[JsonProperty("monto_cta_cte")]
		public decimal MontoCtaCte { get; set; }


		[JsonProperty("activo")]
		public bool Activo { get; set; }

		public ProveedorDTO From(Proveedor entity)
		{
			EncryptedId = EncryptionService.Encrypt(entity.ProveedorId);
			Nombre = entity.Nombre;
			Apellido = entity.Apellido;
			RazonSocial = entity.RazonSocial;
			NombreFantasia = entity.NombreFantasia;
			TipoDocumentoEncryptedId = EncryptionService.Encrypt(entity.TipoDocumentoId);
			TipoDocumento = entity.TipoDocumento?.Descripcion;
			NumeroDocumento = entity.NumeroDocumento;
			Domicilio = entity.Domicilio;
			EntreCalles = entity.EntreCalles;
			Localidad = entity.Localidad;
			EsEmpresa = entity.EsEmpresa;
			ContactoTelefono1 = entity.ContactoTelefono1;
			ContactoTelefono2 = entity.ContactoTelefono2;
			ContactoEmail1 = entity.ContactoEmail1;
			ContactoEmail2 = entity.ContactoEmail2;
			ContactoObservaciones = entity.ContactoObservaciones;
			Activo = entity.Activo;
			MontoCtaCte = entity.MontoCtaCte;
			EsPresupuesto = entity.EsPresupuesto;

			return this;
		}
	}
}
