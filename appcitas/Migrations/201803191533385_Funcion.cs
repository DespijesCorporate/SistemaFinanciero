namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Funcion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_Funciones",
                c => new
                    {
                        FuncionCodigo = c.String(nullable: false, maxLength: 128),
                        FuncionNombre = c.String(nullable: false, maxLength: 300),
                        FuncionNumeroParametros = c.Int(nullable: false),
                        FuncionTipoDeRetorno = c.Int(nullable: false),
                        FuncionDescripcion = c.String(),
                        FuncionFormula = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FuncionCodigo);
            
            AddColumn("dbo.SGRC_Parametros", "FuncionCodigo", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.SGRC_Parametros", "FuncionCodigo");
            AddForeignKey("dbo.SGRC_Parametros", "FuncionCodigo", "dbo.SGRC_Funciones", "FuncionCodigo", cascadeDelete: true);
            DropColumn("dbo.SGRC_Parametros", "TipoParametro");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGRC_Parametros", "TipoParametro", c => c.Int(nullable: false));
            DropForeignKey("dbo.SGRC_Parametros", "FuncionCodigo", "dbo.SGRC_Funciones");
            DropIndex("dbo.SGRC_Parametros", new[] { "FuncionCodigo" });
            DropColumn("dbo.SGRC_Parametros", "FuncionCodigo");
            DropTable("dbo.SGRC_Funciones");
        }
    }
}
