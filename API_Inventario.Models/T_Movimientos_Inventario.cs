using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models
{
    public class T_Movimientos_Inventario
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fech_Registro { get; set; } = DateTime.Now;
        public string Usuario_Id { get; set; }
        public int Inventario_Id { get; set; }

        [ForeignKey("Usuario_Id")]
        public T_Usuario Usuario { get; set; }
        [ForeignKey("Inventario_Id")]
        public T_Inventario Inventario { get; set; }
    }
}
