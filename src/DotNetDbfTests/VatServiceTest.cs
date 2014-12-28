using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleServices;
using System.Diagnostics;
using ExampleModel;


namespace DotNetDbfTests
{
    [TestClass]
    public class VatServiceTest
    {
        [TestMethod]
        public void GetAllVatsTest()
        {
            var vatService = new VatService("dbf");
            var result = vatService.GetAllVats();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            result.ToList().ForEach(vat =>
            {
                Debug.Print(vat.ToString());
            });
        }

        [TestMethod]
        public void GetVatByCodeTest()
        {
            var vatService = new VatService("dbf");
            var result = vatService.GetVatByCode("G");

            Assert.IsNotNull(result);

            Debug.Print(result.ToString());
        }

        [TestMethod]
        public void InsertVatTest()
        {
            var vat = new Vat
            {
                Code = "X",
                Description = "Sample Vat",
                Percentage = 23.0
            };

            var vatService = new VatService("dbf");

            var result = vatService.AddVat(vat);
            Assert.AreNotEqual(0, result);

            var addedVat = vatService.GetVatByCode("X");
            Assert.IsNotNull(addedVat);
            Debug.Print(addedVat.ToString());
        }

        [TestMethod]
        public void UpdateVatTest()
        {
            var vatService = new VatService("dbf");

            var vat = vatService.GetVatByCode("S");
            Assert.IsNotNull(vat);

            vat.Description = "SampleVat Up";
            var result = vatService.UpdateVat(vat);
            Assert.AreNotEqual(0, result);

            var updatedVat = vatService.GetVatByCode("S");
            Assert.IsNotNull(updatedVat);
            Debug.Print(updatedVat.ToString());
        }

        [TestMethod]
        public void DeleteVatTest()
        {
            var vatService = new VatService("dbf");

            var vat = vatService.GetVatByCode("S");
            Assert.IsNotNull(vat);

            var result = vatService.DeleteVat(vat);
            Assert.AreNotEqual(0, result);

            var updatedVat = vatService.GetVatByCode("S");
            Assert.IsNull(updatedVat);
        }
    }
}
