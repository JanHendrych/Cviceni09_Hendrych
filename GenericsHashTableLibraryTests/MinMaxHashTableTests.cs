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
    public class MinMaxHashTableTests
    {
        class CollisionKey : IComparable<CollisionKey>  
        {
            public int Key { get; set; }

            public CollisionKey(int key)
            {
                Key = key;
            }

            public int CompareTo(CollisionKey? other)
            {
                return Key.CompareTo(other.Key);
            }

            public override bool Equals(object? obj)
            {
                return obj is CollisionKey key &&
                       Key == key.Key;
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }

        private Type? type;
        private object? obj;

        [TestInitialize]
        public void InitializeType()
        {
            type = GetTestedType("GenericsHashTableLibrary.MinMaxHashTable`2");
            Assert.IsTrue(type.IsGenericType);

            obj = New(type.MakeGenericType(typeof(int), typeof(string)));
            Assert.IsNotNull(obj);
        }

        [TestMethod()]
        public void ClassDoesntHaveAnyForbiddenReferencesInsideTest()
        {
            var fields = type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            foreach (var forbiddenClassName in new string[] { "Dictionary", "HashSet" }) {
                foreach (var field in fields)
                {
                    if (field.FieldType.Name.Contains(forbiddenClassName))
                        Assert.Fail($"Forbidden field used - {field.FieldType.Name}");
                }

                if (type.BaseType is not null && type.BaseType.Name.Contains(forbiddenClassName))
                    Assert.Fail($"Forbidden inheritance - {type.BaseType.Name}");
            }
        }

        [TestMethod()]
        public void AddThenContainsThenGetCorrectTest()
        {
            Invoke(obj, "Add", 123, "alpha");

            Assert.IsTrue((bool)Invoke(obj, "Contains", 123));

            Assert.AreEqual("alpha", Invoke(obj, "Get", 123));
        }

        [TestMethod()]
        public void AddThenRemoveThenContainsCorrectTest()
        {
            Invoke(obj, "Add", 123, "alpha");

            Invoke(obj, "Remove", 123);

            Assert.IsFalse((bool)Invoke(obj, "Contains", 123));
        }

        [TestMethod()]
        public void EmptyHashTableShouldNotContainAnythingTest()
        {
            Assert.IsFalse((bool)Invoke(obj, "Contains", 0));
            Assert.IsFalse((bool)Invoke(obj, "Contains", 100));
            Assert.IsFalse((bool)Invoke(obj, "Contains", 123));
        }

        [TestMethod()]
        public void GetOnNonexistentElementShouldThrowKeyNotFoundExceptionTest()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => Invoke(obj, "Get", 123));
        }

        [TestMethod()]
        public void RemoveOnNonexistentElementShouldThrowKeyNotFoundExceptionTest()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => Invoke(obj, "Remove", 123));
        }

        [TestMethod()]
        public void AddWithNullKeyThrowArgumentNullExceptionTest()
        {
            obj = New(type.MakeGenericType(typeof(string), typeof(string)));
            Assert.IsNotNull(obj);

            Assert.ThrowsException<ArgumentNullException>(() => Invoke(obj, "Add", new object[] { null, "beta" }));
        }

        [TestMethod()]
        public void RemoveWithNullKeyThrowArgumentNullExceptionTest()
        {
            obj = New(type.MakeGenericType(typeof(string), typeof(string)));
            Assert.IsNotNull(obj);

            Assert.ThrowsException<ArgumentNullException>(() => Invoke(obj, "Remove", new object[] { null }));
        }

        [TestMethod()]
        public void ContainsWithNullKeyThrowArgumentNullExceptionTest()
        {
            obj = New(type.MakeGenericType(typeof(string), typeof(string)));
            Assert.IsNotNull(obj);

            Assert.ThrowsException<ArgumentNullException>(() => Invoke(obj, "Contains", new object[] { null }));
        }

        [TestMethod()]
        public void AddingAlreadyExistingKeyThrowsArgumentExceptionTest()
        {
            Invoke(obj, "Add", 700, "alpha");

            Assert.ThrowsException<ArgumentException>(() => Invoke(obj, "Add", 700, "beta"));
        }

        [TestMethod()]
        public void MinimumAndMaximumThrowsInvalidOperationExceptionWhenHashTableIsEmptyTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Minimum"));
            Assert.ThrowsException<InvalidOperationException>(() => GetProperty(obj, "Maximum"));
        }

        [TestMethod()]
        public void CollisionHashCodesWithDifferentKeysTest()
        {
            obj = New(type.MakeGenericType(typeof(CollisionKey), typeof(string)));
            Assert.IsNotNull(obj);

            Invoke(obj, "Add", new CollisionKey(100), "hello");
            Invoke(obj, "Add", new CollisionKey(0), "world");
            Invoke(obj, "Add", new CollisionKey(50), "!");

            Assert.IsTrue((bool)Invoke(obj, "Contains", new CollisionKey(0)));
            Assert.IsTrue((bool)Invoke(obj, "Contains", new CollisionKey(50)));
            Assert.IsTrue((bool)Invoke(obj, "Contains", new CollisionKey(100)));
            
            Assert.IsFalse((bool)Invoke(obj, "Contains", new CollisionKey(-1)));
            Assert.IsFalse((bool)Invoke(obj, "Contains", new CollisionKey(1)));
            Assert.IsFalse((bool)Invoke(obj, "Contains", new CollisionKey(1000)));

            Assert.AreEqual("hello", Invoke(obj, "Get", new CollisionKey(100)));
            Assert.AreEqual("world", Invoke(obj, "Get", new CollisionKey(0)));
            Assert.AreEqual("!", Invoke(obj, "Get", new CollisionKey(50)));
        }


        [TestMethod()]
        public void ComplexHashTableTest()
        {
            HashSet<int> keys = new();
            Random random = new Random(12456);

            for (int i = 0; i < 1024; i++)
            {
                int key = random.Next();
                while (keys.Contains(key))
                    key = random.Next();

                keys.Add(key);
                Invoke(obj, "Add", key, key.ToString());
            }

            List<int> keysList = keys.ToList();
            while (keysList.Count > 0)
            {
                int index = random.Next(keysList.Count);
                int key = keysList[index];
                keysList.RemoveAt(index);

                Assert.IsTrue((bool) Invoke(obj, "Contains", key));
                Invoke(obj, "Remove", key);
            }
        }

        [TestMethod()]
        public void ComplexHashTableIncludingMinAndMaxTest()
        {
            HashSet<int> keys = new();
            Random random = new Random(1113);

            for (int i = 0; i < 8192; i++)
            {
                int key = random.Next();
                while (keys.Contains(key))
                    key = random.Next();

                keys.Add(key);
                Invoke(obj, "Add", key, key.ToString());
            }

            List<int> keysList = keys.ToList();
            while (keysList.Count > 0)
            {
                int expectedMin = keysList.Min();
                int expectedMax = keysList.Max();

                Assert.AreEqual(expectedMin, GetProperty(obj, "Minimum"));
                Assert.AreEqual(expectedMax, GetProperty(obj, "Maximum"));

                int index = random.Next(keysList.Count);
                int key = keysList[index];
                keysList.RemoveAt(index);

                Assert.IsTrue((bool)Invoke(obj, "Contains", key));
                Invoke(obj, "Remove", key);
            }
        }
    }
}