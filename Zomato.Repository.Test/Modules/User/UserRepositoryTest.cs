using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.Test.Bootstrap;
using Zomato.Repository.UserRepository;
using Microsoft.AspNetCore.Identity;
using Zomato.DomainModel.Models;
using System.Threading.Tasks;
using System.Linq;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;

namespace Zomato.Repository.Test.Modules.UserTesting
{
    [Collection("Register Dependency")]
    public class UserRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private Mock<UserManager<IdentityUser>> _userManager { get; }
        private Mock<SignInManager<IdentityUser>> _signInManager { get; }
        private IUserRepository _userRepository { get; }

        public UserRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _userManager = initialize.serviceProvider.GetService<Mock<UserManager<IdentityUser>>>();
            _signInManager = initialize.serviceProvider.GetService<Mock<SignInManager<IdentityUser>>>();
            _userRepository = initialize.serviceProvider.GetService<IUserRepository>();
            _userManager.Reset();
        }

        [Fact]
        public async Task AddUserToRole_Verify()
        {
            var user = new IdentityUser();
            var roleName = "36bf775a-621e-4a6d-92b5-5263da58882c";
            await _userRepository.AddUserToRole(user,roleName);
            _userManager.Verify(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task FindByEmail_IsNotNull()
        {
            var email = "harshil@gmail.com";
            var user = new IdentityUser();
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            var userData = await _userRepository.FindByEmail(email);
            Assert.NotNull(userData);
        }

        [Fact]
        public async Task GetUserDetail_IsNotNull()
        {
            var userId = "36bf775a-621e-4a6d-92b5-5263da58882c";
            var user = new IdentityUser();
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            var userData = await _userRepository.GetUserDetail(userId);
            Assert.NotNull(userData);
        }

        [Fact]
        public async Task GetUsernameByUserId_IsNotNull()
        {
            var userId = "36bf775a-621e-4a6d-92b5-5263da58882c";
            var user = new IdentityUser();
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            var userData = await _userRepository.GetUsernameByUserId(userId);
            Assert.NotNull(userData);
        }

        //Not Working
        [Fact]
        public async Task GetUserList_IsNotEmpty()
        {
            var userList = new List<IdentityUser> {
                new IdentityUser()
                {
                    Id = "36bf775a-621e-4a6d-92b5-5263da58882c"
                }
            };  
            _userManager.Setup(x => x.Users).Returns(userList.AsQueryable().BuildMock().Object);
            var users = await _userRepository.GetUserList();
            Assert.NotNull(users);
        }

        [Fact]
        public async Task GetUserRole_NotNull()
        {
            var user = new IdentityUser();
            var userData = new User();
            userData.UserEmailAddress = "harshil@gmail.com";
            var role = new List<string> { "admin" };
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).Returns(Task.FromResult((IList<string>)role));

            var roleData = await _userRepository.GetUserRole(userData);
            Assert.NotNull(roleData);
        }

        [Fact]
        public async Task getUserRole_IsNotNull()
        {
            var user = new IdentityUser();
            user.Email = "harshil@gmail.com";
            var role = new List<string> { "admin" };
            _userManager.Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>())).Returns(Task.FromResult((IList<string>)role));
            var roleData = await _userRepository.getUserRole(user);
            Assert.NotNull(roleData);
        }

        [Fact]
        public async Task Login_Veriify()
        {
            var user = new IdentityUser();
            var userData = new User();
            userData.UserEmailAddress = "harshil@gmail.com";
            userData.UserPassword = "Harshil@123";
            _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((string userName, string password, bool isPersistent, bool lockoutOnFailure) =>
            {
                return Task.FromResult(SignInResult.Success);
            });
            var signInUser = await _userRepository.Login(userData);
            Assert.NotNull(signInUser);
        }

        [Fact]
        public async Task Register_Verify()
        {
            var user = new IdentityUser();
            var password = "Harshil@123";
            await _userRepository.RegisterUser(user, password);
            _userManager.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
        }
    }
}
