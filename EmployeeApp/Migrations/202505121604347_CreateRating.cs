namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        DesignationId = c.Int(nullable: false),
                        Ratings = c.Int(nullable: false),
                        Review = c.String(),
                        Createdby = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RatingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ratings");
        }
    }
}
