using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary
{
    public class MinMaxHashTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        private HashTableNode<TKey, TValue>?[] table;
        private MinAddresableBinaryHeap<TKey> minHeap;
        private MaxAddresableBinaryHeap<TKey> maxHeap;

        public TKey Minimum
        {
            get
            {
                return minHeap.Top.Key;
            }
        }
        public TKey Maximum
        {
            get
            {
                return maxHeap.Top.Key;
            }
        }

        public MinMaxHashTable()
        {
            minHeap = new MinAddresableBinaryHeap<TKey>();
            maxHeap = new MaxAddresableBinaryHeap<TKey>();
            table = new HashTableNode<TKey, TValue>?[0];
        }
        internal class HashTableNode<TKey, TValue> where TKey : IComparable<TKey>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public HeapHandle<TKey> MinHeapNode { get; set; }
            public HeapHandle<TKey> MaxHeapNode { get; set; }

            public HashTableNode(TKey key, TValue value, HeapHandle<TKey> min, HeapHandle<TKey> max)
            {
                Key = key;
                Value = value;
                MinHeapNode = min;
                MaxHeapNode = max;
            }
        }

        void Add(TKey key, TValue value)
        {
            HeapHandle<TKey> heapHandleMin = minHeap.Add(key);
            HeapHandle<TKey> heapHandleMax = maxHeap.Add(key);
            Array.Resize(ref table, table.Length + 1);

            table[table.Count() - 1] = new HashTableNode<TKey, TValue>(key, value, heapHandleMin, heapHandleMax);
        }

        bool Contains(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            for (int i = 0; i < table.Length; i++)
            {
                HashTableNode<TKey, TValue> aktualni = table[i];
                if (aktualni.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }
        TValue Get(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            if (!Contains(key))
            {
                throw new KeyNotFoundException();
            }
            for (int i = 0; i < table.Length; i++)
            {
                HashTableNode<TKey, TValue> aktualni = table[i];
                if (aktualni.Key.Equals(key))
                {
                    return aktualni.Value;
                }
            }
            throw new KeyNotFoundException();
        }
        TValue Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }
            if (!Contains(key))
            {
                throw new KeyNotFoundException();
            }

            for (int i = 0; i < table.Length; i++)
            {
                HashTableNode<TKey, TValue> aktualni = table[i];
                if (aktualni.Key.Equals(key))
                {
                    TValue aktualniValue = aktualni.Value;
                    table[i] = null;
                    for (int j = i; j < table.Length - 1; j++)
                    {
                        (table[j], table[j + j]) = (table[j + 1], table[j]);
                    }
                    Array.Resize(ref table, table.Length - 1);
                    minHeap.Remove(aktualni.MinHeapNode);
                    maxHeap.Remove(aktualni.MaxHeapNode);
                    return aktualniValue;
                }
            }
            throw new KeyNotFoundException();
        }
    }
}
