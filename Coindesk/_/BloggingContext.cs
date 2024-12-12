using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public virtual DbSet<Currency> Currency { get; set; }
    public BloggingContext() : base() { }
    public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) { }
}