namespace EmployeeApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DesignationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesignationId = c.Int(nullable: false, identity: true),
                        DesignationName = c.String(),
                        DesignationDescription = c.String(),
                        CreatedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.DesignationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Designations");
        }
    }
}
