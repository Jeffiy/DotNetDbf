using DotNetDbf;
using DotNetDbf.Attributes;
using System;

namespace ExampleModel
{
    [TableName("ARTICULO")]
    public sealed class Product
        : EntityBase
    {
        [ColumnName("CREF")]
        public string Code { get; set; }

        [ColumnName("CDETALLE")]
        public string Description { get; set; }

        [ColumnName("CTIPOIVA")]
        public string VatCode { get; set; }

        [ColumnName("NPVP")]
        public double ClearPrice { get; set; }

        [ColumnName("NPCONIVA")]
        public double PriceWithVat { get; set; }

        [ColumnName("DACTUALIZA")]
        public DateTime UpdateDate { get; set; }

        public override string ToString()
        {
            return Code + " - " + Description;
        }
    }
}
