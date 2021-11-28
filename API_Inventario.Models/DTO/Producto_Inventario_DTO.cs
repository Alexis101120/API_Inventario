using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Producto_Inventario_DTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Inventario_Id { get; set; }
        public int Existencia { get; set; }
        public string Descripcion { get; set; }
    }
}
