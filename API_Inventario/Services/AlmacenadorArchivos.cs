using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Services
{
    public class AlmacenadorArchivos : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment _host;
        private readonly IHttpContextAccessor _httcontextAccesor;
        private readonly ILogger<AlmacenadorArchivos> _logger;

        public AlmacenadorArchivos(IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, ILogger<AlmacenadorArchivos> logger)
        {
            _logger = logger;
            _host = host;
            _httcontextAccesor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(_host.WebRootPath, contenedor, nombreArchivo);
                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }
            return Task.FromResult(0);
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            try
            {
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                string folder = Path.Combine(_host.WebRootPath, contenedor);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string ruta = Path.Combine(folder, nombreArchivo);
                await File.WriteAllBytesAsync(ruta, contenido);
                var urlActual = $"{_httcontextAccesor.HttpContext.Request.Scheme}://{_httcontextAccesor.HttpContext.Request.Host}";
                var urlParaBD = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");
                return urlParaBD;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace + ex.InnerException);
                return null;
            }
        }
    }
}
