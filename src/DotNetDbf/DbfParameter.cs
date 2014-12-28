using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDbf
{
    public sealed class DbfParameter
    {
        public DbfParameter() { }

        public DbfParameter(string fieldType, string fieldName, string operation, string value)
        {
            this.FieldType = fieldType;
            this.FieldName = fieldName;
            this.Operator = operation;
            this.Value = value;
        }

        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
