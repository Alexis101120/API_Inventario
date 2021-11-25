using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _host;

        public ProductosController(IContenedorTrabajo ctx, IMapper mapper, IWebHostEnvironment host)
        {
            _ctx = ctx;
            _mapper = mapper;
            _host = host;
        }

        [HttpGet("{Codigo}")]
        public async Task<IActionResult> Get(string Codigo)
        {
            try
            {
                var Producto = await _ctx.Producto.GetFirstOrdefaultAsync(x => x.Codigo == Codigo);
                var ProductoDTO = _mapper.Map<ProductoDTO>(Producto);
                return Ok(new { success = true, data = ProductoDTO });
            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpGet("{TiendaId:int}")]
        public async Task<IActionResult> Get(int TiendaId)
        {
            try
            {
                var Productos = await _ctx.Producto.GetAllasync(x => x.Tienda_Id == TiendaId);
                var ProductosDTO = _mapper.Map<List<ProductoDTO>>(Productos);
                return Ok(new { success = true, data = ProductosDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Producto = _mapper.Map<T_Producto>(item);
                    await _ctx.Producto.AddAsync(Producto);
                    await _ctx.SaveAsync();
                    return Ok(new { success = true, mensaje = "Producto agregado con éxito" });
                }
                return BadRequest(new { success = false, mensaje = "Error al registrar producto, verifique sus datos" });
            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPost("LeerExcel")]
        public async Task<IActionResult> Post([FromForm] Productos_Excel_DTO item)
        {
            try
            {
                if (item.Excel != null)
                {
                    var Ruta_Principal = _host.WebRootPath;
                    string Ruta_Temporales = Path.Combine(Ruta_Principal, @"Temporales\");
                    string Nombre_Archivo = item.Excel.FileName;
                    if (!Directory.Exists(Ruta_Temporales)) Directory.CreateDirectory(Ruta_Temporales);
                    using (var filestram = new FileStream(Path.Combine(Ruta_Temporales, Nombre_Archivo), FileMode.Create))
                    {
                        await item.Excel.CopyToAsync(filestram);
                    }
                    FileInfo file = new FileInfo(Path.Combine(Ruta_Temporales, Nombre_Archivo));
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            return NotFound(new { codigo = "404", mensaje = "Error al cargar datos, verifique su archivo" });
                        }
                        else
                        {
                            var Numero_Filas = worksheet.Dimension.Rows;
                            var Productos = new List<T_Producto>();
                            for(int Fila = 2; Fila<=Numero_Filas; Fila++)
                            {
                                var Producto = new T_Producto
                                {
                                    Codigo = Convert.ToString(worksheet.Cells[Fila, 1].Value.ToString().Trim()),
                                    Descripcion = Convert.ToString(worksheet.Cells[Fila, 2].Value.ToString().Trim()),
                                    Tienda_Id = item.TiendaId
                                };
                                Productos.Add(Producto);
                            }

                            _ctx.Producto.BulkInsert(Productos);
                            return Ok(new { success = true, mensaje = "Productos agregados con éxito" });
                        }
                    }
                }
                return Ok(new { success = true, mensaje = "No hay excel" });
                }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPut("{Codigo}")]
        public async Task<IActionResult> Put(string Codigo, [FromBody] ProductoDTO item)
            {
                if (ModelState.IsValid)
                {
                    var Producto = _mapper.Map<T_Producto>(item);
                    Producto.Codigo = Codigo;
                    await _ctx.Producto.Update(Producto);
                    await _ctx.SaveAsync();
                return Ok(new { success = true, mensaje = "Producto agregado con éxito" });
            }
            return BadRequest(new { success = false, mensaje = "Error al registrar producto, verifique sus datos" });

        }

    }
}
