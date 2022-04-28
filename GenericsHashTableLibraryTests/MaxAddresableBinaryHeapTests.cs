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
    public class MaxAddresableBinaryHeapTests
    {
        private Type? type;
        private object? obj;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.MaxAddresableBinaryHeap`1");
            Assert.IsTrue(type.IsGenericType);

            obj = New(type.MakeGenericType(typeof(int)));
            Assert.IsNotNull(obj);
        }


        [TestMethod()]
        public void ComplexHeapTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Top"));

            object? handle01 = Invoke(obj, "Add", 1);
            object? handle10 = Invoke(obj, "Add", 10);
            object? handle05a = Invoke(obj, "Add", 5);
            object? handle05b = Invoke(obj, "Add", 5);
            object? handle00 = Invoke(obj, "Add", 0);
            object? handle20 = Invoke(obj, "Add", 20);
            object? handle50 = Invoke(obj, "Add", 50);

            Assert.AreEqual(7, GetProperty(obj, "Count"));
            Assert.AreEqual(handle50, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle50);
            Assert.IsNull(GetProperty(handle50, "Index"));

            Assert.AreEqual(handle20, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle20);
            Assert.IsNull(GetProperty(handle20, "Index"));
            
            Invoke(obj, "Remove", handle00);
            Invoke(obj, "Remove", handle01);
            Invoke(obj, "Remove", handle10);
            Invoke(obj, "Remove", handle05a);

            Assert.AreEqual(1, GetProperty(obj, "Count"));

            Assert.AreEqual(handle05b, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle05b);
            Assert.IsNull(GetProperty(handle05b, "Index"));

            Assert.AreEqual(0, GetProperty(obj, "Count"));
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Top"));
        }

        [TestMethod()]
        public void ComplexUglyHeapTest()
        {
            Random random = new Random(12203);
            List<int> expected = new();
            for (int i = 0; i < 2048; i++)
            {
                int value = random.Next(-10000,10000);
                expected.Add(value);
                Invoke(obj, "Add", value);
            }

            expected.Sort();
            expected.Reverse();

            foreach (var expectedKey in expected)
            {
                object? handle = GetProperty(obj, "Top");
                Assert.AreEqual(expectedKey, GetProperty(handle, "Key"));

                Invoke(obj, "Remove", handle);
            }

            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }
    }
}