using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using WebApi.AuthIdentity;

namespace WebApi.Providers
{
    public class UserStoreService<TUser> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserRoleStore<TUser>
        where TUser : ApplicationUser
    {
        private readonly IList<TUser> _users;


        public UserStoreService(IList<TUser> users)
        {
            _users = users;
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.Password);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.Password != null);
        }

        public virtual Task AddToRoleAsync(TUser user, string roleName)
        {
            user.AddRole(roleName);
            return Task.FromResult(0);
        }

        public virtual Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            user.RemoveRole(roleName);
            return Task.FromResult(0);
        }

        public virtual Task<IList<string>> GetRolesAsync(TUser user)
        {
            return Task.FromResult((IList<string>) user.Roles);
        }

        public virtual Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public virtual void Dispose()
        {
        }

        public virtual Task CreateAsync(TUser user)
        {
            user.CreatedTime = DateTime.Now;
            user.UpdatedTime = DateTime.Now;
            _users.Add(user);
            return Task.FromResult(true);
        }

        public virtual Task UpdateAsync(TUser user)
        {
            // todo should add an optimistic concurrency check
            user.UpdatedTime = DateTime.Now;
            _users.Remove(user);
            _users.Add(user);
            return Task.FromResult(true);
        }

        public virtual Task DeleteAsync(TUser user)
        {
            return Task.FromResult(_users.Remove(user));
        }

        public virtual Task<TUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == userId));
        }

        public virtual Task<TUser> FindByNameAsync(string userName)
        {
            // todo exception on duplicates? or better to enforce unique index to ensure this
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == userName));
        }
    }
}