namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalaryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Salaries",
                c => new
                    {
                        SalaryId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Basic = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HRA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Allowances = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deductions = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Month = c.String(),
                        Year = c.String(),
                        CreatedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.SalaryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Salaries");
        }
    }
}
