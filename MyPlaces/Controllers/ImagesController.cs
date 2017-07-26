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

using System.Drawing;

//For catching the JSON image data
using System.Runtime.Serialization.Json;


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

        // GET: api/Images/5
        [ResponseType(typeof(Images))]
        [Route("{name}")]
        [HttpGet]
        public IEnumerable<Images> GetImageByName(string name)
        {
            //Get the first contact in the contacts list with the specified id
            //Pass in the location name and check if the main image description contains it

            //This return all image objects including main description, landmark id and secondary images
            Images[] ImageArray = db.Images.Where<Images>(c => c.Description.Contains(name)).ToArray();

            //If the location or any duplicate of it is not found in the ImageArray the array size count will be O

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