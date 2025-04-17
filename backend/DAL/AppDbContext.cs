using Microsoft.EntityFrameworkCore;
using Models.Accounts;
using Models.Businesses;
using Models.Categories;
using Models.Cities;
using Models.Emails;
using Models.Feedbacks;
using Models.Friendships;
using Models.Messages;
using Models.Places;

namespace DAL;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region DbSets
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Message> Messages { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
