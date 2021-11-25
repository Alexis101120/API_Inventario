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
    public class InventarioRepository : Repository<T_Inventario>, IInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public InventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> Abrir(int id)
        {
            try
            {
                var Inventario = await _db.T_Inventarios.FirstOrDefaultAsync(x => x.Id == id);
                Inventario.Activo = true;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Cerrar(int id)
        {
            try
            {
                var Inventario = await _db.T_Inventarios.FirstOrDefaultAsync(x => x.Id == id);
                Inventario.Activo = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var Inventario = await _db.T_Inventarios.FirstOrDefaultAsync(x => x.Id == id);
                Inventario.Eliminado = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Update(T_Inventario item)
        {
            try
            {
                var Inventario = await _db.T_Inventarios.FirstOrDefaultAsync(x => x.Id == item.Id);
                Inventario.Fecha = item.Fecha;
                Inventario.Nombre = item.Nombre;
                Inventario.Activo = item.Activo;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
