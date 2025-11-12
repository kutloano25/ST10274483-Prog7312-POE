using System;
using System.Collections.Generic;

namespace MunicipalApp
{
    // Minimal priority queue (min-heap) by provided comparer
    public class PriorityQueue<T>
    {
        private readonly List<T> _heap = new List<T>();
        private readonly IComparer<T> _comparer;

        public PriorityQueue(IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public int Count => _heap.Count;

        public void Enqueue(T item)
        {
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
        }

        public T Dequeue()
        {
            if (_heap.Count == 0) throw new InvalidOperationException("Queue empty");
            var root = _heap[0];
            var last = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            if (_heap.Count > 0)
            {
                _heap[0] = last;
                HeapifyDown(0);
            }
            return root;
        }

        public T Peek()
        {
            if (_heap.Count == 0) throw new InvalidOperationException("Queue empty");
            return _heap[0];
        }

        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (_comparer.Compare(_heap[i], _heap[parent]) >= 0) break;
                Swap(i, parent);
                i = parent;
            }
        }

        private void HeapifyDown(int i)
        {
            int last = _heap.Count - 1;
            while (true)
            {
                int left = 2 * i + 1;
                int right = 2 * i + 2;
                int smallest = i;
                if (left <= last && _comparer.Compare(_heap[left], _heap[smallest]) < 0) smallest = left;
                if (right <= last && _comparer.Compare(_heap[right], _heap[smallest]) < 0) smallest = right;
                if (smallest == i) break;
                Swap(i, smallest);
                i = smallest;
            }
        }

        private void Swap(int a, int b)
        {
            var tmp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = tmp;
        }
    }

    // Comparer for EventItem by StartDate
    public class EventDateComparer : IComparer<EventItem>
    {
        public int Compare(EventItem x, EventItem y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return DateTime.Compare(x.StartDate, y.StartDate);
        }
    }
}