using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Events
        [HttpGet]
        public IActionResult GetEvents()
        {
            List<Event> events = new List<Event>();

            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = @"SELECT EventID, OrganiserID, EventName,
                                        EventDescription, EventDate,
                                        EventLocation, EventDistance, EventType
                                 FROM Events";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new Event
                            {
                                EventID = reader.GetInt32(0),
                                OrganiserID = reader.GetInt32(1),
                                EventName = reader.GetString(2),
                                EventDescription = reader.IsDBNull(3)
                                    ? null : reader.GetString(3),
                                EventDate = reader.GetDateTime(4),
                                EventLocation = reader.GetString(5),
                                EventDistance = reader.GetDecimal(6),
                                EventType = reader.GetString(7)
                            });
                        }
                    }
                }
            }


            return Ok(events);
        }

        // GET: api/Events/1
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            Event? eventItem = null;

            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = @"SELECT EventID, OrganiserID, EventName,
                                        EventDescription, EventDate,
                                        EventLocation, EventDistance, EventType
                                 FROM dbo.Events
                                 WHERE EventID = @EventID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventID", id);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            eventItem = new Event
                            {
                                EventID = reader.GetInt32(0),
                                OrganiserID = reader.GetInt32(1),
                                EventName = reader.GetString(2),
                                EventDescription = reader.IsDBNull(3)
                                    ? null : reader.GetString(3),
                                EventDate = reader.GetDateTime(4),
                                EventLocation = reader.GetString(5),
                                EventDistance = reader.GetDecimal(6),
                                EventType = reader.GetString(7)
                            };
                        }
                    }
                }
            }

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
            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO dbo.Events
                    (OrganiserID, EventName, EventDescription, EventDate,
                     EventLocation, EventDistance, EventType)
                    VALUES
                    (@OrganiserID, @EventName, @EventDescription, @EventDate,
                     @EventLocation, @EventDistance, @EventType)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrganiserID", newEvent.OrganiserID);
                    command.Parameters.AddWithValue("@EventName", newEvent.EventName);
                    command.Parameters.AddWithValue("@EventDescription",
                        (object?)newEvent.EventDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EventDate", newEvent.EventDate);
                    command.Parameters.AddWithValue("@EventLocation", newEvent.EventLocation);
                    command.Parameters.AddWithValue("@EventDistance", newEvent.EventDistance);
                    command.Parameters.AddWithValue("@EventType", newEvent.EventType);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Ok("Event created successfully.");
        }

        // PUT: api/Events/1
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, Event updatedEvent)
        {
            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = @"UPDATE dbo.Events
                    SET OrganiserID = @OrganiserID,
                        EventName = @EventName,
                        EventDescription = @EventDescription,
                        EventDate = @EventDate,
                        EventLocation = @EventLocation,
                        EventDistance = @EventDistance,
                        EventType = @EventType
                    WHERE EventID = @EventID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventID", id);
                    command.Parameters.AddWithValue("@OrganiserID", updatedEvent.OrganiserID);
                    command.Parameters.AddWithValue("@EventName", updatedEvent.EventName);
                    command.Parameters.AddWithValue("@EventDescription",
                        (object?)updatedEvent.EventDescription ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EventDate", updatedEvent.EventDate);
                    command.Parameters.AddWithValue("@EventLocation", updatedEvent.EventLocation);
                    command.Parameters.AddWithValue("@EventDistance", updatedEvent.EventDistance);
                    command.Parameters.AddWithValue("@EventType", updatedEvent.EventType);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Event not found.");
                    }
                }
            }

            return Ok("Event updated successfully.");
        }

        // DELETE: api/Events/1
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            {
                string query = "DELETE FROM dbo.Events WHERE EventID = @EventID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EventID", id);

                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Event not found.");
                    }
                }
            }

            return Ok("Event deleted successfully.");
        }


    }
}

