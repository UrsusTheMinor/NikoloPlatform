using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nikolo.Data.Models;

namespace Nikolo.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<AvailableTime> AvailableTimes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingInformation> BookingInformations { get; set; }
    public DbSet<Child> Children { get; set; }
    public DbSet<DateTimeSlots> DateTimeSlots { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    public DbSet<InformationType> InformationTypes { get; set; }
    public DbSet<InformationGroup> InformationGroups { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillEmployee> SkillEmployees { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamBuddy> TeamBuddies { get; set; }
    public DbSet<TeamEmployee> TeamEmployees { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SkillEmployee>()
            .HasKey(se => new { se.SkillId, se.EmployeeId });
        
        modelBuilder.Entity<TeamEmployee>()
            .HasKey(se => new { se.TeamId, se.EmployeeId });
        
        modelBuilder.Entity<TeamBuddy>()
            .HasKey(tb => new { tb.Employee1Id, tb.Employee2Id });

        modelBuilder.Entity<TeamBuddy>()
            .HasOne(tb => tb.Employee1)
            .WithMany()
            .HasForeignKey(tb => tb.Employee1Id)
            .OnDelete(DeleteBehavior.NoAction); // No cascade delete

        modelBuilder.Entity<TeamBuddy>()
            .HasOne(tb => tb.Employee2)
            .WithMany()
            .HasForeignKey(tb => tb.Employee2Id)
            .OnDelete(DeleteBehavior.NoAction);

    }

}