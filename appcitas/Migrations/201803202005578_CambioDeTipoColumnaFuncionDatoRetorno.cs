namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambioDeTipoColumnaFuncionDatoRetorno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Parametros", "ConfigID", c => c.String(nullable: false, maxLength: 5, unicode: false));
            AlterColumn("dbo.SGRC_Funciones", "FuncionTipoDeRetorno", c => c.String(nullable: false));
            AlterColumn("dbo.SGRC_Parametros", "TipoId", c => c.String(nullable: false, maxLength: 10, unicode: false));            
            CreateIndex("dbo.SGRC_Parametros", new[] { "ConfigID", "TipoId" });
            AddForeignKey("dbo.SGRC_Parametros", new[] { "ConfigID", "TipoId" }, "dbo.SGRC_ConfigItem", new[] { "ConfigId", "ConfigItemId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_Parametros", new[] { "ConfigID", "TipoId" }, "dbo.SGRC_ConfigItem");
            DropIndex("dbo.SGRC_Parametros", new[] { "ConfigID", "TipoId" });           
            AlterColumn("dbo.SGRC_Parametros", "TipoId", c => c.String(nullable: false));
            AlterColumn("dbo.SGRC_Funciones", "FuncionTipoDeRetorno", c => c.Int(nullable: false));
            DropColumn("dbo.SGRC_Parametros", "ConfigID");
            AddPrimaryKey("dbo.SGRC_ConfigItem", new[] { "ConfigId", "ConfigItemId" });
        }
    }
}
