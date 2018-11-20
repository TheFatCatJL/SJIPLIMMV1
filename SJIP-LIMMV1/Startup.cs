using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Services;

[assembly: OwinStartupAttribute(typeof(SJIP_LIMMV1.Startup))]
namespace SJIP_LIMMV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IEmailSender, EmailSender>();
        }

        //TO-DO : Refactor this once helper methods for configs reaches a certain amount
        private void createRolesandUsers()
        {
            //Instantiates the DB context to link the db to the roleManager and UserManager APIs
            ApplicationDbContext dbcontext = new ApplicationDbContext();

            // create managers and their stores with the dbcontext
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbcontext));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbcontext));

            //creates default admin role and an admin user at start of app
            //Note this section can be commented if such is not necessary although impact to code performance is minimal
            if(!roleManager.RoleExists("Admin"))
            {
                //Role Creation Area
                var roleAdmin = new IdentityRole();
                roleAdmin.Name = "Admin";
                roleManager.Create(roleAdmin);


                //User Creation Area
                var userAdmin = new ApplicationUser();
                //set default personal data of default admin
                userAdmin.UserName = "sjintern";
                userAdmin.Email = "ia-jason.limeh@surbanajurong.com";
                //One can set a hashed password in this manner
                //userAdmin.PasswordHash = "myHashedPassword";
                string userPWD = "sjintern123";
                var createUserResult = userManager.Create(userAdmin, userPWD);

                if(createUserResult.Succeeded)
                {
                    var addUserToAdminRoleResult = userManager.AddToRole(userAdmin.Id, "Admin");
                }
            }

            //Creating other roles - for the sake of populating dropdownlist later for adding users
            //Should consider refactor to a one-time setup area with above default admin code
            if(!roleManager.RoleExists("Checker"))
            {
                var roleChecker = new IdentityRole("Checker");
                roleChecker.Name = "Checker";
                roleManager.Create(roleChecker);
            }

            if (!roleManager.RoleExists("Supervisor"))
            {
                var roleSupervisor = new IdentityRole("Supervisor");
                roleSupervisor.Name = "Supervisor";
                roleManager.Create(roleSupervisor);
            }

            if (!roleManager.RoleExists("Technician"))
            {
                var roleTechnician = new IdentityRole("Technician");
                roleTechnician.Name = "Technician";
                roleManager.Create(roleTechnician);
            }

            if (!roleManager.RoleExists("LeadTech"))
            {
                var roleLeadTech = new IdentityRole("LeadTech");
                roleLeadTech.Name = "LeadTech";
                roleManager.Create(roleLeadTech);
            }

            if (!roleManager.RoleExists("DataUsers"))
            {
                var roleDataUsers = new IdentityRole("DataUsers");
                roleDataUsers.Name = "DataUsers";
                roleManager.Create(roleDataUsers);
            }

        }
    }
}
