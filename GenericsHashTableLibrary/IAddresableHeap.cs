using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary
{
    public interface IAddresableHeap<T> where T : IComparable<T>
    {
        public int Count { get; }
        HeapHandle<T> Top { get; }

        HeapHandle<T> Add(T key);
        void Remove(HeapHandle<T> handle);
    }
}
