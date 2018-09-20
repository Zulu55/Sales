namespace Sales.API.Helpers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Common.Models;
    using Domain.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static DataContext db = new DataContext();

        public static bool DeleteUser(string userName, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var response = userManager.RemoveFromRole(userASP.Id, roleName);
            return response.Succeeded;
        }

        public static ApplicationUser GetUserASP(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            return userASP;
        }

        public static Response CreateUserASP(UserRequest userRequest)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                var oldUserASP = userManager.FindByEmail(userRequest.EMail);
                if (oldUserASP != null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "001. User already exists.",
                    };
                }

                var userASP = new ApplicationUser
                {
                    Email = userRequest.EMail,
                    UserName = userRequest.EMail,
                    PhoneNumber = userRequest.Phone,
                };

                var result = userManager.Create(userASP, userRequest.Password);
                if (result.Succeeded)
                {
                    var newUserASP = userManager.FindByEmail(userRequest.EMail);
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.GivenName, userRequest.FirstName));
                    userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Name, userRequest.LastName));

                    if (!string.IsNullOrEmpty(userRequest.Address))
                    {
                        userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.StreetAddress, userRequest.Address));
                    }

                    if (!string.IsNullOrEmpty(userRequest.ImagePath))
                    {
                        userManager.AddClaim(newUserASP.Id, new System.Security.Claims.Claim(ClaimTypes.Uri, userRequest.ImagePath));
                    }

                    return new Response
                    {
                        IsSuccess = true,
                    };
                }

                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error}, ";
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = errors,
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(currentUserName);
            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = newUserName;
            userASP.Email = newUserName;
            var response = userManager.Update(userASP);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, "Admin", password);
                return;
            }
        }

        public static void CreateUserASP(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, email);
            }

            userManager.AddToRole(userASP.Id, roleName);
        }

        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = userManager.Create(userASP, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(userASP.Id, roleName);
            }
        }

        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}", random.Next(100000, 999999));
            var response = await userManager.AddPasswordAsync(userASP.Id, newPassword);
            if (response.Succeeded)
            {
                var subject = "Sales App - Recuperación de contraseña";
                var body = string.Format(@"
                    <h1>Sales App - Recuperación de contraseña</h1>
                    <p>Su nueva contraseña es: <strong>{0}</strong></p>
                    <p>Por favor no olvide cambiarla por una de fácil recordación",
                    newPassword);

                await MailHelper.SendMail(email, subject, body);
            }
        }

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}