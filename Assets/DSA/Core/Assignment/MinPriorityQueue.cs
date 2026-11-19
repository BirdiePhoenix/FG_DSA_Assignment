using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    
    public sealed class MinPriorityQueue<T>
    {
        private readonly List<Entry> _heap = new List<Entry>();
        public int Count => _heap.Count;

        public void Enqueue(T item, int priority)
        {
            _heap.Add(new Entry(item, priority));
            if (Count > 1) //If only 1 Entry, skip
            {
                SiftUp(Count-1);
            }
        }

        private void SiftUp(int index)
        {
            while (true)
            {
                int parentIndex = ParentIndex(index);
                
                if(_heap[parentIndex].Priority <= _heap[index].Priority || index == 0) return;
                
                (_heap[parentIndex], _heap[index]) = (_heap[index], _heap[parentIndex]);

                index = parentIndex;
            }
        }

        private int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        public Entry Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty!");
            }
            
            return _heap[0];
        }

        public Entry Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty!");
            }
            
            Entry minItem = _heap[0]; //Saving min

            if (Count > 1)
            {
                _heap[0] = _heap[Count - 1]; //Replacing min with last
            }
            _heap.RemoveAt(Count - 1);

            if (Count > 1)
            {
                SiftDown(0);
            }
            return minItem;
        }

        private void SiftDown(int index)
        {
            while (true)
            {
                int leftIndex = LeftIndex(index);

                if (leftIndex >= Count) //If no left child
                {
                    return;
                }

                int rightIndex = RightIndex(index);

                if (_heap[index].Priority > _heap[leftIndex].Priority && rightIndex >= Count)
                {   //If parent priority is bigger than left and no right exists
                    (_heap[index], _heap[leftIndex]) = (_heap[leftIndex], _heap[index]);
                    return;
                }
                if (rightIndex >= Count)
                {
                    return;
                }

                if (_heap[index].Priority <= _heap[leftIndex].Priority &&
                    _heap[index].Priority <= _heap[rightIndex].Priority)
                {
                    return;
                }

                if (_heap[leftIndex].Priority <= _heap[rightIndex].Priority)
                {
                    (_heap[index], _heap[leftIndex]) = (_heap[leftIndex], _heap[index]);
                    index = leftIndex;
                }
                else
                {
                    (_heap[index], _heap[rightIndex]) = (_heap[rightIndex], _heap[index]);
                    index = rightIndex;
                }
            }
        }

        private int LeftIndex(int index)
        {
            return 2 * index + 1;
        }

        private int RightIndex(int index)
        {
            return 2 * index + 2;
        }

        public readonly struct Entry
        {
            public Entry(T item, int priority)
            {
                Item = item;
                Priority = priority;
            }

            public T Item { get; }
            public int Priority { get; }
        }
    }
}
