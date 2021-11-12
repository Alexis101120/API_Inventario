using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;

        public ProductosController(IContenedorTrabajo ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
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
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpGet("{TiendaId:int}")]
        public async Task<IActionResult> Get(int TiendaId)
        {
            try
            {
                var Productos = _ctx.Producto.GetAllasync(x => x.Tienda_Id == TiendaId);
                var ProductosDTO = _mapper.Map<List<ProductoDTO>>(Productos);
                return Ok(new { success = true, data = ProductosDTO });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, data = "Ocurrio un error en el servidor, intentelo de nuevo" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoDTO item)
        {
            if (ModelState.IsValid)
            {
                var Producto = _mapper.Map<T_Producto>(item);
                await _ctx.Producto.AddAsync(Producto);
                await _ctx.SaveAsync();
                return Ok(new { success = true, data = "Producto agregado con éxito" });
            }
            return BadRequest(new { success = false, data = "Error al registrar producto, verifique sus datos" });
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
                return Ok(new { success = true, data = "Producto agregado con éxito" });
            }
            return BadRequest(new { success = false, data = "Error al registrar producto, verifique sus datos" });

        }

    }
}
