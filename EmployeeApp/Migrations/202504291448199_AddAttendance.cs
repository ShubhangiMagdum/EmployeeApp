namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        Date = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.AttendanceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attendances");
        }
    }
}
