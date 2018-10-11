namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VariableChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Variables", "OrigenId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_Variables", "DatoDeRetornoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_Variables", "TipoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            DropColumn("dbo.SGRC_Variables", "Origen");
            DropColumn("dbo.SGRC_Variables", "DatoDeRetorno");
            DropColumn("dbo.SGRC_Variables", "TipoDeVariable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGRC_Variables", "TipoDeVariable", c => c.String(nullable: false));
            AddColumn("dbo.SGRC_Variables", "DatoDeRetorno", c => c.String(nullable: false));
            AddColumn("dbo.SGRC_Variables", "Origen", c => c.String(nullable: false));
            DropColumn("dbo.SGRC_Variables", "TipoId");
            DropColumn("dbo.SGRC_Variables", "DatoDeRetornoId");
            DropColumn("dbo.SGRC_Variables", "OrigenId");
        }
    }
}
