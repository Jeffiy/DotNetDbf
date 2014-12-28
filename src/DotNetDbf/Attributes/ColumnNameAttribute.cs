using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDbf.Attributes
{
    /// <summary>
    /// Column mapper attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">DB column name</param>
        public ColumnNameAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        /// <summary>
        /// DB column name
        /// </summary>
        public string ColumnName 
        {
            get;
            private set;
        }
    }
}
