using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Inventario.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> Enviar(string Destinatario, string Mensaje, string Titulo, bool Html, string Asunto, string Adjuntos = null)
        {
            try
            {
                //Creación del cuerpo
                var builder = new BodyBuilder();
                if (Adjuntos != null)
                {
                    var Arreglo_Adjuntos = Adjuntos.Split(',');
                    foreach (var Adjunto in Arreglo_Adjuntos)
                    {
                        builder.Attachments.Add(Adjunto);
                    }
                }
                if (Html)
                {
                    builder.HtmlBody = Mensaje;
                }
                else
                {
                    builder.TextBody = Mensaje;
                }
                var MiMensaje = new MimeMessage();
                MiMensaje.From.Add(new MailboxAddress(name:"Conteos App", _config.GetSection("Email")["Correo"]));
                MiMensaje.To.Add(new MailboxAddress(Asunto, Destinatario));
                MiMensaje.Subject = Asunto;
                MiMensaje.Body = builder.ToMessageBody();
                using (var Cliente = new SmtpClient())
                {
                    await Cliente.ConnectAsync(_config.GetSection("Email")["Smtp"], Convert.ToInt32(_config.GetSection("Email")["Puerto"]));
                    await Cliente.AuthenticateAsync(_config.GetSection("Email")["Correo"], _config.GetSection("Email")["Contrasena"]);
                    await Cliente.SendAsync(MiMensaje);
                    await Cliente.DisconnectAsync(true);
                    Cliente.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return false;
            }
        }
    }
}
