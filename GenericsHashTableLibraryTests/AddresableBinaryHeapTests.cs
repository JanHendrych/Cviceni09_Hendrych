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
    public class AddresableBinaryHeapTests
    {
        private Type? type;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.AddresableBinaryHeap`1");
            Assert.IsTrue(type.IsGenericType);
        }

        [TestMethod()]
        public void ClassIsAbstractTest()
        {
            Assert.IsTrue(type.IsAbstract);
        }

        [TestMethod()]
        public void ClassRealizesRequiredInterfaceTest()
        {
            Assert.IsNotNull(type.GetInterface("IAddresableHeap`1"));
        }
    }
}