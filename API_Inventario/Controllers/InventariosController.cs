using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using API_Inventario.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InventariosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<InventariosController> _logger;
        private readonly IWebHostEnvironment _host;
        private readonly IEmailService _email;

        public InventariosController(IContenedorTrabajo ctx, IMapper mapper, ILogger<InventariosController> logger, IWebHostEnvironment host, IEmailService email)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
            _host = host;
            _email = email;
        }

        [HttpGet("{InventarioId:int}")]
        public async Task<IActionResult> Get(int InventarioId)
        {
            try
            {
                var Inventario = await _ctx.Inventario.GetFirstOrdefaultAsync(x => x.Id == InventarioId, include:source=> source.Include(x=> x.Usuario));
                var InventarioDto = _mapper.Map<InventarioDTO>(Inventario);
                InventarioDto.Usuario = Inventario.Usuario.Nombre_Completo;
                return Ok(new { succesS = true, data = InventarioDto });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("ObtenTodo/{TiendaId:int}")]
        public async Task<IActionResult> ObtenTodo(int TiendaId)
        {
            try
            {
                var Inventarios = await _ctx.Inventario.GetAllasync(x => x.Tienda_id == TiendaId, include: source=> source.Include(x=> x.Usuario));
                var Inventarios_DTO = _mapper.Map<List<InventarioDTO>>(Inventarios);
                return Ok(new { success = true, data = Inventarios_DTO });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("Abrir/{InventarioId:int}")]
        public async Task<IActionResult> Abrir(int InventarioId)
        {
            try
            {
                var Result = await _ctx.Inventario.Abrir(InventarioId);
                await _ctx.SaveAsync();
                if(Result == true)
                {
                    return Ok(new { success = true, mensaje = "Inventario abierto" });
                }
                else
                {
                    return Ok(new { success = false, mensaje = "Ocurrio un error, intentelo de nuevo" });
                }
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("Cerrar/{InventarioId:int}")]
        public async Task<IActionResult> Cerrar(int InventarioId)
        {
            try
            {
                var Result = await _ctx.Inventario.Cerrar(InventarioId);
                await _ctx.SaveAsync();
                if (Result == true)
                {
                    return Ok(new { success = true, mensaje = "Inventario abierto" });
                }
                else
                {
                    return Ok(new { success = false, mensaje = "Ocurrio un error, intentelo de nuevo" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Inventario_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                    var Inventario = _mapper.Map<T_Inventario>(item);
                    Inventario.Usuario_Id = usuarioId;
                    await _ctx.Inventario.AddAsync(Inventario);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Inventario creado con éxito" });
                }
                return Ok(new { success = false, mensaje = "Error al crear inventrario" });
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpPut("InventarioId:int")]
        public async Task<IActionResult> Put(int InventarioId, [FromBody]Inventario_CrearDTO item)
        {
            try
            {
                var Inventario = _mapper.Map<T_Inventario>(item);
                Inventario.Id = InventarioId;
                await _ctx.Inventario.Update(Inventario);
                await _ctx.SaveAsync();
                return Ok(new { success = true, mensaje = "Inventario actualizado con éxito" });
            }catch(Exception ex)
            {
                _logger.LogError($"{ex.Message} => {ex.StackTrace}");
                return StatusCode(500, new { success = false, mensaje = "Error del lado del servidor" });
            }
        }

        [HttpGet("GenerarExcel/{InventarioId:int},{Correo}")]
        public async Task<IActionResult> GenerarExcel(int InventarioId, string Correo)
        {
            try
            {
                var Inventario = await _ctx.Inventario.GetFirstOrdefaultAsync(x => x.Id == InventarioId, include: source=> source.Include(x=> x.Tienda));
                var RutaPrincipal = _host.WebRootPath;
                string nombre = $"{Inventario.Nombre}.xlsx";
                string RutaSalida = Path.Combine(RutaPrincipal, $"/Excel/{nombre}");
                string Folder = Path.Combine(RutaPrincipal, $"/Excel/");
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                var Productos = await _ctx.Producto_Inventario.GetAllasync(x => x.Inventario_Id == InventarioId, include: source => source.Include(x => x.Producto));
                using(MemoryStream ms = new MemoryStream())
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage ep = new ExcelPackage())
                    {
                        Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#98A6A7");
                        ep.Workbook.Worksheets.Add("Conteo_Sistema");
                        ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                        int col_enc=1;

                        ew.Cells[2, col_enc].Value = "Codigo";
                        ew.Cells[2, col_enc].Style.Font.Size = 12;
                        ew.Cells[2, col_enc].AutoFitColumns();
                        ew.Cells[2, col_enc].Style.Font.Bold = true;
                        ew.Cells[2, col_enc].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ew.Cells[2, col_enc].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        ew.Cells[2, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        col_enc++;

                        ew.Cells[2, col_enc].Value = "Descripción";
                        ew.Cells[2, col_enc].Style.Font.Size = 12;
                        ew.Cells[2, col_enc].AutoFitColumns();
                        ew.Cells[2, col_enc].Style.Font.Bold = true;
                        ew.Cells[2, col_enc].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ew.Cells[2, col_enc].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        ew.Cells[2, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        col_enc++;

                        ew.Cells[2, col_enc].Value = "Cantidad";
                        ew.Cells[2, col_enc].Style.Font.Size = 12;
                        ew.Cells[2, col_enc].AutoFitColumns();
                        ew.Cells[2, col_enc].Style.Font.Bold = true;
                        ew.Cells[2, col_enc].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ew.Cells[2, col_enc].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        ew.Cells[2, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        col_enc++;

                        int y = 3;
                       

                        foreach(var ele in Productos)
                        {
                            col_enc = 1;
                            ew.Cells[y, col_enc].Value = ele.Codigo;
                            ew.Cells[y, col_enc].Style.Font.Size = 12;
                            ew.Cells[y, col_enc].AutoFitColumns(15, 30);
                            ew.Cells[y, col_enc].Style.Font.Bold = false;
                            ew.Cells[y, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            col_enc++;

                            ew.Cells[y, col_enc].Value = ele.Producto.Descripcion;
                            ew.Cells[y, col_enc].Style.Font.Size = 12;
                            ew.Cells[y, col_enc].AutoFitColumns(15, 30);
                            ew.Cells[y, col_enc].Style.Font.Bold = false;
                            ew.Cells[y, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            col_enc++;

                            ew.Cells[y, col_enc].Value = ele.Existencia;
                            ew.Cells[y, col_enc].Style.Font.Size = 12;
                            ew.Cells[y, col_enc].AutoFitColumns(15, 30);
                            ew.Cells[y, col_enc].Style.Font.Bold = false;
                            ew.Cells[y, col_enc].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            col_enc++;

                            y++;
                        }
                        await ep.SaveAsAsync(new FileInfo(RutaSalida));

                    }
                }
                var Resp = await _email.Enviar(Correo, $"{Inventario.Nombre} con fecha {Inventario.Fecha.ToShortDateString()} de la tienda {Inventario.Tienda.Nombre}", $"{Inventario.Nombre} con fecha {Inventario.Fecha.ToShortDateString()} de la tienda {Inventario.Tienda.Nombre}"
                    ,false, $"{Inventario.Nombre} con fecha {Inventario.Fecha.ToShortDateString()} de la tienda {Inventario.Tienda.Nombre}",RutaSalida);
                if (Resp)
                {
                    return Ok(new { success = true, mensaje = "Correo enviado con éxito" });
                }
                else
                {
                    return Ok(new { success = false, mensaje = "Ocurrio un error, verifique información e intentelo de nuevo" });
                }
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }


    }
}
