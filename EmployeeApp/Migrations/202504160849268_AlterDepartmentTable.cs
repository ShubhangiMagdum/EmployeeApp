namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterDepartmentTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "CreatedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Departments", "UpdatedAt", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Departments", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
