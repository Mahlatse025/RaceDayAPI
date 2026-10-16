
using RaceDayAPI.Models;

namespace RaceDayAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Event_ShouldStoreEventDetails()
        {
            // Arrange: Create a test event
            Event testEvent = new Event
            {
                EventID = 1,
                OrganiserID = 1,
                EventName = "Pretoria Spring Run",
                EventDescription = "A test running event",
                EventDate = new DateTime(2026, 10, 17),
                EventLocation = "Pretoria",
                EventDistance = 15,
                EventType = "Run"
            };

            // Act: Read the event name
            string eventName = testEvent.EventName;

            // Assert: Check that the name is correct
            Assert.Equal("Pretoria Spring Run", eventName);
        }

        [Fact]
        public void Event_ShouldStoreEventDistance()
        {
            // Arrange: Create a test event
            Event testEvent = new Event
            {
                EventName = "Durban Family Walk",
                EventDistance = 8
            };

            // Act: Read the event distance
            decimal distance = testEvent.EventDistance;

            // Assert: Check that the distance is correct
            Assert.Equal(8m, distance);
        }

    }
}
