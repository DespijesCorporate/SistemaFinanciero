namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change1 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.SGRC_Config",
            //    c => new
            //        {
            //            ConfigId = c.String(nullable: false, maxLength: 5),
            //            ConfigDescripcion = c.String(maxLength: 500),
            //            ConfigObservacion = c.String(maxLength: 500),
            //            ConfigStatus = c.String(maxLength: 1),
            //        })
            //    .PrimaryKey(t => t.ConfigId);
            
            //CreateTable(
            //    "dbo.SGRC_ConfigItem",
            //    c => new
            //        {
            //            ConfigId = c.String(nullable: false, maxLength: 5),
            //            ConfigItemId = c.String(nullable: false, maxLength: 10),
            //            ConfigItemDescripcion = c.String(maxLength: 500),
            //            ConfigItemAbreviatura = c.String(maxLength: 50),
            //            ConfigItemObservacion = c.String(maxLength: 200),
            //            ConfigItemStatus = c.String(maxLength: 1),
            //        })
            //    .PrimaryKey(t => new { t.ConfigId, t.ConfigItemId });
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.SGRC_ConfigItem");
            //DropTable("dbo.SGRC_Config");
        }
    }
}
