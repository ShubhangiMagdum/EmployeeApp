namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalarySlip : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalarySlips",
                c => new
                    {
                        SlipId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        SalaryMonth = c.DateTime(nullable: false),
                        NetSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SlipId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SalarySlips");
        }
    }
}
