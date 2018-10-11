namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parametros3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SGRC_Parametros", "Variable_VariableCodigo", "dbo.SGRC_Variables");
            DropForeignKey("dbo.SGRC_Parametros", "VariableCodigo", "dbo.SGRC_Variables");
            DropIndex("dbo.SGRC_Parametros", new[] { "VariableCodigo" });
            DropIndex("dbo.SGRC_Parametros", new[] { "Variable_VariableCodigo" });
            AddColumn("dbo.SGRC_Variables", "VariableFormula", c => c.String());
            DropColumn("dbo.SGRC_Parametros", "VariableCodigo");
            DropColumn("dbo.SGRC_Parametros", "Variable_VariableCodigo");
            DropColumn("dbo.SGRC_Variables", "VariableFuncion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGRC_Variables", "VariableFuncion", c => c.String());
            AddColumn("dbo.SGRC_Parametros", "Variable_VariableCodigo", c => c.String(maxLength: 128));
            AddColumn("dbo.SGRC_Parametros", "VariableCodigo", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.SGRC_Variables", "VariableFormula");
            CreateIndex("dbo.SGRC_Parametros", "Variable_VariableCodigo");
            CreateIndex("dbo.SGRC_Parametros", "VariableCodigo");
            AddForeignKey("dbo.SGRC_Parametros", "VariableCodigo", "dbo.SGRC_Variables", "VariableCodigo");
            AddForeignKey("dbo.SGRC_Parametros", "Variable_VariableCodigo", "dbo.SGRC_Variables", "VariableCodigo");
        }
    }
}
