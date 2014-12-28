using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleServices;
using System.Linq;
using System.Diagnostics;

namespace DotNetDbfTests
{
    [TestClass]
    public class ProductServiceTest
    {
        [TestMethod]
        public void GetAllProductsTest()
        {
            var productService = new ProductsService("dbf");

            var result = productService.GetAllProducts();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            result.ToList().ForEach(product =>
            {
                Debug.Print(product.ToString());
            });
        }

        [TestMethod]
        public void GetProductsCountTest()
        {
            var productService = new ProductsService("dbf");
            var result = productService.GetProductsCount();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result);

            Debug.Print("Number of rows: " + result);
        }

        [TestMethod]
        public void FindProductsByCodeTest()
        {
            var productService = new ProductsService("dbf");

            var result = productService.FindProducts("3A0026", string.Empty, null, null);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            result.ToList().ForEach(product =>
            {
                Debug.Print(product.ToString());
            });
        }

        [TestMethod]
        public void FindProductsByPrice()
        {
            var productService = new ProductsService("dbf");

            var result = productService.FindProducts(string.Empty, string.Empty, 19, null);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            result.ToList().ForEach(product =>
            {
                Debug.Print(product.ToString());
            });
        }

        [TestMethod]
        public void FindProdcutsByUpdateDate()
        {
            var productService = new ProductsService("dbf");

            var result = productService.FindProducts(string.Empty, string.Empty, null, new DateTime(2014, 7, 24));

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            result.ToList().ForEach(product =>
            {
                Debug.Print(product.Code + " " + product.UpdateDate.ToShortDateString());
            });
        }
    }
}
