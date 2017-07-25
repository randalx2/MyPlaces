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

using System.Drawing;

//For catching the JSON image data
using System.Runtime.Serialization.Json;


namespace MyPlaces.Controllers
{
    [RoutePrefix("api/Locations")]
    public class ImagesController : ApiController
    {
        private MyPlacesContext db = new MyPlacesContext();

        // GET: api/Images
        public IQueryable<Images> GetImages()
        {
            //Convert the image attbs to base 64 string
            /*
            foreach (Images image in db.Images)
            {
                Convert.ToBase64String(image.Image1);
                Convert.ToBase64String(image.Image2);
                Convert.ToBase64String(image.Image3); 
            }
            */

            //Avoid getting all image objects for the user as it consumes the main thread
            
            return db.Images;
        }

        //Provide a url Route for this function

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        public async Task<IHttpActionResult> GetImages(int id)
        {
            Images images = await db.Images.FindAsync(id);
            if (images == null)
            {
                return NotFound();
            }
            
            return Ok(images);
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