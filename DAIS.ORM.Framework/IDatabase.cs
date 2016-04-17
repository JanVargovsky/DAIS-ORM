using System;
using System.Data.SqlClient;

namespace DAIS.ORM.Framework
{
    public interface IDatabase : IDisposable
    {
        void Open();
        void Close();
        SqlCommand CreateSqlCommand(string sql = "");
    }
}
