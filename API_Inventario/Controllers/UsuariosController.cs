using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly IMapper _mapper;
        private readonly UserManager<T_Usuario> _userManager;

        public UsuariosController(IContenedorTrabajo ctx, IMapper mapper, UserManager<T_Usuario> userManager)
        {
            _ctx = ctx;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Usuarios = await _ctx.Usuario.GetAllasync();
                var UsuariosDTO = _mapper.Map<UsuarioDTO>(Usuarios);
                return Ok(new { success = true, data = UsuariosDTO });

            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error, revise servidor" });
            }
        }

        [HttpGet("{UsuarioId}")]
        public async Task<IActionResult>Get(string UsuarioId)
        {
            try
            {
                var Usuario = await _ctx.Usuario.GetFirstOrdefaultAsync(x => x.Id == UsuarioId);
                var UsuarioDTO = _mapper.Map<UsuarioDTO>(Usuario);
                return Ok(new { success = true, data = UsuarioDTO });
            }catch(Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error, revise servidor" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario_CrearDTO item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Usuario = _mapper.Map<T_Usuario>(item);
                    var Password = _GetRandomPassword(7);
                    var Result = await _userManager.CreateAsync(Usuario, Password);
                    if (Result.Succeeded)
                    {
                        return Ok(new { success = true, mensaje = "Usuario creado con éxito", pass = Password });
                    }
                    else
                    {
                        return Ok(new { success = false, mensaje = "Error al crear usuario", errores = Result.Errors });
                    }
                }
                var Mensaje = "";
                foreach (var Errores in ModelState.Values)
                {
                    foreach (var Error in Errores.Errors)
                    {
                        Mensaje += Error.ErrorMessage;
                        Mensaje += Environment.NewLine;
                    }
                }
                return BadRequest(new { success = false, mensaje = Mensaje });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, mensaje = "Ocurrio un error, revise servidor" });
            }
        }


        private string _GetRandomPassword(int Length)
        {
            const string charsMay = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string CharnMin = "abcdefghijklmnopqrstuvwxyz";
            const string CharNum = "1234567890";
            const string CharExt = "@#$%";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            int indexM = rnd.Next(charsMay.Length);
            sb.Append(charsMay[indexM]);

            for (int i = 0; i < Length - 3; i++)
            {
                int index = rnd.Next(CharnMin.Length);
                sb.Append(CharnMin[index]);
            }

            int indexNum = rnd.Next(CharNum.Length);
            sb.Append(CharNum[indexNum]);

            int indexExt = rnd.Next(CharExt.Length);
            sb.Append(CharExt[indexExt]);

            return sb.ToString();
        }


    }
}
