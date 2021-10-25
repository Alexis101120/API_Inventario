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
    public class TiendaRepository : Repository<T_Tienda>, ITiendaRepository
    {
        private readonly ApplicationDbContext _db;
        public TiendaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public async Task<bool> SoftDelete(int id)
        {
            try
            {
                var Tienda = await _db.T_Tienda.FirstOrDefaultAsync(x => x.Id == id);
                Tienda.Eliminado = true;
                return true;
            }catch(Exception ex)
            {
                return false;
            }
           
        }

        public async Task<bool> Update(T_Tienda item)
        {
            try
            {
                var Tienda = await _db.T_Tienda.FirstOrDefaultAsync(x => x.Id == item.Id);
                Tienda.Nombre = item.Nombre;
                Tienda.Descripcion = item.Descripcion;
                return true;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
    }
}
