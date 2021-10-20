using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class InventarioDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
        public string Usuario_Id { get; set; }
        public string Tienda_id { get; set; }
    }
}
