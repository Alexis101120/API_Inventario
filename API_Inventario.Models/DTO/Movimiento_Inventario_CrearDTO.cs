using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Movimiento_Inventario_CrearDTO
    {
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fech_Registro { get; set; } = DateTime.Now;
        public int Inventario_Id { get; set; }
    }
}
