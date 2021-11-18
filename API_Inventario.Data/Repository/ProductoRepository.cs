using API_Inventario.Data.IRepository;
using API_Inventario.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.Repository
{
    public class ProductoRepository : Repository<T_Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(T_Producto item)
        {
            var Producto = await _db.T_Productos.FirstOrDefaultAsync(x => x.Codigo == item.Codigo);
            Producto.Descripcion = item.Descripcion;
        }
    }
}
