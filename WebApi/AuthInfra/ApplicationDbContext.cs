using System;
using System.Collections.Generic;
using WebApi.AuthIdentity;

namespace WebApi.AuthInfra
{
    public class ApplicationDbContext : IDisposable
    {
        private ApplicationDbContext(IList<ApplicationUser> users)
        {
            Users = users;
        }

        public IList<ApplicationUser> Users { get; set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public static ApplicationDbContext Create()
        {
            //You can use any database and hook it here

            // todo add settings where appropriate to switch server & database in your own application
            //var client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString);
            //var database = client.GetDatabase(ConfigurationManager.AppSettings["MongoDBName"]);
            //var users = database.GetCollection<ApplicationUser>("users");
            //var roles = database.GetCollection<IdentityRole>("roles");

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "a@a.com",
                    Email = "a@a.com",
                    Password = "test",
                    Roles = new List<string> {"Admin", "Admin2"}
                },
                new ApplicationUser
                {
                    UserName = "a@a2.com",
                    Email = "a@a2.com",
                    Password = "test2",
                    Roles = new List<string> {"Admin"}
                }
            };

            return new ApplicationDbContext(users);
        }
    }
}