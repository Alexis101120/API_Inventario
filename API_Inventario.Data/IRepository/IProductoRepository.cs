using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IProductoRepository : IRepository<T_Producto>
    {
        Task Update(T_Producto item);
    }
}
