namespace appcitas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MotorTablesV11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SGRC_Variables", "VariableFuncion", c => c.String());
            AlterColumn("dbo.SGRC_Variables", "VariableDescripcion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SGRC_Variables", "VariableDescripcion", c => c.String());
            DropColumn("dbo.SGRC_Variables", "VariableFuncion");
        }
    }
}
