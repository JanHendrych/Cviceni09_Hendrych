using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary
{
    public class HeapHandle<T> where T : IComparable<T>
    {
        public T Key { get; init; }
        public int? Index { get; set; }

        public HeapHandle(T key, int? index)
        {
            Key = key;
            Index = index;
        }
    }
}
