using DotNetDbf.Attributes;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DotNetDbf
{
    public abstract class EntityBase
    {
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
