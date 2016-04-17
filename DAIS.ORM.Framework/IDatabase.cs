using System.Data.SqlClient;

namespace DAIS.ORM.Framework
{
    public interface IDatabase
    {
        void Open();
        void Close();
        SqlCommand CreateSqlCommand(string sql = "");
    }
}
