using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Respuesta_Autenticacion
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public string Tipo_Usuario { get; set; }
        public string UserName { get; set; }
        public string Nombre { get; set; }
    }
}
