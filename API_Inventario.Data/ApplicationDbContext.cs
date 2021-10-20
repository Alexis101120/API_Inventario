using API_Inventario.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Inventario.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        //Entidades
        public DbSet<T_Usuario> T_Usuarios { get; set; }
        public DbSet<T_Tienda> T_Tienda { get; set; }
        public DbSet<T_Producto> T_Productos { get; set; }
        public DbSet<T_Inventario> T_Inventarios { get; set; }
        public DbSet<T_Movimientos_Inventario> T_Movimientos_Inventarios { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<T_Tienda>().HasQueryFilter(x => x.Eliminado == false);
            builder.Entity<T_Producto>().HasQueryFilter(x => x.Eliminado == false);
            builder.Entity<T_Inventario>().HasQueryFilter(x => x.Eliminado == false);
            builder.Entity<T_Movimientos_Inventario>().HasQueryFilter(x => x.Eliminado == false);
            base.OnModelCreating(builder);
        }
    }
}
