using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Existencias_DTO
    {
        public int Inventario_Id { get; set; }
        public IFormFile Excel { get; set; }
    }
}
