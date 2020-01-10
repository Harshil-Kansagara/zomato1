using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserRepository( UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserToRole(IdentityUser user, string roleName)
        {
           return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityUser> FindByEmail(string email)
        {
           var a = await _userManager.FindByEmailAsync(email);
            if (a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<IdentityUser> GetUserDetail(string userId)
        {
            var a = await _userManager.FindByIdAsync(userId);
            if(a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<List<IdentityUser>> GetUserList()
        {
            var a = await _userManager.Users.ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<IdentityUser> GetUsernameByUserId(string userId)
        {
            var a = await _userManager.FindByIdAsync(userId);
            if (a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<string> GetUserRole(User user)
        {
            var user1 = await _userManager.FindByEmailAsync(user.UserEmailAddress);
            if(user1 == null)
            {
                return null;
            }
            else
            { 
                var result = await _userManager.GetRolesAsync(user1);
                if(result == null)
                {
                    return null;
                }
                return result[0];
            }
        }

        public async Task<string> getUserRole(IdentityUser user)
        {
            var result = await _userManager.GetRolesAsync(user);
            if(result == null)
            {
                return null;
            }
            return result[0];
        }

        public async Task<SignInResult> Login(User userData)
        {
            var user = await _userManager.FindByEmailAsync(userData.UserEmailAddress);
            var a = await _signInManager.PasswordSignInAsync(user.UserName, userData.UserPassword, false, false);
            if(a == null)
            {
                return null;
            }
            return a;
        }

        public async Task<IdentityResult> RegisterUser(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
