using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace MyPlaces.Models
{
    public class Landmark
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key
        public int LocationId { get; set; }

        //Meant to be related to image title perhaps
        public string Title { get; set; }

        //Navigation Property
        public Location Location { get; set; }
    }
}