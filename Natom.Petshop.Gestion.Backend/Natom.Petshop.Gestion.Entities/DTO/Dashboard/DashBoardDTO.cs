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
    public class DashBoardDTO
    {
        public int TotalVentasMensuales { get; set; }
        public int TotalEgresosMensuales { get; set; }
        public int TotalVentasAnuales { get; set; }
        public int TotalEgresosAnuales { get; set; }

        public List<VentasSemanaDTO>? IngresosUltimaSemana { get; set; }
        public List<GastosSemanaDTO>? EgresosUltimaSemana { get; set; }
        public List<CajaGrandeUltimaSemanaDTO>? CajaGrandeUltimaSemana { get; set; }
    }
}
