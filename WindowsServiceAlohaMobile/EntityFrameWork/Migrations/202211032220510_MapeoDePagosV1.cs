namespace WindowsServiceAlohaMobile.EntityFrameWork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MapeoDePagosV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mapeo_pagos",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    id_pago_aloha = c.Int(nullable: false),
                    clv_mapeo_pago = c.String(),
                    nombre_pago_aloha = c.String(),
                    status = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropTable("dbo.Mapeo_pagos");
        }
    }
}
