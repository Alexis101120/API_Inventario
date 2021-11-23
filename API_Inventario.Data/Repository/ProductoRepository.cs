using API_Inventario.Data.IRepository;
using API_Inventario.Models.Entities;
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

        public Task<bool> SoftDelete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T_Producto item)
        {
            throw new NotImplementedException();
        }
    }
}
