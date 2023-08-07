using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Entities.DTO.Dashboard
{
    public class CajaGrandeUltimaSemanaDTO
    {
        public string? Fecha { get; set; }
        public int Total { get; set; }
    }
}
