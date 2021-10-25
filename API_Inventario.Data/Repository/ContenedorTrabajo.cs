using API_Inventario.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Usuario = new UsuarioReposotory(db);
            Producto = new ProductoRepository(db);
            Tienda = new TiendaRepository(db);
            Inventario = new InventarioRepository(db);
            Movimiento_Inventario = new MovimientoInventarioRepository(_db);
            Producto_Inventario = new ProductoInventarioRepository(_db);
        }

        public IUsuarioRepository Usuario { get; private set; }
        public ITiendaRepository Tienda { get; private set; }
        public IInventarioRepository Inventario { get; private set; }
        public IMovimientoInventarioRepository Movimiento_Inventario { get; private set; }
        public IProductoRepository Producto { get; private set; }
        public IProductoInventarioRepository Producto_Inventario { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
