﻿using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Natom.Petshop.Gestion.Backend.Services;
using Natom.Petshop.Gestion.Biz.Exceptions;
using Natom.Petshop.Gestion.Biz.Managers;
using Natom.Petshop.Gestion.Entities.DTO;
using Natom.Petshop.Gestion.Entities.DTO.DataTable;
using Natom.Petshop.Gestion.Entities.DTO.Pedidos;
using Natom.Petshop.Gestion.Entities.DTO.Precios;
using Natom.Petshop.Gestion.Entities.DTO.Stock;
using Natom.Petshop.Gestion.Entities.DTO.Transportes;
using Natom.Petshop.Gestion.Entities.DTO.Zonas;
using Natom.Petshop.Gestion.Entities.Model;
using Natom.Petshop.Gestion.Entities.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PedidosController : BaseController
    {
        public PedidosController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: pedidos/list?status={status}
        [HttpPost]
        [ActionName("list")]
        public async Task<IActionResult> PostListAsync([FromBody] DataTableRequestDTO request, [FromQuery] string status = null, [FromQuery] string zona = null, [FromQuery] string transporte = null)
        {
            try
            {
                int? zonaId = null;
                
                    zonaId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(zona));

                int? transporteId = null;
                if (!string.IsNullOrEmpty(transporte))
                    transporteId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(transporte));

                var manager = new PedidosManager(_serviceProvider);
                var pedidosCount = await manager.ObtenerPedidosCountAsync();
                var pedidos = await manager.ObtenerPedidosDataTableAsync(request.Start, request.Length, request.Search.Value, request.Order.First().ColumnIndex, request.Order.First().Direction, statusFilter: status, zonaId, transporteId);

                return Ok(new ApiResultDTO<DataTableResponseDTO<PedidoListDTO>>
                {
                    Success = true,
                    Data = new DataTableResponseDTO<PedidoListDTO>
                    {
                        RecordsTotal = pedidosCount,
                        RecordsFiltered = pedidos.FirstOrDefault()?.CantidadFiltrados ?? 0,
                        Records = pedidos.Select(pedido => new PedidoListDTO().From(pedido)).ToList()
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/detail?id={encrypted_id}
        [HttpPost]
        [ActionName("detail")]
        public async Task<IActionResult> PostDetailAsync([FromBody] DataTableRequestDTO request, [FromQuery] string id)
        {
            try
            {
                int pedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(id));

                var manager = new PedidosManager(_serviceProvider);
                var pedido = await manager.ObtenerPedidoAsync(pedidoId);

                return Ok(new ApiResultDTO<DataTableResponseDTO<PedidoListDetalleDTO>>
                {
                    Success = true,
                    Data = new DataTableResponseDTO<PedidoListDetalleDTO>
                    {
                        RecordsTotal = pedido.Detalle.Count,
                        RecordsFiltered = pedido.Detalle.Count,
                        Records = pedido.Detalle.Select(item => new PedidoListDetalleDTO().From(item)).ToList()
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/comprobantes/next
        [HttpGet]
        [ActionName("comprobantes/next")]
        public async Task<IActionResult> GetComprobanteNextNumeroAsync()
        {
            try
            {
                var manager = new PedidosManager(_serviceProvider);
                var nextNumero = await manager.ObtenerSiguienteNumeroRemitoAsync();

                return Ok(new ApiResultDTO<string>
                {
                    Success = true,
                    Data = nextNumero
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/basics/data
        // GET: pedidos/basics/data?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("basics/data")]
        public async Task<IActionResult> GetBasicsDataAsync([FromQuery] string encryptedId = null)
        {
            try
            {
                var manager = new PedidosManager(_serviceProvider);
                var numeroPedido = string.Empty;
                PedidoDTO entity = null;

                if (!string.IsNullOrEmpty(encryptedId))
                {
                    var pedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));
                    var pedido = await manager.ObtenerPedidoAsync(pedidoId);
                    entity = new PedidoDTO().From(pedido);
                    numeroPedido = entity.Numero;
                }
                else
                {
                    numeroPedido = (await manager.ObtenerSiguienteNumeroAsync()).ToString().PadLeft(8, '0');
                }

                var stockManager = new StockManager(_serviceProvider);
                var depositos = await stockManager.ObtenerDepositosActivosAsync();

                var preciosManager = new PreciosManager(_serviceProvider);
                var listasDePrecios = await preciosManager.ObtenerListasDePreciosAsync();

                var rangosHorarios = await manager.ObtenerRangosHorariosActivosAsync();


                return Ok(new ApiResultDTO<dynamic>
                {
                    Success = true,
                    Data = new
                    {
                        entity = entity,
                        depositos = depositos.Select(deposito => new DepositoDTO().From(deposito)).ToList(),
                        listasDePrecios = listasDePrecios.Select(lista => new ListaDePreciosDTO().From(lista)),
                        rangos_horarios = rangosHorarios.Select(rango => new RangoHorarioDTO().From(rango)).ToList(),
                        numero_pedido = numeroPedido
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/basics/data_list
        [HttpGet]
        [ActionName("basics/data_list")]
        public async Task<IActionResult> GetBasicsDataListAsync()
        {
            try
            {
                var manager = new ZonasManager(_serviceProvider);
                var zonas = await manager.ObtenerZonasActivasAsync();

                var transporteMgr = new TransportesManager(_serviceProvider);
                var transportes = await transporteMgr.ObtenerTransportesActivasAsync();

                return Ok(new ApiResultDTO<dynamic>
                {
                    Success = true,
                    Data = new
                    {
                        zonas = zonas.Select(zona => new ZonaDTO().From(zona)).ToList(),
                        transportes = transportes.Select(tr => new TransporteDTO().From(tr)).ToList()
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/imprimir/orden?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("imprimir/orden")]
        public async Task<IActionResult> GetImprimirOrdenAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(encryptedId);
                var manager = new PedidosManager(_serviceProvider);

                var data = manager.ObtenerDataOrdenDePedidoReport(ordenDePedidoId);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "OrdenDePedidoReport.rdlc");
                var report = new LocalReport(path);
                report.AddDataSource("DataSet1", data);
                var result = report.Execute(RenderType.Pdf, extension, parameters, mimtype);
                return File(result.MainStream, "application/pdf");
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/imprimir/remito?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("imprimir/remito")]
        public async Task<IActionResult> GetImprimirRemitoAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(encryptedId);
                var manager = new PedidosManager(_serviceProvider);

                var data = manager.ObtenerDataRemitoReport(ordenDePedidoId);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "RemitoReport.rdlc");
                var report = new LocalReport(path);
                report.AddDataSource("DataSet1", data);
                var result = report.Execute(RenderType.Pdf, extension, parameters, mimtype);
                return File(result.MainStream, "application/pdf");
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // GET: pedidos/list_by_cliente
        // GET: pedidos/list_by_cliente?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("list_by_cliente")]
        public async Task<IActionResult> GetListByClienteAsync([FromQuery] string encryptedId)
        {
            try
            {
                var manager = new PedidosManager(_serviceProvider);

                var clienteId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));
                var pedidos = await manager.ObtenerPedidosPendientesDeFacturacionAsync(clienteId);

                return Ok(new ApiResultDTO<dynamic>
                {
                    Success = true,
                    Data = new
                    {
                        listaDeOrdenes = pedidos.Select(p => new PedidoDTO().From(p))
                    }
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/save
        [HttpPost]
        [ActionName("save")]
        public async Task<IActionResult> PostSaveAsync([FromBody] PedidoDTO pedidoDto)
        {
            try
            {
                var manager = new PedidosManager(_serviceProvider);
                var pedido = await manager.GuardarPedidoAsync((int)(_token?.UserId ?? 0), pedidoDto);

                await RegistrarAccionAsync(pedido.OrdenDePedidoId, nameof(OrdenDePedido), string.IsNullOrEmpty(pedidoDto.EncryptedId) ? "Alta" : "Edición");

                return Ok(new ApiResultDTO<PedidoDTO>
                {
                    Success = true,
                    Data = new PedidoDTO().From(pedido)
                });
            }
            catch (HandledException ex)
            {
                return Ok(new ApiResultDTO { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/preparacion/iniciar?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("preparacion/iniciar")]
        public async Task<IActionResult> IniciarPreparacionAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.MarcarInicioPreparacionAsync((int)(_token?.UserId ?? 0), ordenDePedidoId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Inicio de preparación");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/preparacion/cancelar?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("preparacion/cancelar")]
        public async Task<IActionResult> CancelarPreparacionAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.MarcarCancelacionPreparacionAsync((int)(_token?.UserId ?? 0), ordenDePedidoId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Preparación cancelada");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/preparacion/finalizacion?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("preparacion/finalizacion")]
        public async Task<IActionResult> FinalizarPreparacionAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.MarcarFinalizacionPreparacionAsync((int)(_token?.UserId ?? 0), ordenDePedidoId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Preparación completada");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // DELETE: pedidos/anular?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("anular")]
        public async Task<IActionResult> AnularPedidoAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.AnularPedidoAsync((int)(_token?.UserId ?? 0), ordenDePedidoId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Orden anulada");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/despachar?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("despachar")]
        public async Task<IActionResult> MarcarDespachoAsync([FromQuery] string encryptedId, [FromQuery] string transporteId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));
                var _transporteId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(transporteId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.MarcarDespachoAsync((int)(_token?.UserId ?? 0), ordenDePedidoId, _transporteId);

                var transporteMgr = new TransportesManager(_serviceProvider);
                var transporte = await transporteMgr.ObtenerTransporteAsync(_transporteId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), $"Orden despachada /// Transporte '{transporte.Descripcion}'");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/entregado?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("entregado")]
        public async Task<IActionResult> MarcarEntregadoAsync([FromQuery] string encryptedId, [FromBody]List<PedidoListDetalleDTO> detalle)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                var detalleEntrega = detalle
                                        .Select(x => new KeyValuePair<int, decimal>(EncryptionService.Decrypt<int>(x.EncryptedId), x.Entregado.Value))
                                        .ToDictionary(x => x.Key, x => x.Value);
                var conDevoluciones = await manager.MarcarEntregaAsync((int)(_token?.UserId ?? 0), ordenDePedidoId, detalleEntrega);

                string status = "Orden entregada al cliente";
                if (conDevoluciones)
                    status += " - Con devoluciones";

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), status);

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/save_detail?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("save_detail")]
        public async Task<IActionResult> SaveDetailAsync([FromQuery] string encryptedId, [FromBody] List<PedidoListDetalleDTO> detalle)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                var detalleModificacion = detalle
                                        .Select(x => new KeyValuePair<int, decimal>(EncryptionService.Decrypt<int>(x.EncryptedId), x.Cantidad))
                                        .ToDictionary(x => x.Key, x => x.Value);
                await manager.ModificarCantidadesAsync((int)(_token?.UserId ?? 0), ordenDePedidoId, detalleModificacion);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Modificación de cantidades en el pedido");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }

        // POST: pedidos/no_entrega?encryptedId={encryptedId}
        [HttpPost]
        [ActionName("no_entrega")]
        public async Task<IActionResult> MarcarNoEntregaAsync([FromQuery] string encryptedId)
        {
            try
            {
                var ordenDePedidoId = EncryptionService.Decrypt<int>(Uri.UnescapeDataString(encryptedId));

                var manager = new PedidosManager(_serviceProvider);
                await manager.MarcarRegresoPedidoAsync((int)(_token?.UserId ?? 0), ordenDePedidoId);

                await RegistrarAccionAsync(ordenDePedidoId, nameof(OrdenDePedido), "Orden no entregada. Reingreso nuevamente a planta.");

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
                await LoggingService.LogExceptionAsync(_db, ex, usuarioId: (int)(_token?.UserId ?? 0), _userAgent);
                return Ok(new ApiResultDTO { Success = false, Message = "Se ha producido un error interno." });
            }
        }
    }
}
