namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FuncionesTableChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Funciones", "ConfigId", c => c.String(nullable: false, maxLength: 5, unicode: false));
            AlterColumn("dbo.SGRC_Funciones", "FuncionTipoDeRetorno", c => c.String(nullable: false, maxLength: 10, unicode: false));
            CreateIndex("dbo.SGRC_Funciones", new[] { "ConfigId", "FuncionTipoDeRetorno" });
            AddForeignKey("dbo.SGRC_Funciones", new[] { "ConfigId", "FuncionTipoDeRetorno" }, "dbo.SGRC_ConfigItem", new[] { "ConfigId", "ConfigItemId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_Funciones", new[] { "ConfigId", "FuncionTipoDeRetorno" }, "dbo.SGRC_ConfigItem");
            DropIndex("dbo.SGRC_Funciones", new[] { "ConfigId", "FuncionTipoDeRetorno" });
            AlterColumn("dbo.SGRC_Funciones", "FuncionTipoDeRetorno", c => c.String(nullable: false));
            DropColumn("dbo.SGRC_Funciones", "ConfigId");
        }
    }
}
