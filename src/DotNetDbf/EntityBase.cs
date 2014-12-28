using DotNetDbf.Attributes;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DotNetDbf
{
    /// <summary>
    /// Entity base class used for Entity classes
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets the field name of given property
        /// </summary>
        /// <param name="exp">Lambda expression</param>
        /// <returns>String with field name</returns>
        public string GetFieldName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            var attr = body.Member.GetCustomAttributes(typeof(ColumnNameAttribute), false).ToList();
            if (attr.Count == 0)
            {
                throw new InvalidOperationException();
            }

            return (attr.First() as ColumnNameAttribute).ColumnName;
        }

        /// <summary>
        /// Gets the .NET data type of given property
        /// </summary>
        /// <param name="exp">Lambda expression</param>
        /// <returns>String with FullName Type</returns>
        public string GetFieldType(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Type.FullName;
        }
    }
}
