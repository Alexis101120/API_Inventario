using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Productos_Excel_DTO
    {
        public int TiendaId { get; set; }
        public IFormFile Excel { get; set; }
    }
}
