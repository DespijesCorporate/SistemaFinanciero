namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluacionAnualidadChange1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SGRC_VariablesAnulidades", "AnualidadId", "dbo.SGRC_Anualidades");
            DropForeignKey("dbo.SGRC_VariablesAnulidades", "VariableCodigo", "dbo.SGRC_Variables");
            DropIndex("dbo.SGRC_VariablesAnulidades", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_VariablesAnulidades", new[] { "AnualidadId" });
            DropTable("dbo.SGRC_VariablesAnulidades");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SGRC_VariablesAnulidades",
                c => new
                    {
                        VariableCodigo = c.String(nullable: false, maxLength: 128),
                        ValorObtenido = c.String(nullable: false),
                        PasoEvaluacion = c.Boolean(nullable: false),
                        AnualidadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.VariableCodigo);
            
            CreateIndex("dbo.SGRC_VariablesAnulidades", "AnualidadId");
            CreateIndex("dbo.SGRC_VariablesAnulidades", "VariableCodigo");
            AddForeignKey("dbo.SGRC_VariablesAnulidades", "VariableCodigo", "dbo.SGRC_Variables", "VariableCodigo");
            AddForeignKey("dbo.SGRC_VariablesAnulidades", "AnualidadId", "dbo.SGRC_Anualidades", "AnualidadId", cascadeDelete: true);
        }
    }
}
