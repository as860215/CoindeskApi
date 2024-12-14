using Microsoft.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    public virtual DbSet<Currency> Currency { get; set; }
    public DatabaseContext() : base() { }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
}