using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary
{
    public class MaxAddresableBinaryHeap<T> : AddresableBinaryHeap<T> where T : IComparable<T>
    {
        public override int CompareKeys(T key1, T key2)
        {
            return key2.CompareTo(key1);
        }
    }
}
