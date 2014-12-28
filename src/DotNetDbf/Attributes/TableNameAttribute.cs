using System;

namespace DotNetDbf.Attributes
{
    /// <summary>
    /// DB table name attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TableNameAttribute : Attribute
    {
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="tableName">DB table name</param>
        public TableNameAttribute(string tableName)
        {
            this.TableName = tableName;
        }

        /// <summary>
        /// DB table name
        /// </summary>
        public string TableName
        {
            get;
            private set;
        }
    }
}
