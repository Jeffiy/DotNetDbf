using DotNetDbf;
using ExampleModel;
using System;
using System.Collections.Generic;

namespace ExampleServices
{
    /// <summary>
    /// Product entity data services
    /// </summary>
    public class ProductsService
        : DbfBase<Product>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to DBF database</param>
        public ProductsService(string path)
            : base(path)
        {

        }

        /// <summary>
        /// Gets the product table count
        /// </summary>
        /// <returns>Number of rows</returns>
        public int GetProductsCount()
        {
            return (int)base.ExecuteScalar("select count(*) from [articulo]");
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>List of products</returns>
        public IList<Product> GetAllProducts()
        {
            return base.ExecuteQuery(base.GetDefaultSelectQuery());
        }

        /// <summary>
        /// Find products in table
        /// </summary>
        /// <param name="code">Code filter</param>
        /// <param name="description">Description filter</param>
        /// <param name="clearPrice">ClearPrice filter</param>
        /// <param name="updateDate">Update date filter</param>
        /// <returns>List of products</returns>
        public IList<Product> FindProducts(string code, string description, double? clearPrice, DateTime? updateDate)
        {
            Product d = new Product();
            List<DbfParameter> filters = new List<DbfParameter>();

            if (!string.IsNullOrEmpty(code))
            {
                filters.Add(new DbfParameter
                {
                    FieldType = d.GetFieldType(() => d.Code),
                    FieldName = d.GetFieldName(() => d.Code),
                    Operator = "like",
                    Value = code + "%"
                });
            }

            if (!string.IsNullOrEmpty(description))
            {
                filters.Add(new DbfParameter
                {
                    FieldType = d.GetFieldType(() => d.Description),
                    FieldName = d.GetFieldName(() => d.Description),
                    Operator = "like",
                    Value = description + "%"
                });
            }

            if (clearPrice.HasValue)
            {
                filters.Add(new DbfParameter
                {
                    FieldType = d.GetFieldType(()=>d.ClearPrice),
                    FieldName = d.GetFieldName(()=>d.ClearPrice),
                    Operator = "=",
                    Value = clearPrice.Value.ToString()
                });
            }

            if (updateDate.HasValue)
            {
                filters.Add(new DbfParameter
                {
                    FieldType = d.GetFieldType(() => d.UpdateDate),
                    FieldName = d.GetFieldName(() => d.UpdateDate),
                    Operator = "=",
                    Value = updateDate.Value.ToShortDateString()
                });
            }

            return base.ExecuteQuery(base.GetDefaultSelectQuery(), filters);
        }
    }
}
