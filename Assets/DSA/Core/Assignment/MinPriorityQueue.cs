using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 3. Implement as a binary min-heap.

    /// <summary>
    /// A minimum-priority queue implemented as a binary min-heap.
    ///
    /// Items with lower integer priority values should be returned before
    /// items with higher priority values.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    /// </summary>
    public sealed class MinPriorityQueue<T>
    {
        // The list stores the physical representation of the binary heap.
        // Your implementation is responsible for preserving the min-heap invariant.
        private readonly List<Entry> _heap = new List<Entry>();

        /// <summary>
        /// Number of items currently stored in the queue.
        /// </summary>
        public int Count => _heap.Count;

        /// <summary>
        /// Adds an item with the supplied priority.
        /// Lower priority values are removed before higher priority values.
        /// </summary>
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

        /// <summary>
        /// Returns the minimum-priority item without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("The queue is empty!");
            }
            
            return _heap[0].Item;
        }

        /// <summary>
        /// Removes and returns the minimum-priority item.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty!");
            }
            
            T minItem = _heap[0].Item; //Saving min

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

        /// <summary>
        /// Stores one item together with the priority used by the heap.
        /// </summary>
        private readonly struct Entry
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
