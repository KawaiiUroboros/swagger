using Dapper;
using HSEApiTraining.Models.Customer;
using HSEApiTraining.Providers;
using System;
using System.Collections.Generic;

namespace HSEApiTraining
{
    public interface ICustomerRepository
    {
        (IEnumerable<Customer> Customers, string Error) GetCustomers(int count);
        (IEnumerable<Customer> Customers, string Error) SearchByName(string searchTerm);
        (IEnumerable<Customer> Customers, string Error) SearchBySurname(string searchTerm);
        string AddCustomer(AddCustomerRequest request);
        string DeleteCustomer(int id);
        string UpdateCustomer(int id, UpdateCustomerRequest request);
        string DeleteAllCustomers();
        (IEnumerable<Customer> Customers, string Error) GetCustomersWithBannedPhone();
        (IEnumerable<Customer> Customers, string Error) GetCustomersWithNotBannedPhone();
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISQLiteConnectionProvider _connectionProvider;
        public CustomerRepository(ISQLiteConnectionProvider sqliteConnectionProvider)
        {
            _connectionProvider = sqliteConnectionProvider;
        }

        public (IEnumerable<Customer> Customers, string Error) GetCustomers(int count)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<Customer>(@"
                        SELECT 
                        id as Id,
                        name as Name, 
                        surname as Surname, 
                        phone_number as PhoneNumber 
                        FROM Customer 
                        LIMIT @count",
                        new { count = count }),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public string AddCustomer(AddCustomerRequest request)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    connection.Execute(
                        @"INSERT INTO Customer 
                        ( name, surname, phone_number ) VALUES 
                        ( @Name, @Surname, @PhoneNumber );",
                        new { Name = request.Name, Surname = request.Surname, PhoneNumber = request.PhoneNumber });
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DeleteCustomer(int id)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    int rowsaffected = connection.Execute(
                        @"DELETE FROM Customer WHERE id = @Id;",
                        new { Id = id });

                    if (rowsaffected == 0)
                        throw new NullReferenceException("There is no person with such id.");
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DeleteAllCustomers()
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    connection.Execute(
                        @"DELETE FROM Customer");
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public (IEnumerable<Customer> Customers, string Error) SearchByName(string searchTerm)
        {
            try
            {
                searchTerm = $"%{searchTerm}%";
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<Customer>(@"
                        SELECT 
                        id as Id,
                        name as Name, 
                        surname as Surname, 
                        phone_number as PhoneNumber 
                        FROM Customer 
                        WHERE Name 
                        LIKE @searchTerm",
                        new { searchTerm = searchTerm }),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public (IEnumerable<Customer> Customers, string Error) SearchBySurname(string searchTerm)
        {
            try
            {
                searchTerm = $"%{searchTerm}%";
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<Customer>($@"
                        SELECT 
                        id as Id,
                        name as Name, 
                        surname as Surname, 
                        phone_number as PhoneNumber 
                        FROM Customer 
                        WHERE Surname
                        LIKE @searchTerm",
                        new { searchTerm = searchTerm }),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public string UpdateCustomer(int id, UpdateCustomerRequest request)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    int rowsaffected = connection.Execute(
                        @"UPDATE Customer 
                        SET name = @Name, surname = @Surname, phone_number = @PhoneNumber
                        WHERE id = @Id;",
                        new { Name = request.Name, Surname = request.Surname, PhoneNumber = request.PhoneNumber, Id = id });

                    if (rowsaffected == 0)
                        throw new NullReferenceException("There is no person with such id.");
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public (IEnumerable<Customer> Customers, string Error) GetCustomersWithBannedPhone()
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<Customer>(@"
                        SELECT 
                        Customer.id as Id,
                        Customer.name as Name, 
                        Customer.surname as Surname, 
                        Customer.phone_number as PhoneNumber 
                        FROM Customer 
                        INNER JOIN Banned_phone
                        ON Customer.phone_number = Banned_Phone.Phone"),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public (IEnumerable<Customer> Customers, string Error) GetCustomersWithNotBannedPhone()
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<Customer>(@"
                        SELECT 
                        Customer.id as Id,
                        Customer.name as Name, 
                        Customer.surname as Surname, 
                        Customer.phone_number as PhoneNumber 
                        FROM Customer 
                        EXCEPT
                        SELECT 
                        Customer.id as Id,
                        Customer.name as Name, 
                        Customer.surname as Surname, 
                        Customer.phone_number as PhoneNumber 
                        FROM Customer 
                        INNER JOIN Banned_phone
                        ON Customer.phone_number = Banned_Phone.Phone"),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }
    }
}
