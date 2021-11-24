using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class MovimientoInventarioDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fech_Registro { get; set; }
        public string Usuario_Id { get; set; }
        public int Inventario_Id { get; set; }
    }
}
