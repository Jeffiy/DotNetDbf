using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDbf.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TableNameAttribute : Attribute
    {
        private readonly string _tableName;

        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }

        public string TableName
        {
            get { return _tableName; }
        }
    }
}
