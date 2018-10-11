namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parametros : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_Parametros",
                c => new
                    {
                        ParametroId = c.Int(nullable: false, identity: true),
                        ParametroName = c.String(),
                        TipoId = c.Int(nullable: false),
                        VariableCodigo = c.String(nullable: false, maxLength: 128),
                        Variable_VariableCodigo = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ParametroId)
                .ForeignKey("dbo.SGRC_TiposDeVariables", t => t.TipoId, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_Variables", t => t.Variable_VariableCodigo)
                .ForeignKey("dbo.SGRC_Variables", t => t.VariableCodigo)
                .Index(t => t.TipoId)
                .Index(t => t.VariableCodigo)
                .Index(t => t.Variable_VariableCodigo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_Parametros", "VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_Parametros", "Variable_VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_Parametros", "TipoId", "dbo.SGRC_TiposDeVariables");
            DropIndex("dbo.SGRC_Parametros", new[] { "Variable_VariableCodigo" });
            DropIndex("dbo.SGRC_Parametros", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_Parametros", new[] { "TipoId" });
            DropTable("dbo.SGRC_Parametros");
        }
    }
}
