namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anulidades : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_Anualidades",
                c => new
                    {
                        AnualidadId = c.Guid(nullable: false, identity: true),
                        NumeroCuenta = c.String(nullable: false, maxLength: 50),
                        CIF = c.String(nullable: false, maxLength: 25),
                        Fecha = c.DateTime(nullable: false),
                        Familia = c.String(nullable: false, maxLength: 50),
                        Segmento = c.String(nullable: false, maxLength: 100),
                        Marca = c.String(nullable: false, maxLength: 50),
                        TipoAnualidad = c.String(nullable: false),
                        Monto = c.Decimal(nullable: false, storeType: "money"),
                        FechaDeCargo = c.DateTime(nullable: false, storeType: "date"),
                        SaldoActual = c.Decimal(nullable: false, storeType: "money"),
                        Limite = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.AnualidadId);
            
            CreateTable(
                "dbo.SGRC_ResultadosAnualidades",
                c => new
                    {
                        ResultadoEvaluacionAnualidadId = c.Int(nullable: false, identity: true),
                        ResultadoDescripcion = c.String(nullable: false),
                        ResultadoAceptado = c.Boolean(nullable: false),
                        AnualidadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ResultadoEvaluacionAnualidadId)
                .ForeignKey("dbo.SGRC_Anualidades", t => t.AnualidadId, cascadeDelete: true)
                .Index(t => t.AnualidadId);
            
            CreateTable(
                "dbo.SGRC_VariablesAnulidades",
                c => new
                    {
                        VariableCodigo = c.String(nullable: false, maxLength: 128),
                        ValorObtenido = c.String(nullable: false),
                        PasoEvaluacion = c.Boolean(nullable: false),
                        AnualidadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.VariableCodigo)
                .ForeignKey("dbo.SGRC_Anualidades", t => t.AnualidadId, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_Variables", t => t.VariableCodigo)
                .Index(t => t.VariableCodigo)
                .Index(t => t.AnualidadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_VariablesAnulidades", "VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_VariablesAnulidades", "AnualidadId", "dbo.SGRC_Anualidades");
            DropForeignKey("dbo.SGRC_ResultadosAnualidades", "AnualidadId", "dbo.SGRC_Anualidades");
            DropIndex("dbo.SGRC_VariablesAnulidades", new[] { "AnualidadId" });
            DropIndex("dbo.SGRC_VariablesAnulidades", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_ResultadosAnualidades", new[] { "AnualidadId" });
            DropTable("dbo.SGRC_VariablesAnulidades");
            DropTable("dbo.SGRC_ResultadosAnualidades");
            DropTable("dbo.SGRC_Anualidades");
        }
    }
}
