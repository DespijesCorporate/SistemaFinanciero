namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambioFormulaANullValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SGRC_Funciones", "FuncionFormula", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGRC_Funciones", "FuncionFormula", c => c.String(nullable: false));
        }
    }
}
