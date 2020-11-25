using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalendarWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarWebApi.DataAccess
{
    public class Repository : IRepository
    {
        private readonly CalendarContext _context;

        public Repository(CalendarContext context)
        {
            _context = context;
        }

        public async Task<List<Calendar>> GetCalendar()
        {
            IQueryable<Calendar> calendars = _context.Calendar;
            return await calendars.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<Calendar> GetCalendarByID(int ID)
        {
            return await _context.Calendar.Where(x => x.Id == ID).FirstOrDefaultAsync();
        }

        public async Task<Calendar> AddEvent(Calendar calendarEvent)
        {
            await _context.Calendar.AddAsync(calendarEvent);
            await _context.SaveChangesAsync();

            return calendarEvent;
        }

        public async Task<List<Calendar>> GetCalendar(EventQueryModel query)
        {
            IQueryable<Calendar> calendars = _context.Calendar;
            if (query.Location != null)
            {
                calendars = calendars.Where(o => o.Location.Contains(query.Location));
            }

            if (query.EventOrganizer != null)
            {
                calendars = calendars.Where(o => o.EventOrganizer.Contains(query.EventOrganizer));
            }

            if (query.Name != null)
            {
                calendars = calendars.Where(o => o.Name.Contains(query.Name));
            }

            return await calendars.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<Calendar> DeleteEvent(Calendar calendarEvent)
        {
            _context.Calendar.Remove(calendarEvent);
            await _context.SaveChangesAsync();

            return calendarEvent;
        }

        public async Task<List<Calendar>> GetEventsSorted()
        {
            IQueryable<Calendar> calendars = _context.Calendar;
            return await calendars.OrderByDescending(o => o.Time).ToListAsync();
        }

        public async Task<Calendar> UpdateEvent(Calendar calendarEvent)
        {
            _context.Calendar.Update(calendarEvent);
            await _context.SaveChangesAsync();

            return calendarEvent;
        }

    }
}
