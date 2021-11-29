using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Services
{
    public interface IEmailService
    {
        Task<bool> Enviar(string Destinatario, string Mensaje, string Titulo, bool Html, string Asunto, string Adjuntos = null);
    }
}
