namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsDeReclamosChange3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidad");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId");
            DropColumn("dbo.SGRC_ItemsDeReclamos", "Segmento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SGRC_ItemsDeReclamos", "Segmento", c => c.String());
            AddColumn("dbo.SGRC_ItemsDeReclamos", "SegmentoId", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidad", c => c.String());
            AddColumn("dbo.SGRC_ItemsDeReclamos", "TipoAnualidadId", c => c.String(maxLength: 10, unicode: false));
        }
    }
}
