namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluacionAnualidadChange3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Anualidades", "SegmentoId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_Anualidades", "MarcaId", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AddColumn("dbo.SGRC_Anualidades", "TipoAnualidadId", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_Anualidades", "TipoAnualidadId");
            DropColumn("dbo.SGRC_Anualidades", "MarcaId");
            DropColumn("dbo.SGRC_Anualidades", "SegmentoId");
        }
    }
}
