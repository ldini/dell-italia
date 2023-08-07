using Natom.Petshop.Gestion.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Backend.Services
{
    public interface IDashBoardRepositorio
    {
        Task<int> TotalVentasMensuales();
        Task<int> TotalEgresosMensuales();
        Task<int> TotalVentasAnuales();
        Task<int> TotalEgresosAnuales();
        Task<Dictionary<string, int>> VentasUltimaSemana();
        Task<Dictionary<string, int>> EgresosUltimaSemana();

    }
}
