using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericsHashTableLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary.Tests
{
    [TestClass()]
    public class HeapHandleTests
    {
        private Type? type;
        private object? obj;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.HeapHandle`1");
            Assert.IsTrue(type.IsGenericType);
        }

        [TestMethod()]
        public void ConstructorTest()
        {
            Type? specificType = type?.MakeGenericType(typeof(int));
            Assert.IsNotNull(specificType);

            obj = New(specificType, 123, 200);
            Assert.IsNotNull(obj);
        }

        [TestMethod()]
        public void PropertiesTest()
        {
            Type? specificType = type?.MakeGenericType(typeof(int));
            Assert.IsNotNull(specificType);

            obj = New(specificType, 65, 10);
            Assert.IsNotNull(obj);

            Assert.AreEqual(65, GetProperty(obj, "Key"));
            Assert.AreEqual(10, GetProperty(obj, "Index"));
        }
    }
}