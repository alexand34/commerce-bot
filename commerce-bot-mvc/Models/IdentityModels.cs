using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Bot;
using Bot.Dto.Entitites;
using commerce_bot_mvc.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace commerce_bot_mvc.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<BotUser> BotUsers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasMany(i => i.OrderData).WithMany();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}