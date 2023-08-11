using Microsoft.AspNetCore.Mvc;
using Natom.Petshop.Gestion.Backend.Services;
using Natom.Petshop.Gestion.Biz.Exceptions;
using Natom.Petshop.Gestion.Biz.Managers;
using Natom.Petshop.Gestion.Entities.DTO;
using Natom.Petshop.Gestion.Entities.DTO.Dashboard;
using Natom.Petshop.Gestion.Entities.DTO.Cajas;
using Natom.Petshop.Gestion.Entities.DTO.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Natom.Petshop.Gestion.Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CajasController : BaseController
    {
        private readonly IDashBoardRepositorio _dashboardRepositorio;
        public CajasController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: cajas/diaria/list?filterDate={filterDate}
        [HttpPost]
        [ActionName("diaria/list")]
        public async Task<IActionResult> PostMovimientosCajaDiariaListAsync([FromBody] DataTableRequestDTO request, [FromQuery]string filterDate = null)
        {
            try
            {
                DateTime dt;
                DateTime? dtFilter = null;
                if (DateTime.TryParse(filterDate, out dt))
                    dtFilter = dt;

                var manager = new CajasManager(_serviceProvider);
                var movimientosCount = await manager.ObtenerMovimientosCajaDiariaCountAsync();
                var movimientos = await manager.ObtenerMovimientosCajaDiariaDataTableAsync(request.Start, request.Length, request.Search.Value, request.Order.First().ColumnIndex, request.Order.First().Direction, dtFilter);
                var saldoActual = await manager.ObtenerSaldoActualCajaDiariaAsync(dtFilter);

                return Ok(new ApiResultDTO<DataTableResponseDTO<MovimientoCajaDiariaDTO>>
                {
                    Success = true,
                    Data = new DataTableResponseDTO<MovimientoCajaDiariaDTO>
                    {
                        RecordsTotal = movimientosCount,
                        RecordsFiltered = movimientos.FirstOrDefault()?.CantidadFiltrados ?? 0,
                        Records = movimientos.Select(movimiento => new MovimientoCajaDiariaDTO().From(movimiento)).ToList(),
                        ExtraData = new
                        {
                            saldoActual = saldoActual
                        }
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        [HttpGet]
        [ActionName("diaria/report")]
        public async Task<IActionResult> PostMovimientosCajaDiariaReportAsync([FromQuery] string encryptedId = null)
        {
            try
            {
                DateTime dt;
                DateTime? dtFilter = null;
              //  if (DateTime.TryParse(filterDate, out dt))
                   // dtFilter = dt;

                var manager = new CajasManager(_serviceProvider);
                var movimientosCount = await manager.ObtenerMovimientosCajaDiariaCountAsync();
             //   var movimientos = await manager.ObtenerMovimientosCajaDiariaDataTableAsync(request.Start, request.Length, request.Search.Value, request.Order.First().ColumnIndex, request.Order.First().Direction, dtFilter);
                var saldoActual = await manager.ObtenerSaldoActualCajaDiariaAsync(dtFilter);

                return Ok(new ApiResultDTO<DataTableResponseDTO<MovimientoCajaDiariaDTO>>
                {
                    Success = true,
                    Data = new DataTableResponseDTO<MovimientoCajaDiariaDTO>
                    {
                        RecordsTotal = movimientosCount,
                    //    RecordsFiltered = movimientos.FirstOrDefault()?.CantidadFiltrados ?? 0,
                  //      Records = movimientos.Select(movimiento => new MovimientoCajaDiariaDTO().From(movimiento)).ToList(),
                        ExtraData = new
                        {
                            saldoActual = saldoActual
                        }
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: diaria/dashboard
        [HttpGet]
        [ActionName("diaria/dashboard")]
        public async Task<IActionResult> GetMovimientosCajaDiariaReport2Async()
        {
            Response<DashBoardDTO> _response = new Response<DashBoardDTO>();
            var manager = new CajasManager(_serviceProvider);
            try
            {
                DashBoardDTO vmDashboard = new DashBoardDTO();

                vmDashboard.TotalVentasMensuales  = await manager.TotalVentasMensuales();
                vmDashboard.TotalEgresosMensuales = await manager.TotalEgresosMensuales();
                vmDashboard.TotalVentasAnuales    = await manager.TotalVentasAnuales();
                vmDashboard.TotalEgresosAnuales   = await manager.TotalEgresosAnuales();

                List<VentasSemanaDTO> listaVentasSemana                   = new List<VentasSemanaDTO>();
                List<GastosSemanaDTO> listaGastosSemana                   = new List<GastosSemanaDTO>();
                List<CajaGrandeUltimaSemanaDTO> CajaGrandeUltimaSemanaDTO = new List<CajaGrandeUltimaSemanaDTO>();

                foreach (KeyValuePair<string, int> item in await manager.VentasUltimaSemana())
                {
                    listaVentasSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashboard.IngresosUltimaSemana = listaVentasSemana;

                    foreach (KeyValuePair<string, int> item in await manager.EgresosUltimaSemana())
                    {
                        listaGastosSemana.Add(new GastosSemanaDTO()
                        {
                            Fecha = item.Key,
                            Total = item.Value
                         });
                    }
                vmDashboard.EgresosUltimaSemana = listaGastosSemana;

                    foreach (KeyValuePair<string, int> item in await manager.CajaFuerteUltimaSemana())
                    {
                    CajaGrandeUltimaSemanaDTO.Add(new CajaGrandeUltimaSemanaDTO()
                        {
                            Fecha = item.Key,
                            Total = item.Value
                        });
                    }
                vmDashboard.CajaGrandeUltimaSemana = CajaGrandeUltimaSemanaDTO;

                _response = new Response<DashBoardDTO>() { status = true, msg = "ok", value = vmDashboard };
                return StatusCode(StatusCodes.Status200OK, _response);
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: cajas/diaria/save
        [HttpPost]
        [ActionName("diaria/save")]
        public async Task<IActionResult> PostMovimientoCajaDiariaSaveAsync([FromBody] MovimientoCajaDiariaDTO user)
        {
            try
            {
                var manager = new CajasManager(_serviceProvider);
                var movimiento = await manager.GuardarMovimientoCajaDiariaAsync(user, (int)(_token?.UserId ?? 0));

                return Ok(new ApiResultDTO<MovimientoCajaDiariaDTO>
                {
                    Success = true,
                    Data = new MovimientoCajaDiariaDTO().From(movimiento)
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: cajas/fuerte/list?filterDate={filterDate}
        [HttpPost]
        [ActionName("fuerte/list")]
        public async Task<IActionResult> PostMovimientosCajaFuerteListAsync([FromBody] DataTableRequestDTO request, [FromQuery] string filterDate = null)
        {
            try
            {
                DateTime dt;
                DateTime? dtFilter = null;
                if (DateTime.TryParse(filterDate, out dt))
                    dtFilter = dt;

                var manager = new CajasManager(_serviceProvider);
                var movimientosCount = await manager.ObtenerMovimientosCajaFuerteCountAsync();
                var movimientos = await manager.ObtenerMovimientosCajaFuerteDataTableAsync(request.Start, request.Length, request.Search.Value, request.Order.First().ColumnIndex, request.Order.First().Direction, dtFilter);
                var saldoActual = await manager.ObtenerSaldoActualCajaFuerteAsync(dtFilter);

                return Ok(new ApiResultDTO<DataTableResponseDTO<MovimientoCajaFuerteDTO>>
                {
                    Success = true,
                    Data = new DataTableResponseDTO<MovimientoCajaFuerteDTO>
                    {
                        RecordsTotal = movimientosCount,
                        RecordsFiltered = movimientos.FirstOrDefault()?.CantidadFiltrados ?? 0,
                        Records = movimientos.Select(movimiento => new MovimientoCajaFuerteDTO().From(movimiento)).ToList(),
                        ExtraData = new
                        {
                            saldoActual = saldoActual
                        }
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: cajas/fuerte/save
        [HttpPost]
        [ActionName("fuerte/save")]
        public async Task<IActionResult> PostMovimientoCajaFuerteSaveAsync([FromBody] MovimientoCajaFuerteDTO user)
        {
            try
            {
                var manager = new CajasManager(_serviceProvider);
                var movimiento = await manager.GuardarMovimientoCajaFuerteAsync(user, (int)(_token?.UserId ?? 0));

                return Ok(new ApiResultDTO<MovimientoCajaFuerteDTO>
                {
                    Success = true,
                    Data = new MovimientoCajaFuerteDTO().From(movimiento)
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: cajas/transfer
        [HttpPost]
        [ActionName("transfer")]
        public async Task<IActionResult> PostMovimientoEntreCajasSaveAsync([FromBody] MovimientoEntreCajasDTO movimientoDto)
        {
            try
            {
                var manager = new CajasManager(_serviceProvider);
                await manager.GuardarMovimientoEntreCajasAsync(movimientoDto, (int)(_token?.UserId ?? 0));

                return Ok(new ApiResultDTO
                {
                    Success = true
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }


        // GET: cajas/cierre
        [HttpGet]
        [ActionName("cierre/list")]
        public async Task<IActionResult> GetMovimientosCajaCierreAsync()
        {
            try
            {
               
                var manager = new CajasManager(_serviceProvider);
               
                var movimientosCierre = await manager.ObtenerMovimientosCajaCierreAsync();
                var movimientosCierreDTO = movimientosCierre.Select(mc => new MovimientoCajaCierreDTO().From(mc)).ToList();


                return Ok(new ApiResultDTO<List<MovimientoCajaCierreDTO>>
                {
                    Success = true,
                    Data = movimientosCierreDTO
                });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: cajas/cierre
        [HttpGet]
        [ActionName("cierre/list/all")]
        public async Task<IActionResult> GetMovimientosCajaCierreIndividualAsync(int? mes = null, int? ano = null)
        {
            try
            {
                var manager = new CajasManager(_serviceProvider);

                var movimientosCierre = await manager.ObtenerMovimientosCajaCierreIndividualAsync(mes, ano); // Pasa los parámetros mes y ano al método del manager
                var movimientosCierreDTO = movimientosCierre.Select(mc => new MovimientoCajaCierreIndividualDTO().From(mc)).ToList();

                return Ok(new ApiResultDTO<List<MovimientoCajaCierreIndividualDTO>>
                {
                    Success = true,
                    Data = movimientosCierreDTO
                });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }



        [HttpPost]
        [ActionName("cierre/cierre_caja")]
        public async Task<IActionResult> UpdateCierreCajaAsync([FromBody] MovimientoCajaCierreDTO cierreCajaDTO)
        {
            try
            {
                
                var manager = new CajasManager(_serviceProvider);

                var movimiento = await manager.ObtenerMovimientoCajaCierrePorIdAsync(cierreCajaDTO.Id);

                if (movimiento == null)
                {
                    return NotFound(); // Devolver un 404 si no se encuentra el movimiento
                }

                if (movimiento.Cierre_Caja == null) {
                    movimiento.Cierre_Caja = DateTime.Now;
                    await manager.ActualizarMovimientoCajaCierreAsync(movimiento);
                }


                return Ok(new ApiResultDTO { Success = true, Message = "Cierre de caja actualizado correctamente." });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int?)_token?.UserId, _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }
    }
}
