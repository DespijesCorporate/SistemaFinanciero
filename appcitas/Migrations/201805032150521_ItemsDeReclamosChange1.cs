namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsDeReclamosChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidad", c => c.String());
            AddColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_ItemsDeReclamos", "Segmento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_ItemsDeReclamos", "Segmento");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidad");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId");
        }
    }
}
