namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VariableDeItemChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_VariablesDeItems", "VariableNombre", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_VariablesDeItems", "VariableNombre");
        }
    }
}
