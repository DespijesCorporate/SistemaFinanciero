namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Emisor : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.SGRC_Emisor",
            //    c => new
            //        {
            //            EmisorId = c.Int(nullable: false, identity: true),
            //            EmisorCuenta = c.String(nullable: false, maxLength: 10, unicode: false),
            //            MarcaId = c.String(nullable: false, maxLength: 10, unicode: false),
            //            Producto = c.String(nullable: false, maxLength: 100, unicode: false),
            //            Familia = c.String(nullable: false, maxLength: 100, unicode: false),
            //            SegmentoId = c.String(nullable: false, maxLength: 10, unicode: false),
            //            EmisorStatus = c.String(maxLength: 1, fixedLength: true, unicode: false),
            //        })
            //    .PrimaryKey(t => t.EmisorId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.SGRC_Emisor");
        }
    }
}
