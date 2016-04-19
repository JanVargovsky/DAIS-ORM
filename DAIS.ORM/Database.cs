using DAIS.ORM.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAIS.ORM
{
    public sealed class Database : IDatabase
    {
        private readonly SqlConnection connection;

        public Database(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            connection = new SqlConnection(connectionString);

            connection.InfoMessage += (o, e) => Console.WriteLine(e.Message);
        }

        public void Open() => connection.Open();

        public void Close() => connection.Close();

        public SqlCommand CreateSqlCommand(string sql = "") => new SqlCommand(sql, connection);

        public void Dispose()
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }
    }
}
