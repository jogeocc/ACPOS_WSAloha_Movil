using AlohaWebServiceMobile.EntityFrameWork.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlohaWebServiceMobile.EntityFrameWork.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Mapeo_pagos> Mapeo_Pagos { get; set; }

        public ApplicationDbContext() : base("Conexion")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,Migrations.Configuration>());
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
