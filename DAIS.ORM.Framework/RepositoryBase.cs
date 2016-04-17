﻿using System;
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
        TModel Select(params object[] ids);
        // TODO: Select by an expression?

        // Update
        bool Update(TModel @object);

        // Delete
        bool Delete(params object[] ids);
        bool Delete(TModel @object);

    }

    public class RepositoryBase<TModel> : IRepositoryBase<TModel>
        where TModel : class, new()
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

        #region Delete
        public virtual bool Delete(TModel @object)
        {
            var pks = modelType
                .GetAttributeNameValues()
                .Where(a => a.Type == ColumnType.PrimaryKey)
                .Select(a => a.Value);

            return Delete(pks);
        }

        public virtual bool Delete(params object[] ids)
        {
            if (ids.Length == 0)
                throw new ArgumentException($"{nameof(ids)} is missing");

            var attributes = modelType.GetAttributeNameValues().ToArray();
            var pks = attributes.Where(a => a.Type == ColumnType.PrimaryKey).ToArray();

            if (pks.Length != ids.Length)
                throw new ArgumentException("Different count of primary keys");

            // set value of attribute to that from parameter
            for (int i = 0; i < ids.Length; i++)
                pks[i].Value = ids[i];

            var softDeleteAttribute = attributes.FirstOrDefault(t => t.DeleteIndicator);

            if (softDeleteAttribute != null)
            {
                softDeleteAttribute.Value = true;
                return LogicalDelete(softDeleteAttribute, pks);
            }
            else
                return PhysicalDelete(pks);
        }

        private bool LogicalDelete(AttributeValues softDeleteAttribute, params AttributeValues[] pks)
        {
            try
            {
                StringBuilder query = new StringBuilder()
                    .AppendLine($"UPDATE [{tableName}]")
                    .AppendLine($"SET {softDeleteAttribute.ToSqlUpdateParam()}")
                    .AppendLine($"WHERE {pks.ToSqlWhereParams()};");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    FillParameterWithValue(command, softDeleteAttribute);
                    FillParametersWithValues(command, pks);

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

        private bool PhysicalDelete(params AttributeValues[] pks)
        {
            try
            {
                StringBuilder query = new StringBuilder()
                    .AppendLine($"DELETE FROM [{tableName}]")
                    .AppendLine($"WHERE {pks.ToSqlWhereParams()};");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    FillParametersWithValues(command, pks);

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

        #endregion

        #region Insert

        public virtual bool Insert(TModel @object)
        {
            try
            {
                var attributes = @object.GetAttributeNameValues();

                StringBuilder query = new StringBuilder()
                    .AppendLine($"INSERT INTO [{tableName}]({attributes.ToSqlNameParams()})")
                    .AppendLine($"VALUES ({attributes.ToSqlParamNames()});");

                database.Open();
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

        #endregion

        #region Select

        public virtual IEnumerable<TModel> Select()
        {
            var results = new List<TModel>();

            try
            {
                var attributes = modelType.GetAttributeNameValues();

                StringBuilder query = new StringBuilder()
                    .AppendLine($"SELECT {attributes.ToSqlNameParams()}")
                    .AppendLine($"FROM [{tableName}];");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        // TODO: Cache property info
                        while (reader.Read())
                            results.Add(CreateModelFromReader(reader, attributes));

                        return results;
                    }
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

        public virtual TModel Select(params object[] ids)
        {
            if (ids.Length == 0)
                throw new ArgumentException($"{nameof(ids)} is missing");

            try
            {
                var attributes = modelType.GetAttributeNameValues();
                var pks = attributes.Where(a => a.Type == ColumnType.PrimaryKey).ToArray();

                if (pks.Length != ids.Length)
                    throw new ArgumentException("Different count of primary keys");

                StringBuilder query = new StringBuilder()
                    .AppendLine($"SELECT {attributes.ToSqlNameParams()}")
                    .AppendLine($"FROM [{tableName}]")
                    .AppendLine($"WHERE {pks.ToSqlWhereParams()};");

                // set value of attribute to that from parameter
                for (int i = 0; i < ids.Length; i++)
                    pks[i].Value = ids[i];

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    FillParametersWithValues(command, pks);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return CreateModelFromReader(reader, attributes);
                    }
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

        private TModel CreateModelFromReader(SqlDataReader reader, IEnumerable<AttributeValues> attributes)
        {
            TModel result = new TModel();

            int i = 0;
            foreach (var attr in attributes)
            {
                var value = reader.GetValue(i++);
                if (value != DBNull.Value)
                    modelType.GetProperty(attr.PropertyName).SetValue(result, value);
            }

            return result;
        }

        #endregion

        #region Update

        public virtual bool Update(TModel @object)
        {
            try
            {
                var attributes = @object.GetAttributeNameValues();
                var pks = attributes.Where(a => a.Type == ColumnType.PrimaryKey);
                var updateAttributes = attributes.Where(a => a.Type != ColumnType.PrimaryKey);

                StringBuilder query = new StringBuilder()
                    .AppendLine($"UPDATE [{tableName}]")
                    .AppendLine($"SET {updateAttributes.ToSqlUpdateParams()}")
                    .AppendLine($"WHERE {pks.ToSqlWhereParams()};");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    FillParametersWithValues(command, updateAttributes);
                    FillParametersWithValues(command, pks);

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

        #endregion

        #region Helper methods of SqlCommand

        private void FillParameterWithValue(SqlCommand command, AttributeValues attr)
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

        private void FillParametersWithValues(SqlCommand command, IEnumerable<AttributeValues> attributes)
        {
            foreach (var attr in attributes)
                FillParameterWithValue(command, attr);
        }

        #endregion

        #endregion

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
        }
    }
}
