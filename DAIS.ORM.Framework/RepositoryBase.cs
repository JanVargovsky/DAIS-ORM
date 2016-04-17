using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIS.ORM.Framework
{
    public interface IRepositoryBase<TModel>
    {
        // Create
        bool Insert(TModel @object);

        // Read
        IEnumerable<TModel> Select();
        TModel Select(object id);

        // Update
        bool Update(TModel @object);

        // Delete
        bool Delete(object id);
        bool Delete(TModel @object);

    }

    public class RepositoryBase<TModel> : IRepositoryBase<TModel>
        where TModel : new()
    {
        private readonly IDatabase database;
        private readonly Type modelType;
        private readonly string tableName;

        private Dictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>();

        public RepositoryBase(IDatabase database)
        {
            Contract.Assert(database != null, $"{nameof(database)} is null");

            Contract.EndContractBlock();

            this.database = database;
            this.modelType = typeof(TModel);
            this.tableName = modelType.GetTableName();
            TypeMapInitialize();
        }

        #region IRepositoryBase

        public virtual bool Delete(TModel @object)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public virtual bool Insert(TModel @object)
        {
            try
            {
                database.Open();

                var attributes = @object.GetAttributeNameValues().ToArray();

                StringBuilder query = new StringBuilder()
                    .AppendLine($"INSERT INTO [{tableName}]({attributes.ToSqlNameParams()})")
                    .AppendLine($"VALUES ({attributes.ToSqlParamNames()});");

                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    FillParametersWithValues(command, attributes);

                    int result = command.ExecuteNonQuery();

                    return result == 1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                database.Close();
            }
        }

        public virtual IEnumerable<TModel> Select()
        {
            throw new NotImplementedException();
        }

        public virtual TModel Select(object id)
        {
            throw new NotImplementedException();
        }

        public virtual bool Update(TModel @object)
        {
            throw new NotImplementedException();
        }

        #endregion
        private void FillParametersWithValues(SqlCommand command, IEnumerable<AttributeValues> attributes)
        {
            foreach (var attr in attributes)
            {
                if (attr.Value == null)
                {
                    var sqlParameter = new SqlParameter(attr.ParameterName, typeMap[attr.ValueType])
                    {
                        Value = DBNull.Value
                    };
                    command.Parameters.Add(sqlParameter);
                }
                else
                    command.Parameters.AddWithValue(attr.ParameterName, attr.Value);
            }
        }

        private void TypeMapInitialize()
        {
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
        }
    }
}
