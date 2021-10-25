using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.Entities
{
    public class T_Producto
    {
        [Key]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Tienda_Id { get; set; }
        
        public bool Eliminado { get; set; } = false;

        [ForeignKey("Tienda_Id")]
        public T_Tienda Tienda { get; set; }
    }
}
