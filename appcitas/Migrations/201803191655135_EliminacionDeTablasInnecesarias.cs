namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminacionDeTablasInnecesarias : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SGRC_Parametros", "TipoId", "dbo.SGRC_TiposDeVariables");
            DropForeignKey("dbo.SGRC_Variables", "VariableTipoDatoRetorno", "dbo.SGRC_TiposDeDatosDeRetorno");
            DropForeignKey("dbo.SGRC_Variables", "VariableOrigen", "dbo.SGRC_TiposDeOrigenesDeVAriables");
            DropForeignKey("dbo.SGRC_Variables", "VariableTipoDeVariable", "dbo.SGRC_TiposDeVariables");
            DropIndex("dbo.SGRC_Parametros", new[] { "TipoId" });
            DropIndex("dbo.SGRC_Variables", new[] { "VariableOrigen" });
            DropIndex("dbo.SGRC_Variables", new[] { "VariableTipoDatoRetorno" });
            DropIndex("dbo.SGRC_Variables", new[] { "VariableTipoDeVariable" });
            AddColumn("dbo.SGRC_Variables", "Origen", c => c.String(nullable: false));
            AddColumn("dbo.SGRC_Variables", "DatoDeRetorno", c => c.String(nullable: false));
            AddColumn("dbo.SGRC_Variables", "TipoDeVariable", c => c.String(nullable: false));
            AlterColumn("dbo.SGRC_Parametros", "TipoId", c => c.String(nullable: false));
            DropColumn("dbo.SGRC_Variables", "VariableOrigen");
            DropColumn("dbo.SGRC_Variables", "VariableTipoDatoRetorno");
            DropColumn("dbo.SGRC_Variables", "VariableTipoDeVariable");
            DropTable("dbo.SGRC_TiposDeDatosDeRetorno");
            DropTable("dbo.SGRC_TiposDeVariables");
            DropTable("dbo.SGRC_TiposDeOrigenesDeVAriables");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SGRC_TiposDeOrigenesDeVAriables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreDeOrigen = c.String(nullable: false, maxLength: 300),
                        DescripcionDeOrigen = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SGRC_TiposDeVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreTipoVariable = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SGRC_TiposDeDatosDeRetorno",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescripcionTipoDatoRetorno = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SGRC_Variables", "VariableTipoDeVariable", c => c.Int(nullable: false));
            AddColumn("dbo.SGRC_Variables", "VariableTipoDatoRetorno", c => c.Int(nullable: false));
            AddColumn("dbo.SGRC_Variables", "VariableOrigen", c => c.Int(nullable: false));
            AlterColumn("dbo.SGRC_Parametros", "TipoId", c => c.Int(nullable: false));
            DropColumn("dbo.SGRC_Variables", "TipoDeVariable");
            DropColumn("dbo.SGRC_Variables", "DatoDeRetorno");
            DropColumn("dbo.SGRC_Variables", "Origen");
            CreateIndex("dbo.SGRC_Variables", "VariableTipoDeVariable");
            CreateIndex("dbo.SGRC_Variables", "VariableTipoDatoRetorno");
            CreateIndex("dbo.SGRC_Variables", "VariableOrigen");
            CreateIndex("dbo.SGRC_Parametros", "TipoId");
            AddForeignKey("dbo.SGRC_Variables", "VariableTipoDeVariable", "dbo.SGRC_TiposDeVariables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SGRC_Variables", "VariableOrigen", "dbo.SGRC_TiposDeOrigenesDeVAriables", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SGRC_Variables", "VariableTipoDatoRetorno", "dbo.SGRC_TiposDeDatosDeRetorno", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SGRC_Parametros", "TipoId", "dbo.SGRC_TiposDeVariables", "Id", cascadeDelete: true);
        }
    }
}
