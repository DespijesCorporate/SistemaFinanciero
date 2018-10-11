namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VariableDeItemChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SGRC_VariablesDeItems", "VariableCodigo", "dbo.SGRC_Variables");
            DropPrimaryKey("dbo.SGRC_VariablesDeItems");
            AddColumn("dbo.SGRC_VariablesDeItems", "VariableDeItemId", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.SGRC_VariablesDeItems", "VariableDeItemId");
            AddForeignKey("dbo.SGRC_VariablesDeItems", "VariableCodigo", "dbo.SGRC_Variables", "VariableCodigo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SGRC_VariablesDeItems", "VariableCodigo", "dbo.SGRC_Variables");
            DropPrimaryKey("dbo.SGRC_VariablesDeItems");
            DropColumn("dbo.SGRC_VariablesDeItems", "VariableDeItemId");
            AddPrimaryKey("dbo.SGRC_VariablesDeItems", "VariableCodigo");
            AddForeignKey("dbo.SGRC_VariablesDeItems", "VariableCodigo", "dbo.SGRC_Variables", "VariableCodigo");
        }
    }
}
