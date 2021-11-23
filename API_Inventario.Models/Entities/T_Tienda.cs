using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.Entities
{
    public class T_Tienda
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Logo_url { get; set; }
        public bool Eliminado { get; set; } = false;
    }
}
