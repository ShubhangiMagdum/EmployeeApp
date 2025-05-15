namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSalarySlip : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SalarySlips", "SalaryMonth", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SalarySlips", "SalaryMonth", c => c.DateTime(nullable: false));
        }
    }
}
