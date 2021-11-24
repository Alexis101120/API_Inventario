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
    public class ProductoInventarioRepository : Repository<T_Producto_Inventario>, IProductoInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductoInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Update(T_Producto_Inventario item, int conteo)
        {
            var Inventario = await _db.T_Productos_Inventario.FirstOrDefaultAsync(x => x.Codigo == item.Codigo && x.Inventario_Id == item.Inventario_Id);
            Inventario.Existencia += conteo;
        }
    }
}
