using System.Threading.Tasks;
using System.Collections.Generic;
using CalendarWebApi.Models;

namespace CalendarWebApi.DataAccess
{
    public interface IRepository
    {
        Task<List<Calendar>> GetCalendar();
        Task<Calendar> GetCalendarByID(int ID);
        Task<Calendar> AddEvent(Calendar calendarEvent);
        Task<List<Calendar>> GetCalendar(EventQueryModel query);
        Task<Calendar> DeleteEvent(Calendar calendarEvent);
        Task<List<Calendar>> GetEventsSorted();
        Task<Calendar> UpdateEvent(Calendar calendarEvent);
    }
}