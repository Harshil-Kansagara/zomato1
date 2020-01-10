using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Data;
using Zomato.DomainModel.Models;
using Zomato.Repository.DataRepository;

namespace Zomato.Repository.UserAddressRepository
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private IDataRepository _dataRepository;

        public UserAddressRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<UserAddress> AddAddress(UserAddress userAddress)
        {
            await _dataRepository.AddAsync(userAddress);
            return userAddress;
        }

        public async Task deleteAddress(int addressId)
        {
            var address = await _dataRepository.Find<UserAddress>(addressId);
            if (address != null)
            {
                _dataRepository.Remove(address);
            }
        }

        public async Task EditAddress(UserAddress userAddress)
        {
            var address = await _dataRepository.Where<UserAddress>(x => x.AddressId == userAddress.AddressId).ToListAsync();
            address.ForEach(
                    x => {
                   x.Address = userAddress.Address;
               }
             );
        }

        public async Task<List<UserAddress>> GetAddressList(string userId)
        {
            var a = await _dataRepository.Where<UserAddress>(x => x.UserId == userId).ToListAsync();
            if(a.Count == 0)
            {
                return null;
            }
            return a;
        }

        public async Task<string> GetAddressNameById(int addressId)
        {
            var address = await _dataRepository.Find<UserAddress>(addressId);
            if(address == null)
            {
                return null;
            }
            return address.Address;
        }
    }
}
