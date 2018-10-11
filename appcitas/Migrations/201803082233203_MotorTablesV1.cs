namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MotorTablesV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SGRC_TiposDeDatosDeRetorno",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescripcionTipoDatoRetorno = c.String(nullable: false),
                        AssemblyTipoDatoRetorno = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.SGRC_Variables",
                c => new
                    {
                        VariableCodigo = c.String(nullable: false, maxLength: 128),
                        VariableNombre = c.String(nullable: false, maxLength: 200),
                        VariableOrigen = c.Int(nullable: false),
                        VariableTipoDatoRetorno = c.Int(nullable: false),
                        VariableTipoDeVariable = c.Int(nullable: false),
                        VariableDescripcion = c.String(),
                    })
                .PrimaryKey(t => t.VariableCodigo)
                .ForeignKey("dbo.SGRC_TiposDeDatosDeRetorno", t => t.VariableTipoDatoRetorno, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_TiposDeOrigenesDeVAriables", t => t.VariableOrigen, cascadeDelete: true)
                .ForeignKey("dbo.SGRC_TiposDeVariables", t => t.VariableTipoDeVariable, cascadeDelete: true)
                .Index(t => t.VariableOrigen)
                .Index(t => t.VariableTipoDatoRetorno)
                .Index(t => t.VariableTipoDeVariable);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_Variables", "VariableTipoDeVariable", "dbo.SGRC_TiposDeVariables");
            DropForeignKey("dbo.SGRC_Variables", "VariableOrigen", "dbo.SGRC_TiposDeOrigenesDeVAriables");
            DropForeignKey("dbo.SGRC_Variables", "VariableTipoDatoRetorno", "dbo.SGRC_TiposDeDatosDeRetorno");
            DropIndex("dbo.SGRC_Variables", new[] { "VariableTipoDeVariable" });
            DropIndex("dbo.SGRC_Variables", new[] { "VariableTipoDatoRetorno" });
            DropIndex("dbo.SGRC_Variables", new[] { "VariableOrigen" });
            DropTable("dbo.SGRC_Variables");
            DropTable("dbo.SGRC_TiposDeVariables");
            DropTable("dbo.SGRC_TiposDeOrigenesDeVAriables");
            DropTable("dbo.SGRC_TiposDeDatosDeRetorno");
        }
    }
}
