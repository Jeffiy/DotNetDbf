using DotNetDbf;
using DotNetDbf.Attributes;

namespace ExampleModel
{
    [TableName("zonas")]
    public sealed class Zone
        : EntityBase
    {
        [ColumnName("CCODZONA")]
        [PrimaryKey(false)]
        public string Code { get; set; }

        [ColumnName("CNOMZONA")]
        public string Description { get; set; }

        public override string ToString()
        {
            return Code + " - " + Description;
        }
    }
}
