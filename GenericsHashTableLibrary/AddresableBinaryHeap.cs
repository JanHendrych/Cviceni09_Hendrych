using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsHashTableLibrary
{
    public abstract class AddresableBinaryHeap<T> : IAddresableHeap<T> where T : IComparable<T>
    {
        private HeapHandle<T>?[] heapArray;
        public int Count => pocet;
        public HeapHandle<T> Top
        {
            get
            {
                if (Count > 0)
                {
                    return heapArray[0];
                }
                throw new InvalidOperationException();
            }
        }

        protected AddresableBinaryHeap()
        {
            heapArray = new HeapHandle<T>[0];
        }
        private int pocet = 0;
        public HeapHandle<T> Add(T key)
        {
            HeapHandle<T> handle = new HeapHandle<T>(key, null);
            if (Count >= heapArray.Length)
            {
                Array.Resize(ref heapArray, heapArray.Length + 1);
            }
            if (Count == 0)
            {
                heapArray[0] = handle;
                handle.Index = 0;
            }
            else
            {
                heapArray[pocet] = handle;
                handle.Index = pocet;

                BubbleUp(pocet);
            }

            pocet++;
            return handle;
        }

        private void BubbleUp(int index)
        {
            int otec = (index - 1) / 2;
            if (index <= 0)
            {
                return;
            }

            if (CompareKeys(heapArray[index].Key, heapArray[otec].Key) == 0)
            {
                HeapHandle<T> tmp = heapArray[index];
                heapArray[index] = heapArray[otec];
                heapArray[otec] = tmp;

                int? indexTmp = heapArray[index].Index;
                heapArray[index].Index = heapArray[otec].Index;
                heapArray[otec].Index = indexTmp;
            }
            BubbleUp(otec);
        }

        public void Remove(HeapHandle<T> handle)
        {

            ForceBubbleUp((int)handle.Index);
            //TODO Check
            heapArray[pocet] = null;

            Heapify(0);

            handle.Index = null;
            //TODO Check
            handle = null;
            pocet--;
        }

        private void Heapify(int index)
        {
            int levaVetev = (index * 2) + 1;
            int pravaVetev = (index * 2) + 2;

            int nejmensi = index;

            if (levaVetev < pocet && CompareKeys(heapArray[levaVetev].Key, heapArray[nejmensi].Key) < 0)
            {
                nejmensi = levaVetev;
            }
            if (pravaVetev < pocet && CompareKeys(heapArray[pravaVetev].Key, heapArray[nejmensi].Key) < 0)
            {
                nejmensi = pravaVetev;
            }
            if (nejmensi != index)
            {
                Console.WriteLine(nejmensi + "|" + index);
                HeapHandle<T> tmp = heapArray[index];

                heapArray[index] = heapArray[nejmensi];
                heapArray[nejmensi] = tmp;

                int? indexTmp = heapArray[index].Index;
                heapArray[index].Index = heapArray[nejmensi].Index;
                heapArray[nejmensi].Index = indexTmp;

                Console.WriteLine(nejmensi);
                Heapify(nejmensi);
            }
        }

        private void ForceBubbleUp(int index)
        {
            while(index > 0)
            {
                int otec = (index - 1) / 2;
                HeapHandle<T> tmp = heapArray[index];
                heapArray[index] = heapArray[otec];
                heapArray[otec] = tmp;

                int? indexTmp = heapArray[index].Index;
                heapArray[index].Index = heapArray[otec].Index;
                heapArray[otec].Index = indexTmp;
            }
        }

        public abstract int CompareKeys(T key1, T key2);


    }
}
