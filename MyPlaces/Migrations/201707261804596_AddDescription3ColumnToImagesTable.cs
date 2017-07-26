namespace MyPlaces.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescription3ColumnToImagesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Images", "Description3", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Images", "Description3");
        }
    }
}
