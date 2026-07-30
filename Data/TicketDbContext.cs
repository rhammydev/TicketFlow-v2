using Microsoft.EntityFrameworkCore;
using TicketFlow.Model;

namespace TicketFlow.Data;

public class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<BookingLog> BookingLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(e => e.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(e => e.Status)
            .HasConversion<string>();

        modelBuilder.Entity<BookingLog>()
            .Property(e => e.Action)
            .HasConversion<string>();

    }
}