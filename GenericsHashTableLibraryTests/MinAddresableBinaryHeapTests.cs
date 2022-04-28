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
    public class MinAddresableBinaryHeapTests
    {
        private Type? type;
        private object? obj;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.MinAddresableBinaryHeap`1");
            Assert.IsTrue(type.IsGenericType);

            obj = New(type.MakeGenericType(typeof(int)));
            Assert.IsNotNull(obj);
        }


        [TestMethod()]
        public void GettingTopShouldThrowInvalidOperationExceptionWhenHeapIsEmptyTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Top"));
        }

        [TestMethod()]
        public void EmptyHeapShouldNotHaveASingleElementTest()
        {
            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        [TestMethod()]
        public void AddAndGetSingleElementFromHeapTest()
        {
            object? handle = Invoke(obj, "Add", 12345);

            Assert.IsNotNull(handle);
            Assert.AreEqual(12345, GetProperty(handle, "Key"));
            Assert.IsNotNull(GetProperty(handle, "Index"));

            Assert.AreSame(handle, GetProperty(obj, "Top"));
        }

        [TestMethod()]
        public void AddAndRemoveSingleElementFromHeapTest()
        {
            object? handle = Invoke(obj, "Add", 12345);

            Invoke(obj, "Remove", GetProperty(obj, "Top"));

            Assert.IsNull(GetProperty(handle, "Index"));
            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        [TestMethod()]
        public void AddAndRemoveTwoElementsFromHeapTest()
        {
            object? handle200 = Invoke(obj, "Add", 200);
            object? handle100 = Invoke(obj, "Add", 100);

            Invoke(obj, "Remove", GetProperty(obj, "Top"));

            Assert.IsNull(GetProperty(handle100, "Index"));
            Assert.AreEqual(1, GetProperty(obj, "Count"));
            Assert.AreEqual(200, GetProperty(GetProperty(obj, "Top"), "Key"));

            Invoke(obj, "Remove", GetProperty(obj, "Top"));

            Assert.IsNull(GetProperty(handle200, "Index"));
            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        [TestMethod()]
        public void AddAndRemoveThreeElementsOutOfOrderFromHeapTest()
        {
            object? handle200 = Invoke(obj, "Add", 200);
            object? handle100 = Invoke(obj, "Add", 100);
            object? handle300 = Invoke(obj, "Add", 300);

            Assert.AreEqual(100, GetProperty(GetProperty(obj, "Top"), "Key"));

            Invoke(obj, "Remove", handle200);
            Assert.IsNull(GetProperty(handle200, "Index"));

            Assert.AreEqual(100, GetProperty(GetProperty(obj, "Top"), "Key"));

            Invoke(obj, "Remove", handle300);
            Assert.IsNull(GetProperty(handle300, "Index"));

            Assert.AreEqual(100, GetProperty(GetProperty(obj, "Top"), "Key"));

            Invoke(obj, "Remove", handle100);
            Assert.IsNull(GetProperty(handle100, "Index"));

            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        [TestMethod()]
        public void RemovingNullShouldThrowArgumentExceptionTest()
        {
            Assert.ThrowsException<ArgumentException>(() => Invoke(obj, "Remove", new object?[] { null }));
        }

        [TestMethod()]
        public void RemovingAlreadyRemovedElementShouldThrowArgumentExceptionTest()
        {
            object? handle100 = Invoke(obj, "Add", 100);
            Invoke(obj, "Remove", handle100);

            Assert.ThrowsException<ArgumentException>(() => Invoke(obj, "Remove", handle100));
        }

        [TestMethod()]
        public void ComplexHeapTest()
        {
            object? handle01 = Invoke(obj, "Add", 1);
            object? handle10 = Invoke(obj, "Add", 10);
            object? handle05a = Invoke(obj, "Add", 5);
            object? handle78 = Invoke(obj, "Add", 78);
            object? handle05b = Invoke(obj, "Add", 5);
            object? handle00 = Invoke(obj, "Add", 0);
            object? handle20 = Invoke(obj, "Add", 20);
            object? handle50 = Invoke(obj, "Add", 50);
            object? handle79 = Invoke(obj, "Add", 79);

            Assert.AreEqual(9, GetProperty(obj, "Count"));
            Assert.AreEqual(handle00, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle00);
            Assert.IsNull(GetProperty(handle00, "Index"));

            Assert.AreEqual(handle01, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle01);
            Assert.IsNull(GetProperty(handle00, "Index"));

            Assert.AreEqual(5, GetProperty(GetProperty(obj, "Top"), "Key"));
            Invoke(obj, "Remove", GetProperty(obj, "Top"));
            Assert.AreEqual(5, GetProperty(GetProperty(obj, "Top"), "Key"));
            Invoke(obj, "Remove", GetProperty(obj, "Top"));

            Assert.IsNull(GetProperty(handle05a, "Index"));
            Assert.IsNull(GetProperty(handle05b, "Index"));

            Invoke(obj, "Remove", handle20);
            Invoke(obj, "Remove", handle50);

            handle00 = Invoke(obj, "Add", 0);
            Assert.IsNotNull(GetProperty(handle00, "Index"));
            Assert.AreEqual(handle00, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle00);
            Assert.IsNull(GetProperty(handle00, "Index"));

            Assert.AreEqual(handle10, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle10);

            Assert.AreEqual(handle78, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle78);

            Assert.AreEqual(handle79, GetProperty(obj, "Top"));
            Invoke(obj, "Remove", handle79);

            Assert.AreEqual(0, GetProperty(obj, "Count"));
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Top"));
        }

        [TestMethod()]
        public void ComplexUglyHeapTest()
        {
            Random random = new Random(23550);
            List<int> expected = new();
            for (int i = 0; i < 4096; i++)
            {
                int value = random.Next();
                expected.Add(value);
                Invoke(obj, "Add", value);
            }

            expected.Sort();

            foreach (var expectedKey in expected)
            {
                object? handle = GetProperty(obj, "Top");
                Assert.AreEqual(expectedKey, GetProperty(handle, "Key"));

                Invoke(obj, "Remove", handle);
            }

            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        [TestMethod()]
        public void ComplexUglyHeapWithNonTopRemovalsTest()
        {
            Random random = new Random(45663);
            List<(int, object?)> expected = new();
            for (int i = 0; i < 65536; i++)
            {
                int value = random.Next();
                object? handle = Invoke(obj, "Add", value);

                expected.Add((value, handle));
            }

            expected.Sort(new KeyHandleComparer());

            while(expected.Count > 0)
            {
                if (random.Next(6) == 0)
                {
                    int randomIndex = random.Next(expected.Count);

                    var (_, randomHandle) = expected[randomIndex];

                    Invoke(obj, "Remove", randomHandle);
                    Assert.IsNull(GetProperty(randomHandle, "Index"));

                    expected.RemoveAt(randomIndex);
                    continue;
                }

                var (expectedKey, _) = expected[0];

                object? handle = GetProperty(obj, "Top");
                Assert.AreEqual(expectedKey, GetProperty(handle, "Key"));

                Invoke(obj, "Remove", handle);

                expected.RemoveAt(0);
            }

            Assert.AreEqual(0, GetProperty(obj, "Count"));
        }

        private class KeyHandleComparer : IComparer<(int, object?)>
        {
            public int Compare((int, object?) x, (int, object?) y)
            {
                return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}