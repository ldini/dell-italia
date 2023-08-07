﻿using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Natom.Petshop.Gestion.Backend.Services;
using Natom.Petshop.Gestion.Biz.Exceptions;
using Natom.Petshop.Gestion.Biz.Managers;
using Natom.Petshop.Gestion.Entities.DTO;
using Natom.Petshop.Gestion.Entities.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Natom.Petshop.Gestion.Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportesController : BaseController
    {
        public ReportesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // GET: reportes/listados/ventas-por-producto-proveedor?proveedorId={proveedorId}&productoId={productoId}&desde={desde}&hasta={hasta}
        [HttpGet]
        [ActionName("listados/ventas-por-producto-proveedor")]
        public async Task<IActionResult> GetImprimirComprobanteAsync([FromQuery] string productoId = null, [FromQuery] string proveedorId = null, [FromQuery] string desde = null, [FromQuery] string hasta = null)
        {
            try
            {
                int? _productoId = null;
                int? _proveedorId = null;

                if (!string.IsNullOrEmpty(productoId))
                    _productoId = EncryptionService.Decrypt<int>(productoId);

                if (!string.IsNullOrEmpty(proveedorId))
                    _proveedorId = EncryptionService.Decrypt<int>(proveedorId);

                DateTime _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime _hasta = DateTime.ParseExact(hasta, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataVentasPorProductoProveedorReport(_productoId, _proveedorId, _desde, _hasta);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "VentasPorProductoProveedorReport.rdlc");
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

        // GET: reportes/listados/clientes-que-no-compran-desde-fecha?desde={desde}
        [HttpGet]
        [ActionName("listados/clientes-que-no-compran-desde-fecha")]
        public async Task<IActionResult> GetClientesQueNoCompranDesdeFechaAsync([FromQuery]string desde = null)
        {
            try
            {
                DateTime _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataClientesQueNoCompranDesdeFechaReport(_desde);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "ClientesQueNoCompranDesdeFechaReport.rdlc");
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

        // GET: reportes/estadistica/kilos-comprados-por-cada-proveedor?desde={desde}
        [HttpGet]
        [ActionName("estadistica/kilos-comprados-por-cada-proveedor")]
        public async Task<IActionResult> GetKilosCompradosPorCadaProveedorAsync([FromQuery] string desde = null)
        {
            try
            {
                DateTime? _desde = null;

                if (!string.IsNullOrEmpty(desde))
                    _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataKilosCompradosPorProveedorReport(_desde);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "KilosCompradosPorProveedorReport.rdlc");
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

        // GET: reportes/estadistica/ventas-reparto-vs-mostrador?desde={desde}
        [HttpGet]
        [ActionName("estadistica/ventas-reparto-vs-mostrador")]
        public async Task<IActionResult> GetVentasRepartoVsMostradorAsync([FromQuery] string desde = null)
        {
            try
            {
                DateTime? _desde = null;

                if (!string.IsNullOrEmpty(desde))
                    _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataVentasRepartoVsMostradorReport(_desde);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "VentasRepartoVsMostradorReport.rdlc");
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

        // GET: reportes/estadistica/ventas-reparto-vs-mostrador?desde={desde}
        [HttpGet]
        [ActionName("estadistica/total-ventas-por-lista-de-precios")]
        public async Task<IActionResult> GetTotalVentasPorListaDePreciosAsync([FromQuery] string desde = null)
        {
            try
            {
                DateTime? _desde = null;

                if (!string.IsNullOrEmpty(desde))
                    _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataTotalVendidoPorListaDePreciosReport(_desde);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "TotalVendidoPorListaDePreciosReport.rdlc");
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

        // GET: reportes/estadistica/compras?desde={desde}&hasta={hasta}
        [HttpGet]
        [ActionName("estadistica/compras")]
        public async Task<IActionResult> GetComprasAsync([FromQuery] string desde = null, [FromQuery] string hasta = null)
        {
            try
            {
                DateTime _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime? _hasta = null;

                if (!string.IsNullOrEmpty(hasta))
                    _hasta = DateTime.ParseExact(hasta, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataEstadisticaComprasReport(_desde, _hasta);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "EstadisticaComprasReport.rdlc");
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

        // GET: reportes/estadistica/ganancias?desde={desde}&hasta={hasta}
        [HttpGet]
        [ActionName("estadistica/ganancias")]
        public async Task<IActionResult> GetGananciasAsync([FromQuery] string desde = null, [FromQuery] string hasta = null)
        {
            try
            {
                DateTime _desde = DateTime.ParseExact(desde, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime? _hasta = null;

                if (!string.IsNullOrEmpty(hasta))
                    _hasta = DateTime.ParseExact(hasta, "d/M/yyyy", CultureInfo.InvariantCulture);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataEstadisticaGananciasReport(_desde, _hasta);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "EstadisticaGananciasReport.rdlc");
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

        // GET: reportes/precios/listas/imprimir?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("precios/listas/imprimir")]
        public async Task<IActionResult> GetPreciosListasImprimirAsync([FromQuery] string encryptedId)
        {
            try
            {
                int listaDePreciosId = EncryptionService.Decrypt<int>(encryptedId);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataListaDePreciosReport(listaDePreciosId);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "ListaDePreciosImprimirReport.rdlc");
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

        // GET: reportes/stock/listas/imprimir?encryptedId={encryptedId}
        [HttpGet]
        [ActionName("stock/listas/imprimir")]
        public async Task<IActionResult> GetStockListasImprimirAsync([FromQuery] string encryptedId)
        {
            try
            {
                int? depositoId = EncryptionService.Decrypt<int?>(encryptedId);

                var manager = new ReportingManager(_serviceProvider);
                var data = manager.ObtenerDataListaStockReport(depositoId);

                string mimtype = "";
                int extension = 1;
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Reporting", "ListaStockImprimirReport.rdlc");
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
    }
}
