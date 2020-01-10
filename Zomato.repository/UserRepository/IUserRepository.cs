using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;

namespace Zomato.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<IdentityUser> GetUsernameByUserId(string userId);
        Task<IdentityResult> RegisterUser(IdentityUser user, string password);
        Task<SignInResult> Login(User user);
        Task<IdentityUser> GetUserDetail(string userId);
        Task<IdentityUser> FindByEmail(string email);
        Task<IdentityResult> AddUserToRole(IdentityUser user, string roleName);
        Task<string> GetUserRole(User user);
        Task<string> getUserRole(IdentityUser user);
        Task<List<IdentityUser>> GetUserList();

    }
}
