using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace WebApi.AuthIdentity
{
    public class ApplicationUser : IUser
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new List<string>();
        }

        public virtual string Email { get; set; }
        public List<string> Roles { get; set; }
        public virtual string Password { get; set; }
        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string Id { get; }
        public string UserName { get; set; }

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}