namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmployeeLeave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeLeaves",
                c => new
                    {
                        LeaveId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        LeaveType = c.String(),
                        Reason = c.String(),
                        LeaveStartDate = c.DateTime(nullable: false),
                        LeaveEndDate = c.DateTime(nullable: false),
                        TotalDays = c.Int(nullable: false),
                        LeaveStatus = c.String(),
                    })
                .PrimaryKey(t => t.LeaveId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeeLeaves");
        }
    }
}
