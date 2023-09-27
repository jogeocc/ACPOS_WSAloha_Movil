using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Mapeo_pagos> Mapeo_Pagos { get; set; }
        public DbSet<Pagos_pendientes> Pagos_pendientes { get; set; }
        public DbSet<Ticket_smart> Ticket_smart { get; set; }
        public DbSet<Producto_Pedido_Espera> Productos_Espera { get; set; }
        public ApplicationDbContext() : base("Conexion")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {

        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

    }
}
