using DotNetDbf;
using DotNetDbf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleModel
{
    [TableName("Ivas")]
    public sealed class Vat
        : EntityBase
    {
        [ColumnName("CTIPOIVA")]
        [PrimaryKey(false)]
        public string Code { get; set; }

        [ColumnName("CDETIVA")]
        public string Description { get; set; }

        [ColumnName("NPORCIVA")]
        public double Percentage { get; set; }

        public override string ToString()
        {
            return Code + " - " + Description + " / " + Percentage.ToString();
        }
    }
}
