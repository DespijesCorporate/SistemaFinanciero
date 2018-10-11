namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambios2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SGRC_VariablesDeItems", "CondicionLogica", c => c.String(nullable: false, maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGRC_VariablesDeItems", "CondicionLogica", c => c.String(maxLength: 5));
        }
    }
}
