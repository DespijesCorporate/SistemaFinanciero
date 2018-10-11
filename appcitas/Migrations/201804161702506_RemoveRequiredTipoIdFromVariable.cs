namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredTipoIdFromVariable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SGRC_Variables", "TipoId", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGRC_Variables", "TipoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
