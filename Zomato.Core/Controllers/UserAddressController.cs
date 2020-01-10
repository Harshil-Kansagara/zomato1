using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zomato.DomainModel.Models;
using Zomato.Repository.UnitofWork;

namespace Zomato.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public UserAddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{userId}/address")]
        public async Task<List<UserAddress>> GetAddressList(string userId)
        {
            return await _unitOfWork.UserAddressRepository.GetAddressList(userId);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress (UserAddress newUserAddress)
        {
            if (ModelState.IsValid) {
                if (newUserAddress.AddressId == 0) { 
                    var userAddressList = _unitOfWork.UserAddressRepository.GetAddressList(newUserAddress.UserId).Result;
                    foreach (var each in userAddressList)
                    {
                        if (each.Address == newUserAddress.Address && each.UserId == newUserAddress.UserId)
                        {
                            return BadRequest();
                        }
                    }
                    await _unitOfWork.UserAddressRepository.AddAddress(newUserAddress);
                    _unitOfWork.commit();
                    return Ok();
                }
                else
                {
                    var userAddressList = _unitOfWork.UserAddressRepository.GetAddressList(newUserAddress.UserId).Result;
                    foreach (var each in userAddressList)
                    {
                        if (each.Address == newUserAddress.Address)
                        {
                            return BadRequest();
                        }
                    }
                    await _unitOfWork.UserAddressRepository.EditAddress(newUserAddress);
                    _unitOfWork.commit();
                    return Ok();

                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{addressId}")]
        public async Task DeleteAddress(int addressId)
        {
            try
            {
                await _unitOfWork.UserAddressRepository.deleteAddress(addressId);
                _unitOfWork.commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
