using Microsoft.EntityFrameworkCore;
using Models;

namespace DataLayer;

public class FunctionAppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public FunctionAppDbContext()
    {
        //blank
    }

    public FunctionAppDbContext(DbContextOptions<FunctionAppDbContext> options)
    : base(options)
    {
        //blank
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));
        }   
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(200);

            entity.HasData(
                new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith" },
                new Employee { Id = 3, FirstName = "Bob", LastName = "Johnson" },
                new Employee { Id = 4, FirstName = "Alice", LastName = "Williams" },
                new Employee { Id = 5, FirstName = "Michael", LastName = "Brown" }
            );
        });

    }
}
