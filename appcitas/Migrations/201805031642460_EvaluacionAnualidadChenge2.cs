namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EvaluacionAnualidadChenge2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Anualidades", "NumeroTarjeta", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SGRC_Anualidades", "NumeroTarjeta");
        }
    }
}
