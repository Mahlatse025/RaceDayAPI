using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RaceDayAPI.Models;
using RaceDayAPI.Data;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RaceDayContext _context;

        public EventsController(
     IConfiguration configuration,
     RaceDayContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetInt32("UserID") != null
                && HttpContext.Session.GetString("UserRole") == "Organiser";
        }

        // GET: api/Events
        [HttpGet]

        public IActionResult GetEvents()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return Unauthorized("Please log in to view events.");

            var events = _context.Events.ToList();

            return Ok(events);
        }


        // GET: api/Events/1
        [HttpGet("{id}")]

        public IActionResult GetEventById(int id)
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return Unauthorized("Please log in to view events.");

            var eventItem = _context.Events.Find(id);

            if (eventItem == null)
            {
                return NotFound("Event not found.");
            }

            return Ok(eventItem);
        }


        // POST: api/Events
        [HttpPost]

        public IActionResult CreateEvent(Event newEvent)
        {
            if (!IsOrganiser())
                return Unauthorized("Only logged-in Organisers can create events.");

            // Use the logged-in Organiser's ID
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return Unauthorized("Please log in first.");

            newEvent.OrganiserID = userId.Value;

            _context.Events.Add(newEvent);
            _context.SaveChanges();

            return Ok("Event created successfully.");
        }

        // PUT: api/Events/1
        [HttpPut("{id}")]

        public IActionResult UpdateEvent(int id, Event updatedEvent)
        {
            if (!IsOrganiser())
                return Unauthorized("Only logged-in Organisers can update events.");

            var existingEvent = _context.Events.Find(id);

            if (existingEvent == null)
                return NotFound("Event not found.");

            existingEvent.EventName = updatedEvent.EventName;
            existingEvent.EventDescription = updatedEvent.EventDescription;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.EventLocation = updatedEvent.EventLocation;
            existingEvent.EventDistance = updatedEvent.EventDistance;
            existingEvent.EventType = updatedEvent.EventType;

            _context.SaveChanges();

            return Ok("Event updated successfully.");
        }


        // DELETE: api/Events/1
        [HttpDelete("{id}")]

        public IActionResult DeleteEvent(int id)
        {
            if (!IsOrganiser())
                return Unauthorized("Only logged-in Organisers can delete events.");

            var eventItem = _context.Events.Find(id);

            if (eventItem == null)
                return NotFound("Event not found.");

            _context.Events.Remove(eventItem);
            _context.SaveChanges();

            return Ok("Event deleted successfully.");
        }



    }
}

