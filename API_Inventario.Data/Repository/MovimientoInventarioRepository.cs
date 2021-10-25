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
    public class MovimientoInventarioRepository : Repository<T_Movimientos_Inventario>, IMovimientoInventarioRepository
    {
        private readonly ApplicationDbContext _db;
        public MovimientoInventarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<bool> Registrar_Movimiento(T_Movimientos_Inventario item)
        {
            using (var Transaccion = _db.Database.BeginTransaction())
            {
                try
                {
                    //Paso registrar movimiento
                    await _db.T_Movimientos_Inventarios.AddAsync(item);

                    //Validar existencia producto
                    var Producto = await _db.T_Productos_Inventario.FirstOrDefaultAsync(x => x.Inventario_Id == item.Inventario_Id && x.Codigo == item.Codigo);
                    if( Producto == null )
                    {
                        var Existencia = new T_Producto_Inventario
                        {
                            Codigo = item.Codigo,
                            Inventario_Id = item.Inventario_Id,
                            Existencia = item.Cantidad,
                        };
                        _db.T_Productos_Inventario.Add(Existencia);
                    }
                    else
                    {
                        Producto.Existencia += item.Cantidad;
                    }
                    await _db.SaveChangesAsync();
                    Transaccion.Commit();
                    return true;
                    
                
                }catch(Exception ex)
                {
                    Transaccion.Rollback();
                return false;
                }
            }
        }

        public async Task<bool> SoftDelete(int id)
        {
            using (var Transaccion = _db.Database.BeginTransaction())
            {
                try
                 {
               
                    var Movimiento = await _db.T_Movimientos_Inventarios.FirstOrDefaultAsync(x => x.Id == id);
                    Movimiento.Eliminado = true;

                    var Producto = await _db.T_Productos_Inventario.FirstOrDefaultAsync(x => x.Inventario_Id == Movimiento.Inventario_Id && x.Codigo == Movimiento.Codigo);
                    Producto.Existencia -= Movimiento.Cantidad;
                    await _db.SaveChangesAsync();
                    Transaccion.Commit();
                    return true;
               
                } catch (Exception ex)
                {
                    Transaccion.Rollback();
                    return false;
                }
            }
        }

        public async Task<bool> Update(T_Movimientos_Inventario item)
        {
            using(var Transaccion = _db.Database.BeginTransaction())
            {
                try
                {
                    var Movimiento = await _db.T_Movimientos_Inventarios.FirstOrDefaultAsync(x => x.Id == item.Id);
                    Movimiento.Cantidad = item.Cantidad;
                    Movimiento.Codigo = item.Codigo;

                    var Producto = await _db.T_Productos_Inventario.FirstOrDefaultAsync(x => x.Inventario_Id == Movimiento.Inventario_Id && x.Codigo == Movimiento.Codigo);
                    Producto.Existencia -= Movimiento.Cantidad;
                    Producto.Existencia += item.Cantidad;

                    await _db.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
