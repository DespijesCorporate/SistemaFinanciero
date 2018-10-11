namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfiguracionesMotor1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_ItemsDeReclamos",
                c => new
                    {
                        ReclamoId = c.String(nullable: false, maxLength: 128),
                        ItemDeReclamoId = c.String(nullable: false, maxLength: 128),
                        ItemDeReclamoDescripcion = c.String(),
                    })
                .PrimaryKey(t => new { t.ReclamoId, t.ItemDeReclamoId })
                .ForeignKey("dbo.SGRC_Reclamos", t => t.ReclamoId, cascadeDelete: true)
                .Index(t => t.ReclamoId);
            
            CreateTable(
                "dbo.SGRC_Reclamos",
                c => new
                    {
                        ReclamoId = c.String(nullable: false, maxLength: 128),
                        ReclamoNombre = c.String(nullable: false),
                        ReclamoDescripcion = c.String(),
                    })
                .PrimaryKey(t => t.ReclamoId);
            
            CreateTable(
                "dbo.SGRC_VariablesDeItems",
                c => new
                    {
                        VariableCodigo = c.String(nullable: false, maxLength: 128),
                        CondicionLogica = c.String(maxLength: 5),
                        ValorAEvaluar = c.String(nullable: false),
                        ReclamoId = c.String(nullable: false, maxLength: 128),
                        ItemDeReclamoId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.VariableCodigo)
                .ForeignKey("dbo.SGRC_ItemsDeReclamos", t => new { t.ReclamoId, t.ItemDeReclamoId }, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_Variables", t => t.VariableCodigo)
                .Index(t => t.VariableCodigo)
                .Index(t => new { t.ReclamoId, t.ItemDeReclamoId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_VariablesDeItems", "VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_VariablesDeItems", new[] { "ReclamoId", "ItemDeReclamoId" }, "dbo.SGRC_ItemsDeReclamos");
            DropForeignKey("dbo.SGRC_ItemsDeReclamos", "ReclamoId", "dbo.SGRC_Reclamos");
            DropIndex("dbo.SGRC_VariablesDeItems", new[] { "ReclamoId", "ItemDeReclamoId" });
            DropIndex("dbo.SGRC_VariablesDeItems", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_ItemsDeReclamos", new[] { "ReclamoId" });
            DropTable("dbo.SGRC_VariablesDeItems");
            DropTable("dbo.SGRC_Reclamos");
            DropTable("dbo.SGRC_ItemsDeReclamos");
        }
    }
}
