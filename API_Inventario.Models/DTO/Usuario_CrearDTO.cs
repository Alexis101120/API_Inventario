﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Models.DTO
{
    public class Usuario_CrearDTO
    {
        [Required]
        public string Nombres { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellito_Materno { get; set; }
        public string Nombre_Completo { get; set; }
        public string Tipo_Usuario { get; set; }
        public string Acceso { get; set; }
    }
}
