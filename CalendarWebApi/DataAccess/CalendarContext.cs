using Microsoft.EntityFrameworkCore;
using CalendarWebApi.Models;

namespace CalendarWebApi.DataAccess
{
    public class CalendarContext : DbContext
    {
        public CalendarContext(DbContextOptions<CalendarContext> options)
            : base(options)
        { }

        public DbSet<Calendar> Calendar { get; set; }
    }
}
