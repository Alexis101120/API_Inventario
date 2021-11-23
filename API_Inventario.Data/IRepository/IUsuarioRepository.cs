using API_Inventario.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data.IRepository
{
    public interface IUsuarioRepository : IRepository<T_Usuario>
    {
        Task<bool> UpdateKey(string UsuarioId, string Key, DateTime Expiration);
        Task<bool> Bloquear(string UsuarioId);
        Task<bool> Desbloquear(string UsuarioId);
    }
}
