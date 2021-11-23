using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Tienda_CrearDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Logo { get; set; }
    }
}
