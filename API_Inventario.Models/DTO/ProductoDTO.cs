using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class ProductoDTO
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Tienda_Id { get; set; }
    }
}
