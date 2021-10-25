using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IMovimientoInventarioRepository : IRepository<T_Movimientos_Inventario>
    {
        Task<bool> Registrar_Movimiento(T_Movimientos_Inventario item);
        Task<bool> Update(T_Movimientos_Inventario item);
        Task<bool> SoftDelete(int id);
    }
}
