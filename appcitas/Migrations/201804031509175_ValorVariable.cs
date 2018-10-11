namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValorVariable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Variables", "VariableValor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_Variables", "VariableValor");
        }
    }
}
