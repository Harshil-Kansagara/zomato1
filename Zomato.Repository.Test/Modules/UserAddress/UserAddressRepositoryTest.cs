using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Zomato.Repository.DataRepository;
using Zomato.Repository.Test.Bootstrap;
using Zomato.Repository.UserAddressRepository;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;

namespace Zomato.Repository.Test.Modules.UserAddressTesting
{
    [Collection("Register Dependency")]
    public class UserAddressRepositoryTest
    {
        private Mock<IDataRepository> _dataRepositoryMock { get; }
        private IUserAddressRepository _userAddressRepository { get; }

        public UserAddressRepositoryTest(Initialize initialize)
        {
            _dataRepositoryMock = initialize.serviceProvider.GetService<Mock<IDataRepository>>();
            _userAddressRepository = initialize.serviceProvider.GetService<IUserAddressRepository>();
        }

        [Fact]
        public async Task AddAddress_Verify()
        {
            var address = new UserAddress();
            await _userAddressRepository.AddAddress(address);
            _dataRepositoryMock.Verify(x => x.AddAsync(address), Times.Once);
        }

        [Fact]
        public async Task DeleteAddress_Verify()
        {
            var givenAddressId = 1;
            var address = new UserAddress();
            _dataRepositoryMock.Setup(x => x.Find<UserAddress>(givenAddressId)).Returns(Task.FromResult(address));
            await _userAddressRepository.deleteAddress(givenAddressId);
            _dataRepositoryMock.Verify(x => x.Remove(address), Times.Once);
        }

        [Fact]
        public async Task EditAddress()
        {

        }

        [Fact]
        public async Task GetAddressList_IsNotEmpty()
        {
            var givenUserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242";
            var userAddressList = new List<UserAddress> {
                new UserAddress
                {
                    UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242",
                    AddressId = 1,
                    Address = "Vadodara"
                }
            };

            _dataRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<UserAddress, bool>>>())).Returns(userAddressList.AsQueryable().BuildMock().Object);

            var addressList = await _userAddressRepository.GetAddressList(givenUserId);
            Assert.NotEmpty(addressList);
        }

        [Fact]
        public async Task GetAddressNameById_IsEqual()
        {
            var expectedAddress = "Vadodara";
            var givenAddressId = 1;
            var userAddress = new UserAddress();
            userAddress.UserId = "21d632cb-a9bd-4ead-bd9b-e25957e1c242";
                    userAddress.AddressId = 1;
                    userAddress.Address = "Vadodara";

            _dataRepositoryMock.Setup(x => x.Find<UserAddress>(givenAddressId)).Returns(Task.FromResult(userAddress));
            var address = await _userAddressRepository.GetAddressNameById(givenAddressId);
            Assert.Equal(expectedAddress, address);
        }
    }
}
