using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models
{
    public class T_Inventario
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
        public string Usuario_Id { get; set; }
        public string Tienda_id { get; set; }

        [ForeignKey("Usuario_Id")]
        public T_Usuario Usuario { get; set; }
        [ForeignKey("Tienda_id")]
        public T_Tienda Tienda { get; set; }
    }
}
