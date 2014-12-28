using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDbf.Attributes
{
    /// <summary>
    /// Primary key mapper attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="autoIncrement">Is auto increment column?</param>
        public PrimaryKeyAttribute(bool autoIncrement)
        {
            this.AutoIncrement = autoIncrement;
        }

        /// <summary>
        /// Autoincrement column.
        /// </summary>
        public bool AutoIncrement 
        { 
            get; 
            private set; 
        }
    }
}
