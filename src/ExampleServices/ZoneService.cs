using DotNetDbf;
using ExampleModel;
using System.Collections.Generic;
using System.Linq;

namespace ExampleServices
{
    public sealed class ZoneService
        : DbfBase<Zone>
    {
        public ZoneService(string path)
            : base(path)
        {

        }

        public IList<Zone> GetAllZones()
        {
            return base.ExecuteQuery(base.GetDefaultSelectQuery());
        }

        public Zone GetZoneByCode(string code)
        {
            Zone d = new Zone();
            DbfParameter filter = new DbfParameter
            {
                FieldType = d.GetFieldType(() => d.Code),
                FieldName = d.GetFieldName(() => d.Code),
                Operator = "=",
                Value = code
            };

            var result = base.ExecuteQuery(filter);

            if (result != null)
            {
                return result.First();
            }

            return null;
        }

        public int AddZone(Zone entity)
        {
            return base.Insert(entity);
        }

        public int UpdateZone(Zone entity)
        {
            return base.Update(entity);
        }

        public int DeleteZone(Zone entity)
        {
            return base.Delete(entity);
        }
    }
}
