using Microsoft.EntityFrameworkCore;

public class BloggingContext : DbContext
{
    public DbSet<Currency> Currency { get; set; }
    public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) { }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(
    //        @"Server=(localdb)\mssqllocaldb;Database=Coindesk;Integrated Security=True");
    //}
}