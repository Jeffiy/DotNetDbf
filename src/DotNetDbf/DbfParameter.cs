
namespace DotNetDbf
{
    /// <summary>
    /// DbfParameter class
    /// </summary>
    public sealed class DbfParameter
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DbfParameter() { }

        /// <summary>
        /// Constructos
        /// </summary>
        /// <param name="fieldType">.NET FullName type</param>
        /// <param name="fieldName">BD field name</param>
        /// <param name="operation">Operator</param>
        /// <param name="value">Value of filter</param>
        public DbfParameter(string fieldType, string fieldName, string operation, string value)
        {
            this.FieldType = fieldType;
            this.FieldName = fieldName;
            this.Operator = operation;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets .NET FullName type
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// Gets or sets the database field name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// SQL operator. =, like, >=, etc.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Value of filter
        /// </summary>
        public string Value { get; set; }
    }
}
