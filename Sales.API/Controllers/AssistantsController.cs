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
using Sales.Common.Models;
using Sales.Domain.Models;

namespace Sales.API.Controllers
{
    public class AssistantsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Assistants
        public IQueryable<Assistant> GetAssistants()
        {
            return db.Assistants;
        }

        // GET: api/Assistants/5
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> GetAssistant(int id)
        {
            Assistant assistant = await db.Assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }

            return Ok(assistant);
        }

        // PUT: api/Assistants/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAssistant(int id, Assistant assistant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assistant.AssistantId)
            {
                return BadRequest();
            }

            db.Entry(assistant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssistantExists(id))
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

        // POST: api/Assistants
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> PostAssistant(Assistant assistant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Assistants.Add(assistant);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = assistant.AssistantId }, assistant);
        }

        // DELETE: api/Assistants/5
        [ResponseType(typeof(Assistant))]
        public async Task<IHttpActionResult> DeleteAssistant(int id)
        {
            Assistant assistant = await db.Assistants.FindAsync(id);
            if (assistant == null)
            {
                return NotFound();
            }

            db.Assistants.Remove(assistant);
            await db.SaveChangesAsync();

            return Ok(assistant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssistantExists(int id)
        {
            return db.Assistants.Count(e => e.AssistantId == id) > 0;
        }
    }
}