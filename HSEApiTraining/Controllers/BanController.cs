using HSEApiTraining.Models.BannedPhone;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        private readonly IBanService _banService;

        public BanController(IBanService banService)
        {
            _banService = banService;
        }

        [HttpPost]
        public AddBannedPhoneResponse AddBannedPhone([FromBody] AddBannedPhoneRequest request)
        {
            try
            {
                CheckPhoneNumber(request.Phone);

                return new AddBannedPhoneResponse
                {
                    Error = _banService.AddBannedPhone(request)
                };
            }
            catch(Exception ex)
            {
                return new AddBannedPhoneResponse
                {
                    Error = ex.Message
                };
            }
        }


        [HttpGet]
        public GetBannedPhonesResponse GetBannedPhones()
        {
            try
            {
                var result = _banService.GetBannedPhones();
                return new GetBannedPhonesResponse
                {
                    BannedPhones = result.BannedPhones,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new GetBannedPhonesResponse
                {
                    BannedPhones = null,
                    Error = ex.Message
                };
            }
        }

        
        [HttpDelete("{id}")]
        public DeleteBannedPhoneResponse DeleteBannedPhone(int id)
        {
            return new DeleteBannedPhoneResponse
            {
                Error = _banService.DeleteBannedPhone(id)
            };
        }


        [HttpDelete("DeleteAll")]
        public DeleteBannedPhoneResponse DeleteAllCustomers()
        {
            return new DeleteBannedPhoneResponse
            {
                Error = _banService.DeleteAllBannedPhones()
            };
        }

        /// <summary>
        /// Проверяет валидность вводимого телефона.
        /// </summary>
        /// <param name="phone"> Телефон. </param>
        private static void CheckPhoneNumber(string phone)
        {
            if (phone == null)
                throw new ArgumentException("Phone can't be empty.");

            if (phone.Length < 2 || phone.Length > 13 || phone[0] != '+' ||
                !phone.Substring(1).All(c => "9876543210".Contains(c)))
            {
                throw new ArgumentException("Phone has to be from digits and starts with + " +
                    "and lenght need to be from 2 to 13 nums.");
            }
        }
    }
}
