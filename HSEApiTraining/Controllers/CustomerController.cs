using System;
using HSEApiTraining.Models.Customer;
using Microsoft.AspNetCore.Mvc;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public GetCustomersResponse GetCustomers([FromQuery]int count)
        {
            try
            {
                if (count < 0)
                    throw new ArgumentException("Count less than 0.");

                var result = _customerService.GetCustomers(count);
                return new GetCustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new GetCustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }

        [HttpGet("GetAll")]
        public GetCustomersResponse GetAllCustomers()
        {
            try
            {
                var result = _customerService.GetCustomers(-1);

                return new GetCustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new GetCustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }

        [HttpGet("SearchByName/{searchTerm}")]
        public CustomersResponse SearchByName(string searchTerm)
        {
            try
            {
                var result = _customerService.SearchByName(searchTerm);
                return new CustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new CustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }

        [HttpGet("SearchBySurname/{searchTerm}")]
        public CustomersResponse SearchBySurname(string searchTerm)
        {
            try
            {
                var result = _customerService.SearchBySurname(searchTerm);
                return new CustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new CustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }

        [HttpPost]
        public AddCustomerResponse AddCustomer([FromBody] AddCustomerRequest request)
        {
            try
            {
                if (request.Name == null || request.Surname == null || request.PhoneNumber == null)
                    throw new ArgumentException("Name, Surname or Phone_number can't be empty.");

                return new AddCustomerResponse
                {
                    Error = _customerService.AddCustomer(request)
                };
            }
            catch(Exception e)
            {
                return new AddCustomerResponse
                {
                    Error = e.Message
                };
            }
            
        }

        [HttpPost("GenerateRandomCustomers")]
        public AddCustomerResponse AddRandomCustomers([FromBody] GeneratorRequest request)
        {
            try
            {
                if(request.сount < 0)
                        throw new ArgumentException("Count less than 0.");

                string error = null;
                for(int i = 0; i < request.сount; i++)
                {
                    try
                    {
                        error = _customerService.AddCustomer(CustomerService.GenerateCustomer());
                    }
                    catch(Exception ex)
                    {
                        i--;
                    }
                }

                return new AddCustomerResponse
                {
                    Error = error
                };
            }
            catch(Exception ex)
            {
                return new AddCustomerResponse
                {
                    Error = ex.Message
                };
            }
        }

        [HttpPut("{id}")]
        public UpdateCustomerResponse UpdateCustomer(int id, [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                if (request.Name == null || request.Surname == null || request.PhoneNumber == null)
                    throw new ArgumentException("Name, Surname or Phone_number can't be empty.");

                return new UpdateCustomerResponse
                {
                    Error = _customerService.UpdateCustomer(id, request)
                };
            }
            catch(Exception e)
            {
                return new UpdateCustomerResponse
                {
                    Error = e.Message
                };
            }
        }

        [HttpDelete("{id}")]
        public DeleteCustomerResponse DeleteCustomer(int id)
        {
            return new DeleteCustomerResponse
            {
                Error = _customerService.DeleteCustomer(id)
            };
        }

        [HttpDelete("DeleteAll")]
        public DeleteCustomerResponse DeleteAllCustomers()
        {
            return new DeleteCustomerResponse
            {
                Error = _customerService.DeleteAllCustomers()
            };
        }

        [HttpGet("getBanned")]
        public GetCustomersResponse GetCustomersWithBannedPhone()
        {
            try
            {
                var result = _customerService.GetCustomersWithBannedPhone();
                return new GetCustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new GetCustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }


        [HttpGet("getNotBanned")]
        public GetCustomersResponse GetCustomersWithNotBannedPhone()
        {
            try
            {
                var result = _customerService.GetCustomersWithNotBannedPhone();
                return new GetCustomersResponse
                {
                    Customers = result.Customers,
                    Error = result.Error
                };
            }
            catch (Exception ex)
            {
                return new GetCustomersResponse
                {
                    Customers = null,
                    Error = ex.Message
                };
            }
        }
    }
}
