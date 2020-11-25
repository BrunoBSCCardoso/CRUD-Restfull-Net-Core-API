using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CalendarWebApi.Models
{
    public class Calendar
    {
        /// <summary>
        /// id of the event
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name of the event
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Location of the event
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Time of the event
        /// </summary>
        public long Time { get; set; }
        /// <summary>
        /// Organizer of the event
        /// </summary>
        public string EventOrganizer { get; set; }
        /// <summary>
        /// Members of the event
        /// </summary>
        public string Members { get; set; }
    }
}
