using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleServices;
using System.Linq;
using System.Diagnostics;
using ExampleModel;

namespace DotNetDbfTests
{
    [TestClass]
    public class ZoneServiceTest
    {
        private string DBFPath = @"dbf";

        [TestMethod]
        public void GetAllZones()
        {
            var zoneService = new ZoneService(DBFPath);

            PrintAllZones(zoneService);
        }

        [TestMethod]
        public void AddZoneTest()
        {
            var zoneService = new ZoneService(DBFPath);
            Zone entity = new Zone
            {
                Code = "123",
                Description = "Zone 123"
            };

            zoneService.AddZone(entity);
            PrintAllZones(zoneService);
        }

        [TestMethod]
        public void UpdateZoneTest()
        {
            var zoneService = new ZoneService(DBFPath);
            var entity = zoneService.GetZoneByCode("123");
            Assert.IsNotNull(entity);

            entity.Description = "Zone 123 Updated";
            zoneService.UpdateZone(entity);

            entity = zoneService.GetZoneByCode("123");
            Assert.IsNotNull(entity);

            Debug.Print(entity.ToString());
        }

        [TestMethod]
        public void DeleteZoneTest()
        {
            var zoneService = new ZoneService(DBFPath);
            var entity = zoneService.GetZoneByCode("123");
            Assert.IsNotNull(entity);

            zoneService.DeleteZone(entity);

            entity = zoneService.GetZoneByCode("123");
            Assert.IsNull(entity);

            PrintAllZones(zoneService);
        }


        private void PrintAllZones(ZoneService zoneService)
        {
            var allZones = zoneService.GetAllZones();
            Assert.IsNotNull(allZones);

            allZones.ToList().ForEach(zone =>
            {
                Debug.Print(zone.ToString());
            });
        }
    }
}
