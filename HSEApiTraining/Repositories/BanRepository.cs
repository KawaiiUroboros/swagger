using Dapper;
using HSEApiTraining.Models.BannedPhone;
using HSEApiTraining.Providers;
using System;
using System.Collections.Generic;

namespace HSEApiTraining
{
    public interface IBanRepository
    {
        (IEnumerable<BannedPhone> BannedPhones, string Error) GetBannedPhones();
        string AddBannedPhone(AddBannedPhoneRequest request);
        string DeleteBannedPhone(int id);
        string DeleteAllBannedPhones();
    }

    public class BanRepository : IBanRepository
    {
        private readonly ISQLiteConnectionProvider _connectionProvider;
        public BanRepository(ISQLiteConnectionProvider sqliteConnectionProvider)
        {
            _connectionProvider = sqliteConnectionProvider;
        }

        public string AddBannedPhone(AddBannedPhoneRequest request)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    int exists = connection.Query<BannedPhone>(
                            $@"SELECT 
                                id as Id,
                                Phone as Phone
                                FROM Banned_Phone
                                WHERE Phone = @Phone;",
                                new { Phone = request.Phone }, 
                                null).AsList().Count;

                    if (exists > 0)
                        throw new ArgumentException("This Phone is already added.");

                    connection.Execute(
                        @"INSERT INTO Banned_phone
                        ( Phone ) VALUES 
                        ( @Phone );",
                        new { Phone = request.Phone });
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public (IEnumerable<BannedPhone> BannedPhones, string Error) GetBannedPhones()
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    return (
                        connection.Query<BannedPhone>(@"
                        SELECT 
                        id as Id,
                        Phone as Phone
                        FROM Banned_Phone"),
                        null);
                }
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public string DeleteBannedPhone(int id)
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    int rowsaffected = connection.Execute(
                        @"DELETE FROM Banned_Phone WHERE id = @Id;",
                        new { Id = id });

                    if (rowsaffected == 0)
                        throw new NullReferenceException("There is no Phone with such id.");
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string DeleteAllBannedPhones()
        {
            try
            {
                using (var connection = _connectionProvider.GetDbConnection())
                {
                    connection.Open();
                    connection.Execute(
                        @"DELETE FROM Banned_Phone");
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
