namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterEmployeeTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "RoleId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "RoleId");
        }
    }
}
