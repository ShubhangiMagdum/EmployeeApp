namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSalarySlip1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalarySlips", "SalaryId", c => c.Int(nullable: false));
            DropColumn("dbo.SalarySlips", "EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalarySlips", "EmployeeId", c => c.Int(nullable: false));
            DropColumn("dbo.SalarySlips", "SalaryId");
        }
    }
}
