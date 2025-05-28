using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EventManagement.Context; // Make sure to import your DbContext namespace

namespace EventManagement.Context
{
    public class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        public EventDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new EventDbContext(optionsBuilder.Options);
        }
    }
}
