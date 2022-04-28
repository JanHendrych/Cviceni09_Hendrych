using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenericsHashTableLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GenericsHashTableLibrary.Tests
{
    [TestClass()]
    public class IAddresableHeapTests
    {
        private Type? type;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.IAddresableHeap`1");
            Assert.IsTrue(type.IsGenericType);
        }

        [TestMethod()]
        public void IsInterfaceTest()
        {
            Assert.IsTrue(type?.IsInterface);
        }

        [TestMethod()]
        public void HasRequiredMembersTest()
        {
            Assert.IsNotNull(type?.GetProperty("Count"));
            Assert.IsNotNull(type?.GetProperty("Top"));
            Assert.IsNotNull(type?.GetMethod("Add"));
            Assert.IsNotNull(type?.GetMethod("Remove"));
        }

        [TestMethod()]
        public void CountPropertyTest()
        {
            PropertyInfo? count = type?.GetProperty("Count");
            Assert.IsNotNull(count);

            Assert.AreEqual(typeof(int), count.PropertyType);
        }

        [TestMethod()]
        public void TopPropertyTest()
        {
            PropertyInfo? top = type?.GetProperty("Top");
            Assert.IsNotNull(top);

            Assert.AreEqual("HeapHandle`1", top.PropertyType.Name);
        }

        [TestMethod()]
        public void AddMethodTest()
        {
            MethodInfo? add = type?.GetMethod("Add");
            Assert.IsNotNull(add);

            Assert.IsTrue(add.GetParameters()[0].ParameterType.IsGenericParameter);
            Assert.AreEqual("HeapHandle`1", add.ReturnParameter.ParameterType.Name);
        }

        [TestMethod()]
        public void RemoveMethodTest()
        {
            MethodInfo? remove = type?.GetMethod("Remove");
            Assert.IsNotNull(remove);

            Assert.AreEqual("HeapHandle`1", remove.GetParameters()[0].ParameterType.Name);
        }
    }
}