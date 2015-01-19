using DotNetDbf.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

namespace DotNetDbf
{
    /// <summary>
    /// Base class to create specific services to manage DBF tables.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DbfBase<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to DBF Database</param>
        public DbfBase(string path)
        {
            this.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBase III";
        }

        /// <summary>
        /// Complete connection string
        /// </summary>
        protected string ConnectionString
        {
            get;
            private set;
        }

        /// <summary>
        /// Get default SQL Query
        /// </summary>
        /// <returns>SQL String</returns>
        protected string GetDefaultSelectQuery()
        {
            return "select * from [" + this.getTableName() + "]";
        }

        /// <summary>
        /// ExecuteScalar wrapper.
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <returns>Object with result</returns>
        protected object ExecuteScalar(string sqlQuery)
        {
            return this.ExecuteScalar(sqlQuery, null);
        }

        /// <summary>
        /// ExecuteScalar wrapper.
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <returns>Object with result</returns>
        protected object ExecuteScalar(string sqlQuery, List<DbfParameter> filters)
        {
            object result = null;

            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();

                OleDbCommand cmd = getTextCommand(sqlQuery, cn, filters);
                result = cmd.ExecuteScalar();
                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// ExecuteQuery Wrapper
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <returns>IList with the query Result of type T</returns>
        protected IList<T> ExecuteQuery()
        {
            return this.ExecuteQuery(this.GetDefaultSelectQuery());
        }

        /// <summary>
        /// ExecuteQuery Wraper
        /// </summary>
        /// <param name="sqlQuery">SQL QUery</param>
        /// <param name="filter">Filter</param>
        /// <returns>IList with the query Result of type T</returns>
        protected IList<T> ExecuteQuery(DbfParameter filter)
        {
            List<DbfParameter> filters = new List<DbfParameter>();
            filters.Add(filter);

            return this.ExecuteQuery(this.GetDefaultSelectQuery(), filters);
        }

        /// <summary>
        /// ExecuteQuery Wrapper
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <returns>IList with the query Result of type T</returns>
        protected IList<T> ExecuteQuery(string sqlQuery)
        {
            return this.ExecuteQuery(sqlQuery, new List<DbfParameter>());
        }

        /// <summary>
        /// ExecuteQuery Wraper
        /// </summary>
        /// <param name="sqlQuery">SQL QUery</param>
        /// <param name="filter">Filter</param>
        /// <returns>IList with the query Result of type T</returns>
        protected IList<T> ExecuteQuery(string sqlQuery, DbfParameter filter)
        {
            List<DbfParameter> filters = new List<DbfParameter>();
            filters.Add(filter);

            return this.ExecuteQuery(sqlQuery, filters);
        }

        /// <summary>
        /// ExecuteQuery Wrapper
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <returns>IList with the query Result of type T</returns>
        protected IList<T> ExecuteQuery(string sqlQuery, List<DbfParameter> filters)
        {
            IList<T> result = null;

            using (OleDbConnection cn = new OleDbConnection(ConnectionString))
            {
                cn.Open();

                OleDbCommand cmd = getTextCommand(sqlQuery, cn, filters);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    result = new List<T>();

                    while (reader.Read())
                    {
                        result.Add(mapRow(reader));
                    }
                }

                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// Add a new record
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Affected Rows</returns>
        protected int Insert(T entity)
        {
            int result = 0;
            string baseInsert = "INSERT INTO {0} ({1}) VALUES ({2})";
            List<string> fieldList = getFieldsNameList(entity, false);
            List<string> valueList = getValueList(entity, false);

            string sqlQuery = string.Format(baseInsert, this.getTableName(), string.Join(",", fieldList.ToArray()), string.Join(",", valueList.ToArray()));

            using (OleDbConnection cn = new OleDbConnection(this.ConnectionString))
            {
                cn.Open();

                OleDbCommand cmd = this.getTextCommand(sqlQuery, cn, null);
                result = cmd.ExecuteNonQuery();

                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// Updates given entity
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>Affected Rows</returns>
        protected int Update(T entity)
        {
            int result = 0;
            string baseUpdate = "UPDATE {0} SET {1} WHERE {2}";

            List<string> fieldValueList = new List<string>();
            string whereFilter = string.Empty;

            entity.GetType().GetProperties().ToList().ForEach(property =>
            {
                if(this.isPropertyColumn(property))
                {
                    if (this.isPrimaryKeyColumn(property))
                    {
                        whereFilter = getColumnName(property) + "=" + getFormattedValue(property, entity);
                    }
                    else
                    {
                        fieldValueList.Add(getColumnName(property) + "=" + getFormattedValue(property, entity));
                    }
                }
            });

            string sqlQuery = string.Format(baseUpdate, this.getTableName(), string.Join(",", fieldValueList), whereFilter);

            using (OleDbConnection cn = new OleDbConnection(this.ConnectionString))
            {
                cn.Open();

                OleDbCommand cmd = this.getTextCommand(sqlQuery, cn, null);
                result = cmd.ExecuteNonQuery();

                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// Deletes given entity
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>Affected Rows</returns>
        protected int Delete(T entity)
        {
            int result = 0;
            string baseDelete = "DELETE FROM {0} WHERE {1}";

            string whereFilter = string.Empty;

            entity.GetType().GetProperties().ToList().ForEach(property =>
            {
                if (this.isPrimaryKeyColumn(property))
                {
                    whereFilter = getColumnName(property) + "=" + getFormattedValue(property, entity);
                }
            });

            string sqlQuery = string.Format(baseDelete, this.getTableName(), whereFilter);

            using (OleDbConnection cn = new OleDbConnection(this.ConnectionString))
            {
                cn.Open();

                OleDbCommand cmd = this.getTextCommand(sqlQuery, cn, null);
                result = cmd.ExecuteNonQuery();

                cn.Close();
            }

            return result;
        }

        /// <summary>
        /// Get value list of given object
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>List of values</returns>
        private List<string> getValueList(T entity, bool includeAutoIncrementColumns)
        {
            List<string> result = new List<string>();

            entity.GetType().GetProperties().ToList().ForEach(property =>
            {
                if(!includeAutoIncrementColumns && isAutoIncrementColumn(property))
                {
                    //Do nothing
                }
                else if (isPropertyColumn(property))
                {
                    result.Add(this.getFormattedValue(property, entity));
                }
            });

            return result;
        }

        /// <summary>
        /// Get the field name list of given object
        /// </summary>
        /// <param name="entity">Object</param>
        /// <returns>List of name list</returns>
        private List<string> getFieldsNameList(T entity, bool includeAutoIncrementColumns)
        {
            List<string> result = new List<string>();

            entity.GetType().GetProperties().ToList().ForEach(property =>
            {
                if (!includeAutoIncrementColumns && isAutoIncrementColumn(property))
                {
                    //Do nothing
                }
                else if (isPropertyColumn(property))
                {
                    result.Add(getColumnName(property));
                }
            });

            return result;
        }

        /// <summary>
        /// Check given property has mapping attribute
        /// </summary>
        /// <param name="property">Property Field</param>
        /// <returns>boolean</returns>
        private bool isPropertyColumn(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(ColumnNameAttribute));
        }

        /// <summary>
        /// Check given property has PK attribute
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>boolean</returns>
        private bool isPrimaryKeyColumn(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(PrimaryKeyAttribute));
        }

        /// <summary>
        /// Check given property is PK and is autoincrement
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>Boolean</returns>
        private bool isAutoIncrementColumn(PropertyInfo property)
        {
            if (Attribute.IsDefined(property, typeof(PrimaryKeyAttribute)))
            {
                var attr = property.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).First() as PrimaryKeyAttribute;
                return attr.AutoIncrement;
            }

            return false;
        }

        /// <summary>
        /// Gets column name assigned to given property
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>Column name</returns>
        private string getColumnName(PropertyInfo property)
        {
            var attr = property.GetCustomAttributes(typeof(ColumnNameAttribute), false).First() as ColumnNameAttribute;
            return attr.ColumnName;
        }

        /// <summary>
        /// Gets the table name
        /// </summary>
        /// <returns>Table Name</returns>
        private string getTableName()
        {
            T item = Activator.CreateInstance<T>();
            var attr = item.GetType().GetCustomAttributes(typeof(TableNameAttribute), false).ToList();

            if (attr.Count == 0)
            {
                throw new InvalidOperationException();
            }

            return (attr.First() as TableNameAttribute).TableName;
        }

        /// <summary>
        /// Maps the query
        /// </summary>
        /// <param name="reader">Input OleDbDataReader</param>
        /// <returns>Object of type T</returns>
        private T mapRow(OleDbDataReader reader)
        {
            T item = Activator.CreateInstance<T>();

            item.GetType().GetProperties().ToList().ForEach(property =>
            {
                if(isPropertyColumn(property))
                {
                    if (!System.DBNull.Value.Equals(reader[this.getColumnName(property)]))
                    {
                        property.SetValue(item, reader[this.getColumnName(property)], null);
                    }
                }
            });

            return item;
        }

        /// <summary>
        /// Create a new OleDbCommand
        /// </summary>
        /// <param name="sqlQuery">SQL Query</param>
        /// <param name="cn">OleDbConnection</param>
        /// <returns>OleDbCommand Object</returns>
        private OleDbCommand getTextCommand(string sqlQuery, OleDbConnection cn, List<DbfParameter> filters)
        {
            OleDbCommand cmd = cn.CreateCommand();
            cmd.CommandType = CommandType.Text;

            if (filters != null && filters.Count > 0)
            {
                sqlQuery += "where 0=0";

                filters.ForEach(filter =>
                {
                    string formattedValue = getFormattedValue(filter);

                    string f = " and " + filter.FieldName + " " + filter.Operator + " " + formattedValue;
                    sqlQuery += f;
                });
            }

            cmd.CommandText = sqlQuery;
            return cmd;
        }

        /// <summary>
        /// Gets SQL formatted of given DBFParameter
        /// </summary>
        /// <param name="filter">Parameter</param>
        /// <returns>String SQL formatted</returns>
        private string getFormattedValue(DbfParameter filter)
        {
            string delimiter = "";

            if (filter.FieldType == string.Empty.GetType().FullName)
            {
                delimiter = "'";
            }

            if (filter.FieldType == DateTime.MinValue.GetType().FullName)
            {
                delimiter = "#";
                filter.Value = DateTime.Parse(filter.Value).ToString("yyy-MM-dd");
            }

            return delimiter + filter.Value + delimiter;
        }

        /// <summary>
        /// Gets SQL formatted of given property
        /// </summary>
        /// <param name="property">Property</param>
        /// <param name="entity">Object</param>
        /// <returns>SQL formatted value</returns>
        private string getFormattedValue(PropertyInfo property, object entity)
        {
            string delimiter = "";
            object value = property.GetValue(entity, null);

            if (property.PropertyType.FullName == string.Empty.GetType().FullName)
            {
                delimiter = "'";
            }

            if (property.PropertyType.FullName == DateTime.MinValue.GetType().FullName)
            {
                delimiter = "#";
                value = DateTime.Parse(value.ToString()).ToString("yyy-MM-dd");
            }

            return delimiter + value.ToString() + delimiter;
        }
    }
}
