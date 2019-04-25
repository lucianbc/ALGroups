namespace ALGroups.Migrations
{
    using ALGroups.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<ALGroups.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ALGroups.Models.ApplicationDbContext";
        }

        protected override void Seed(ALGroups.Models.ApplicationDbContext context)
        {//  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            Task.Run(async () => { await SeedAsync(context); }).Wait();
        }

        private async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == Startup.ADMIN_EMAIL))
            {

                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                await roleManager.CreateAsync(new IdentityRole { Name = Startup.ADMIN_ROLE });

                var user = new ApplicationUser { UserName = Startup.ADMIN_EMAIL, Email = Startup.ADMIN_EMAIL };

                await userManager.CreateAsync(user, Startup.ADMIN_PASSWORD);
                await userManager.AddToRoleAsync(user.Id, role: Startup.ADMIN_ROLE);
            }
        }
    }
}
