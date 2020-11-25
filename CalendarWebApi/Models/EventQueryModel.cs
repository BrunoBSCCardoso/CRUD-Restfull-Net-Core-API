using System;
namespace CalendarWebApi.Models
{
    public class EventQueryModel
    {
        public string EventOrganizer { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
    }
}
