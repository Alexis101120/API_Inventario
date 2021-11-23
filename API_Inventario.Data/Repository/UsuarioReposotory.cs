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
    public class UsuarioReposotory : Repository<T_Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        public UsuarioReposotory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<bool> Bloquear(string UsuarioId)
        {
            try
            {
                var Usuario = await _db.T_Usuarios.FirstOrDefaultAsync(x => x.Id == UsuarioId);
                Usuario.LockoutEnd = DateTime.Now.AddYears(100);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> Desbloquear(string UsuarioId)
        {
            try
            {
                var Usuario = await _db.T_Usuarios.FirstOrDefaultAsync(x => x.Id == UsuarioId);
                Usuario.LockoutEnd = DateTime.Now.AddYears(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateKey(string UsuarioId, string Key, DateTime Expiration)
        {
            try
            {

                var Usuario = await _db.T_Usuarios.FirstOrDefaultAsync(x => x.Id == UsuarioId);
                Usuario.Key = Key;
                Usuario.Expiration = Expiration;
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
