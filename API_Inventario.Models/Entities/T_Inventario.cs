using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.Entities
{
    public class T_Inventario
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
        public string Usuario_Id { get; set; }
        public int Tienda_id { get; set; }
        public bool Eliminado { get; set; } = false;


        [ForeignKey("Usuario_Id")]
        public T_Usuario Usuario { get; set; }

        [ForeignKey("Tienda_id")]
        public T_Tienda Tienda { get; set; }
    }
}
