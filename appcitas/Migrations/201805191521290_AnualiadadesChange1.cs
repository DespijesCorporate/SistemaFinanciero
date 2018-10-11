namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnualiadadesChange1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SGRC_ResultadosAnualidades", "AnualidadId", "dbo.SGRC_Anualidades");
            DropIndex("dbo.SGRC_ResultadosAnualidades", new[] { "AnualidadId" });
            DropTable("dbo.SGRC_Anualidades");
            DropTable("dbo.SGRC_ResultadosAnualidades");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SGRC_ResultadosAnualidades",
                c => new
                    {
                        ResultadoEvaluacionAnualidadId = c.Int(nullable: false, identity: true),
                        ResultadoDescripcion = c.String(nullable: false),
                        ResultadoAceptado = c.Boolean(nullable: false),
                        AnualidadId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ResultadoEvaluacionAnualidadId);
            
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
                    })
                .PrimaryKey(t => t.AnualidadId);
            
            CreateIndex("dbo.SGRC_ResultadosAnualidades", "AnualidadId");
            AddForeignKey("dbo.SGRC_ResultadosAnualidades", "AnualidadId", "dbo.SGRC_Anualidades", "AnualidadId", cascadeDelete: true);
        }
    }
}
