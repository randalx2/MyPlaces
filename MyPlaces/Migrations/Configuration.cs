namespace MyPlaces.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MyPlaces.Models;
    using System.Drawing;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<MyPlaces.Models.MyPlacesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //Method for seeding image Batman-Jim-Lee.jpg

        public byte[] ImageToArray()
        {
            Image img = Image.FromFile(@"C:\Batman-Jim-Lee.jpg");
            byte[] arr;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                arr = ms.ToArray();
            }
            return arr;
        }

        protected override void Seed(MyPlaces.Models.MyPlacesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //Seed our db with test data
            context.Locations.AddOrUpdate(x => x.Id,
                new Location() { Id = 1, Name = "Gotham City", Latitude = 43.899, Longitude = 89.099 },
                new Location() { Id = 2, Name = "Metropolis", Latitude = 23.994, Longitude = 87.343 },
                new Location() { Id = 3, Name = "Starling City", Latitude = 56.343, Longitude = 34.123 }
                );

            context.Landmarks.AddOrUpdate(x => x.Id,
                new Landmark() { Id = 1, Title = "Clock Tower", LocationId = 1},
                new Landmark() { Id = 2, Title = "Daily Planet", LocationId = 2},
                new Landmark() { Id = 3, Title = "Queen Towers", LocationId = 3},
                new Landmark() { Id = 4, Title = "Arkham Asylum", LocationId = 1},
                new Landmark() { Id = 5, Title = "Clark's House", LocationId = 2},
                new Landmark() { Id = 6, Title = "Star City Hall", LocationId = 3}
                );

            context.Images.AddOrUpdate(x => x.Id,
                new Images() { Id = 1, LandmarkId = 4, Description = "Asylum for criminally insane", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray()},
                new Images() { Id = 2, LandmarkId = 1, Description = "Where the Joker fell to his demise", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 3, LandmarkId = 5, Description = "The man of steel's apartment", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 4, LandmarkId = 6, Description = "City Hall for Star City", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 5, LandmarkId = 4, Description = "The Joker's usual Hangout", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 6, LandmarkId = 2, Description = "Wherever Newspapers are still sold", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() },
                new Images() { Id = 7, LandmarkId = 3, Description = "CEO Oliver Queen's 9 to 5", Image1 = ImageToArray(), Image2 = ImageToArray(), Image3 = ImageToArray() }
                );
        }
    }
}
