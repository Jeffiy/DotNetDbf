## DotNetDbf
Very simple library to manage DBF files

## Core features
- Define your model with classes and attributes.

		[TableName("Ivas")]
		public sealed class Vat : EntityBase
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

- Map any query results to an IList<T> collection without dealing with DataReaders or DataTables.

            var vatService = new VatService("dbf");
            var result = vatService.GetAllVats();

            result.ToList().ForEach(vat =>
            {
                Debug.Print(vat.ToString());
            });
	
- Simple API to manage query filters.

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
- Built-in methods for INSERT, UPDATE and DELETE.

        public int AddVat(Vat entity)
        {
            return base.Insert(entity);
        }

## How to use it     

### Create your model

You must create your entity classes derived from **EntityBase**. Then you must add the following attributes:
- TableName with the real table name.
- ColumnName with the real column name.
- PrimaryKey with a boolean argument to indicate if the PK is an autoincrement field.

### Create your data access classes

Then you must create your data access classes derived from **DbfBase**.

		public sealed class VatService
        	: DbfBase<Vat>
            
And then you must create all your data access methods.

	public sealed class VatService
        : DbfBase<Vat>
    {
        public VatService(string path)
            : base(path)
        {

        }

        public IList<Vat> GetAllVats()
        {
            return base.ExecuteQuery(this.GetDefaultSelectQuery());
        }

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

See ExampleModel, ExampleDataServices and Unit Test projects for more details.


