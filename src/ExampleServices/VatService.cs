using DotNetDbf;
using ExampleModel;
using System.Collections.Generic;
using System.Linq;

namespace ExampleServices
{
    /// <summary>
    /// Vat entity data services
    /// </summary>
    public sealed class VatService
        : DbfBase<Vat>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to DBF database</param>
        public VatService(string path)
            : base(path)
        {

        }

        /// <summary>
        /// Gets complete vat list
        /// </summary>
        /// <returns>List of Vat</returns>
        public IList<Vat> GetAllVats()
        {
            return base.ExecuteQuery(this.GetDefaultSelectQuery());
        }

        /// <summary>
        /// Get vat by code
        /// </summary>
        /// <param name="code">Code Filter</param>
        /// <returns>Vat entity</returns>
        public Vat GetVatByCode(string code)
        {
            List<DbfParameter> filters = new List<DbfParameter>();
            Vat d = new Vat();

            filters.Add(new DbfParameter
            {
                FieldType = d.GetFieldType(() => d.Code),
                FieldName = d.GetFieldName(() => d.Code),
                Operator = "=",
                Value = code
            });

            var result = base.ExecuteQuery(base.GetDefaultSelectQuery(), filters);

            if(result != null)
            {
                return result.First();
            }

            return null;
        }

        public int AddVat(Vat entity)
        {
            return base.Insert(entity);
        }

        public int UpdateVat(Vat entity)
        {
            return base.Update(entity);
        }

        public int DeleteVat(Vat entity)
        {
            return base.Delete(entity);
        }
    }
}
