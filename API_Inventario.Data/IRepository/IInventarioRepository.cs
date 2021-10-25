using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IInventarioRepository : IRepository<T_Inventario>
    {
        Task<bool> Eliminar(int id);
        Task<bool> Abrir(int id);
        Task<bool> Cerrar(int id);
    }
}
