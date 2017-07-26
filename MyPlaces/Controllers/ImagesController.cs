using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MyPlaces.Models;
using FlickrNet;
using System.Web;

using System.Drawing;

//For catching the JSON image data
using System.Runtime.Serialization.Json;
using System.IO;

namespace MyPlaces.Controllers
{
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {
        private MyPlacesContext db = new MyPlacesContext();

        // GET: api/Images
        [Route("")]
        [HttpGet]
        public IQueryable<Images> GetImages()
        {
            //Avoid getting all image objects for the user as it consumes the main thread
            
            return db.Images;
        }

        //Provide a url Route for this function

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetImages(int id)
        {
            Images images = await db.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }
            
            return Ok(images);
        }

        //Local service function to convert images to byte arrays before saving to our database
        //Pass in the large URL to set the image data
        public byte[] ImageToArray(string photoUrl)
        {
            WebClient web = new WebClient();

            //Download the image from its URL to the server
            byte[] arr = web.DownloadData(photoUrl);

            return arr;
        }

        //Use this to return a default image if no image data found on flickr
        public byte[] ImageToArrayDefault()
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

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        [Route("{name}")]
        [HttpGet]
        public IEnumerable<Images> GetImageByName(string name)
        {
            //Get the first contact in the contacts list with the specified id
            //Pass in the location name and check if the main image description contains it

            //Create the flickr objects and set it up on each call

            
            Flickr flickr = new Flickr();
            flickr.ApiKey = "dba11127902261afd54826b290ed3de6";
            flickr.ApiSecret = "1af450a5e13b378c";

            //Create the byte arrays to store the binary data of each object returned through the JSON
           
            //Set up search options object
            var options = new PhotoSearchOptions() { Tags = name, PerPage = 3, Page = 1, Extras = PhotoSearchExtras.LargeUrl | PhotoSearchExtras.Tags };
          
            //This return all image objects including main description, landmark id and secondary images
            Images[] ImageArray = db.Images.Where<Images>(c => c.Description.Contains(name)).ToArray();

            //If the location or any duplicate of it is not found in the ImageArray the array size count will be O
            //To make it easier convert this array to a list object for now
            List<Images> myImageList = ImageArray.ToList<Images>();

            //If we have 0 entries in the list corresponding to the search name then check flickr
            int i = 0;
            Images myImageObject = new Images();

            if(myImageList.Count <= 0)
            {
                //Search for photos using the search option
                //Search tags are set for a max of 3 photos per page
                PhotoCollection photos = flickr.PhotosSearch(options);

                //We should at least return 3 photos based on the tag

                //If we found some photos between 1 and 3
                if(photos.Count > 0 && photos.Count <= 3)
                {

                    //Only taking the first photos description due to versioning of the db
                    myImageObject.Description = photos[0].Description;
                    myImageObject.Image1 = ImageToArray(photos[0].LargeUrl);
                    myImageObject.Image2 = ImageToArray(photos[1].LargeUrl);
                    myImageObject.Image3 = ImageToArray(photos[2].LargeUrl);
             
                    //Add this to our list
                    myImageList.Add(myImageObject);

                    //Save these to our db for future purposes
                    ImageArray = myImageList.ToArray<Images>();
                }
                
            }
   
            return ImageArray;
        }

        // PUT: api/Images/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutImages(int id, Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != images.Id)
            {
                return BadRequest();
            }

            db.Entry(images).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Images
        [ResponseType(typeof(Images))]
        public async Task<IHttpActionResult> PostImages(Images images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Images.Add(images);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = images.Id }, images);
        }

        // DELETE: api/Images/5
        [ResponseType(typeof(Images))]
        public async Task<IHttpActionResult> DeleteImages(int id)
        {
            Images images = await db.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }

            db.Images.Remove(images);
            await db.SaveChangesAsync();

            return Ok(images);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ImagesExists(int id)
        {
            return db.Images.Count(e => e.Id == id) > 0;
        }
    }
}