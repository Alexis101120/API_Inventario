using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.Entities
{
    public class T_Producto_Inventario
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Inventario_Id { get; set; }
        public int Existencia { get; set; }
        public bool Eliminado { get; set; }

        [ForeignKey("Codigo")]
        public T_Producto Producto { get; set; }
        [ForeignKey("Inventario_Id")]
        public T_Inventario Inventario { get; set; }
    }
}
