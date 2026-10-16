namespace RaceDayAPI.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        public string EventName { get; set; } = "";

        public string? EventDescription { get; set; }

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; } = "";

        public decimal EventDistance { get; set; }

        public string EventType { get; set; } = "";
    }
}
