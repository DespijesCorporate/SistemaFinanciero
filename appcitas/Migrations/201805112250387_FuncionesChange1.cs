namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FuncionesChange1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SGRC_Funciones", "FuncionFormula");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGRC_Funciones", "FuncionFormula", c => c.String());
        }
    }
}
