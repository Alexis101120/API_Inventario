using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        IUsuarioRepository Usuario { get; }
        IProductoRepository Producto { get; }
        ITiendaRepository Tienda { get; }
        IInventarioRepository Inventario { get; }
        IMovimientoInventarioRepository Movimiento_Inventario { get; }
        IProductoInventarioRepository Producto_Inventario { get; }
        void Save();
        Task SaveAsync();
    }
}
