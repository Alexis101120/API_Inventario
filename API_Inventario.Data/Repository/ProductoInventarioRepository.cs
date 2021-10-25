using API_Inventario.Data.IRepository;
using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.Repository
{
    public class ProductoInventarioRepository : Repository<T_Producto_Inventario>, IProductoInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
