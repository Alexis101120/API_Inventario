using API_Inventario.Data.IRepository;
using API_Inventario.Models.DTO;
using API_Inventario.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly IContenedorTrabajo _ctx;
        private readonly SignInManager<T_Usuario> _SignManager;
        private readonly ILogger<AccesoController> _logger;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _Configuration;


        public AccesoController(IContenedorTrabajo ctx, SignInManager<T_Usuario> signInManager, Microsoft.Extensions.Configuration.IConfiguration configuration, ILogger<AccesoController> logger)
        {
            _ctx = ctx;
            _SignManager = signInManager;
            _Configuration = configuration;
            _logger = logger;
        }


        [HttpPost("LogIn")]
        public async Task<ActionResult<Respuesta_Autenticacion>> LogIn(Credenciales_Usuario_DTO item)
        {

            var resultado = await _SignManager.PasswordSignInAsync(item.UserName,
              item.Password, isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return ConstruirToken(item);
            }
            else
            {
                return BadRequest( new { error = resultado });
            }
        }

        private ActionResult<Respuesta_Autenticacion> ConstruirToken(Credenciales_Usuario_DTO item)
        {
            var Usuario = _ctx.Usuario.GetFirstOrdefault(x => x.UserName == item.UserName && x.Acceso == item.Password);
            var claims = new List<Claim>()
            {
                new Claim("tipo", Usuario.Tipo_Usuario)
            };
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Usuario.Id));
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expricacion = DateTime.UtcNow.AddHours(24);
            var SecurityToken = new JwtSecurityToken(issuer: null, audience: null, claims, expires: expricacion, signingCredentials: creds);
            var Token = new Respuesta_Autenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken),
                Expiracion = expricacion,
                Tipo_Usuario = Usuario.Tipo_Usuario,
                UserName = Usuario.UserName,
                Nombre = Usuario.Nombre_Completo
            };
            _ctx.Usuario.UpdateKey(Usuario.Id, Token.Token, Token.Expiracion);
            _ctx.Save();
            return Token;
        }


    }
}
