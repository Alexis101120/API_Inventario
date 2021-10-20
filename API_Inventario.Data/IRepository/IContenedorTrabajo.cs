using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {

        void Save();
        Task SaveAsync();
    }
}
