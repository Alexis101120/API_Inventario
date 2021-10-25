using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface ITiendaRepository : IRepository<T_Tienda>
    {
        Task<bool> Update(T_Tienda item);
        Task<bool> SoftDelete(int id);
    }
}
