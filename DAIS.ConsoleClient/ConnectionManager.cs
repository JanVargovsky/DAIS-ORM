using DAIS.ORM;
using DAIS.ORM.Framework;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAIS.ConsoleClient
{
    interface ICreate<T>
    {
        T Create();
    }

    public class ConnectionManager : ICreate<IDatabase>
    {
        private static readonly ConnectionManager instance = new ConnectionManager();
        public static ConnectionManager Instance => instance;

        public IDatabase Create()
        {
            string connectionString = GetConnectionString();

            if (!CheckConnection(connectionString))
                throw new ApplicationException("Cant connect to database server");

            return new Database(connectionString);
        }

        private string GetConnectionString()
        {
            string connection = ConfigurationManager.AppSettings["connection"];

            if (connection == "local")
                return CreateLocalSqlConnectionString();
            else if (connection == "school")
                return CreateSchoolSqlConnectionString();

            throw new Exception("connection key in appsettings is missing");
        }

        private string CreateLocalSqlConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = @"C:\USERS\JANVA\DOCUMENTS\DAIS-VAR0065.MDF",
                IntegratedSecurity = true,
                ConnectTimeout = 10,
                Encrypt = false,
                TrustServerCertificate = true,
                ApplicationIntent = ApplicationIntent.ReadWrite,
                MultiSubnetFailover = false,
            }.ToString();
        }

        private string CreateSchoolSqlConnectionString()
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = @"dbsys.cs.vsb.cz\STUDENT",
                InitialCatalog = @"var0065",
                IntegratedSecurity = false,
                UserID = ConfigurationManager.AppSettings["user"],
                Password = ConfigurationManager.AppSettings["password"],
                ConnectTimeout = 15,
                Encrypt = false,
                TrustServerCertificate = false,
            }.ToString();
        }

        private bool CheckConnection(string connectionString)
        {
            DbConnection db = new SqlConnection(connectionString);
            try
            {
                db.Open();
                return true;
            }
            catch (SqlException e)
            {
                return false;
            }
            finally
            {
                db.Close();
            }
        }
    }
}
