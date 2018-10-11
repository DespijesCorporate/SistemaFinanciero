namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnualiadadesChange2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_Anualidades",
                c => new
                    {
                        AnualidadId = c.Guid(nullable: false, identity: true),
                        NumeroCuenta = c.String(nullable: false, maxLength: 50),
                        NumeroTarjeta = c.String(nullable: false, maxLength: 50),
                        CIF = c.String(nullable: false, maxLength: 25),
                        Fecha = c.DateTime(nullable: false),
                        Familia = c.String(nullable: false, maxLength: 50),
                        Segmento = c.String(nullable: false, maxLength: 100),
                        SegmentoId = c.String(nullable: false, maxLength: 10, unicode: false),
                        Marca = c.String(nullable: false, maxLength: 50),
                        MarcaId = c.String(nullable: false, maxLength: 10, unicode: false),
                        TipoAnualidad = c.String(nullable: false),
                        TipoAnualidadId = c.String(nullable: false, maxLength: 10, unicode: false),
                        Monto = c.Decimal(nullable: false, storeType: "money"),
                        FechaDeCargo = c.DateTime(nullable: false, storeType: "date"),
                        SaldoActual = c.Decimal(nullable: false, storeType: "money"),
                        Limite = c.Decimal(nullable: false, storeType: "money"),
                        Observacion = c.String(),
                    })
                .PrimaryKey(t => t.AnualidadId);

            CreateTable(
                "dbo.SGRC_ResultadosDeAnualidad",
                c => new
                {
                    ItemDeReclamoId = c.String(nullable: false, maxLength: 128),
                    AnualidadId = c.Guid(nullable: false),
                    ReclamoId = c.String(),
                    ItemDeReclamoDescripcion = c.String(),
                    ResultadoAceptado = c.Boolean(nullable: false),
                    Anualidad_AnualidadId = c.Guid(),
                })
                .PrimaryKey(t => t.ItemDeReclamoId)
                .ForeignKey("dbo.SGRC_Anualidades", t => t.AnualidadId, cascadeDelete: true)
                .Index(t => t.AnualidadId);

            CreateTable(
                "dbo.SGRC_VariablesEvaluadasDeAnualidad",
                c => new
                {
                    AnualidadId = c.Guid(nullable: false),
                    VariableDeItemId = c.Guid(nullable: false),
                    VariableCodigo = c.String(maxLength: 128),
                    CondicionLogica = c.String(),
                    ValorAEvaluar = c.String(),
                    ValorActual = c.String(),
                    EvaluacionCondicion = c.Boolean(nullable: false),
                    ReclamoId = c.String(),
                    ItemDeReclamoId = c.String(),
                    ItemDeReclamoNombre = c.String(),
                    VariableNombre = c.String(),
                    Anualidad_AnualidadId = c.Guid(),
                })
                .PrimaryKey(t => new { t.AnualidadId, t.VariableDeItemId })
                .ForeignKey("dbo.SGRC_Anualidades", t => t.AnualidadId, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_Variables", t => t.VariableCodigo)
                .Index(t => t.AnualidadId)
                .Index(t => t.VariableCodigo);           
        }
        
        public override void Down()
        {            
            DropForeignKey("dbo.SGRC_VariablesEvaluadasDeAnualidad", "VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_VariablesEvaluadasDeAnualidad", "AnualidadId", "dbo.SGRC_Anualidades");            
            DropForeignKey("dbo.SGRC_ResultadosDeAnualidad", "AnualidadId", "dbo.SGRC_Anualidades");
            DropIndex("dbo.SGRC_VariablesEvaluadasDeAnualidad", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_VariablesEvaluadasDeAnualidad", new[] { "AnualidadId" });
            DropIndex("dbo.SGRC_ResultadosDeAnualidad", new[] { "AnualidadId" });
            DropTable("dbo.SGRC_VariablesEvaluadasDeAnualidad");
            DropTable("dbo.SGRC_ResultadosDeAnualidad");
            DropTable("dbo.SGRC_Anualidades");
        }
    }
}
