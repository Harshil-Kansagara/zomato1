using Microsoft.AspNetCore.Authorization;
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
    public class AdminController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly ApplicationSettingModel _appSettings;

        public AdminController(IUnitOfWork unitOfWork, IOptions<ApplicationSettingModel> appSettings)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login(User user)
        {
            if (ModelState.IsValid)
            {
                string userRole = await _unitOfWork.UserRepository.GetUserRole(user);
                if (userRole == "admin")
                {
                    var result = await _unitOfWork.UserRepository.Login(user);
                    if (result.Succeeded)
                    {
                        var userData = await _unitOfWork.UserRepository.FindByEmail(user.UserEmailAddress);
                        var claims = new List<Claim> {
                                 new Claim("UserId", userData.Id.ToString()),
                                  new Claim("UserName", userData.UserName.ToString()),
                                  new Claim("UserRole", "admin"),
                                  new Claim(ClaimTypes.Name, userData.Id.ToString()),
                                  new Claim(ClaimTypes.Role, "admin")
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
                } else if(userRole == "user")
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
