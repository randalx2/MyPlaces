using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace MyPlaces.Models
{
    public class Images
    {
        //Primary Key
        public int Id { get; set; }

        //Foreign Key
        //Each landmark will be associated with its images ==> 3 at maximum for this app
        public int LandmarkId { get; set; }

        //Navigation Property
        public Landmark Landmark { get; set; }

        public string Description { get; set; }

        //Main Image == Try to give 3 images per landmark
        //Save images as binary data ==> byte arrays
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
    }
}