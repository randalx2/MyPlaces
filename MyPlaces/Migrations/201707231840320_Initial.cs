namespace MyPlaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LandmarkId = c.Int(nullable: false),
                        Description = c.String(),
                        Image1 = c.Binary(),
                        Image2 = c.Binary(),
                        Image3 = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Landmarks", t => t.LandmarkId, cascadeDelete: true)
                .Index(t => t.LandmarkId);
            
            CreateTable(
                "dbo.Landmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationId = c.Int(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "LandmarkId", "dbo.Landmarks");
            DropForeignKey("dbo.Landmarks", "LocationId", "dbo.Locations");
            DropIndex("dbo.Landmarks", new[] { "LocationId" });
            DropIndex("dbo.Images", new[] { "LandmarkId" });
            DropTable("dbo.Locations");
            DropTable("dbo.Landmarks");
            DropTable("dbo.Images");
        }
    }
}
