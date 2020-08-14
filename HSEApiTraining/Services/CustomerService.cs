using HSEApiTraining.Models.Customer;
using System.Text;
using System;
using System.Collections.Generic;

namespace HSEApiTraining
{
    public interface ICustomerService
    {
        (IEnumerable<Customer> Customers, string Error) GetCustomers(int count);
        (IEnumerable<Customer> Customers, string Error) SearchByName(string searchTerm);
        (IEnumerable<Customer> Customers, string Error) SearchBySurname(string searchTerm);
        string AddCustomer(AddCustomerRequest request);
        string UpdateCustomer(int id, UpdateCustomerRequest request);
        string DeleteCustomer(int id);
        string DeleteAllCustomers();
        (IEnumerable<Customer> Customers, string Error) GetCustomersWithBannedPhone();
        (IEnumerable<Customer> Customers, string Error) GetCustomersWithNotBannedPhone();
    }

    public class CustomerService : ICustomerService
    {
        private static Random rnd = new Random();

        private ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
            => _customerRepository = customerRepository;

        public (IEnumerable<Customer> Customers, string Error) GetCustomers(int count)
            => _customerRepository.GetCustomers(count);

        public string AddCustomer(AddCustomerRequest request)
            => _customerRepository.AddCustomer(request);

        public string DeleteCustomer(int id)
            => _customerRepository.DeleteCustomer(id);

        public string DeleteAllCustomers()
            => _customerRepository.DeleteAllCustomers();

        public string UpdateCustomer(int id, UpdateCustomerRequest request)
            => _customerRepository.UpdateCustomer(id, request);

        public (IEnumerable<Customer> Customers, string Error) SearchByName(string searchTerm)
            => _customerRepository.SearchByName(searchTerm);

        public (IEnumerable<Customer> Customers, string Error) SearchBySurname(string searchTerm)
            => _customerRepository.SearchBySurname(searchTerm);

        public (IEnumerable<Customer> Customers, string Error) GetCustomersWithBannedPhone()
            => _customerRepository.GetCustomersWithBannedPhone();

        public (IEnumerable<Customer> Customers, string Error) GetCustomersWithNotBannedPhone()
            => _customerRepository.GetCustomersWithNotBannedPhone();


        /// <summary>
        /// Генерирует запрос для рандомного пользователя.
        /// </summary>
        /// <returns> Запрос. </returns>
        public static AddCustomerRequest GenerateCustomer()
        {
            return  new AddCustomerRequest
            {
                Name = NamesForCustomers[rnd.Next(0, NamesForCustomers.Length)],
                Surname = SurnamesForCustomers[rnd.Next(0, SurnamesForCustomers.Length)],
                PhoneNumber = GenerateNumber()
            };
        }

        private static string[] NamesForCustomers =
        {
            "Mike", "Ira", "Nastya", "Alina", "Petya", "Andrew", "Daniil", "Anna", "Victoria", "Mikhail"
        };

        private static string[] SurnamesForCustomers =
        {
            "Petrov", "Zyl", "Chirikova", "Opeyki", "Perviy", "Stepanov", "Mohrov", "Ioannova", "Winner", "Shadrin"
        };

        private static string[] start = { "+7", "+380", "+0", "+1" };

        /// <summary>
        /// Генерирует номер телефона.
        /// </summary>
        /// <returns> Номер телефона. </returns>
        private static string GenerateNumber()
        {
            StringBuilder sb = new StringBuilder(start[rnd.Next(0, start.Length)]);
            for(int i = 0; i < 8; i++)
            {
                sb.Append(rnd.Next(0, 10));
            }

            return sb.ToString();
        }
    }
}
