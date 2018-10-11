namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsDeReclamosChange2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
