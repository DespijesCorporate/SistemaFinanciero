namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Parametro2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Parametros", "TipoParametro", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_Parametros", "TipoParametro");
        }
    }
}
