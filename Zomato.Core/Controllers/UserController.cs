using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly ApplicationSettingModel _appSettings;

        public UserController(IUnitOfWork unitOfWork, IOptions<ApplicationSettingModel> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("register")]
        //POST: api/User/register
        public async Task<Object> AddUser(User newUser)
        {
            if (ModelState.IsValid) {
                var user = new IdentityUser
                {
                    UserName = newUser.UserName,
                    Email = newUser.UserEmailAddress,
                    PhoneNumber = newUser.UserMobileNumber
                };

                try
                {
                    var result = await _unitOfWork.UserRepository.RegisterUser(user, newUser.UserPassword);
                    if (result.Succeeded) {
                        result = await _unitOfWork.UserRepository.AddUserToRole(user, newUser.UserRole);
                    }
                    _unitOfWork.commit();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login (User user)
        {
            if (ModelState.IsValid)
            {
                string userRole = await _unitOfWork.UserRepository.GetUserRole(user);
                if(userRole == "admin") {
                    return BadRequest();
                }
                else if(userRole == "user")
                {
                    var result = await _unitOfWork.UserRepository.Login(user);
                    if (result.Succeeded)
                    {
                        var userData = await _unitOfWork.UserRepository.FindByEmail(user.UserEmailAddress);
                        var claims = new List<Claim> {
                                 new Claim("UserId", userData.Id.ToString()),
                                  new Claim("UserName", userData.UserName.ToString()),
                                  new Claim("UserRole", "user"),
                                  new Claim(ClaimTypes.Name, userData.Id.ToString()),
                                  new Claim(ClaimTypes.Role, "user")
                        };
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
                        var tokenOptions = new JwtSecurityToken
                        (
                            issuer: "http://localhost/59227",
                            audience: "http://localhost/59227",
                            claims: claims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: signinCredentials
                        );
                        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                        return Ok(new { token });
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IdentityUser> GetUserDetail(string userId)
        {
            return await _unitOfWork.UserRepository.GetUserDetail(userId);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public async Task<List<IdentityUser>> GetUserList(string userId)
        {
            List<IdentityUser> identityUsers = new List<IdentityUser>();
                
            var userList = await _unitOfWork.UserRepository.GetUserList();

            foreach (var user in userList)
            {
                var model = new IdentityUser();
                string userRole = await _unitOfWork.UserRepository.getUserRole(user);
                if(userRole == "user")
                {
                    if(user.Id != userId)
                    {
                        identityUsers.Add(user);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            return identityUsers;

        }
    }
}
