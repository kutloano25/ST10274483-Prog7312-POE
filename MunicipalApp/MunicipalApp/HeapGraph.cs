using System;
using System.Collections.Generic;

namespace MunicipalApp
{
    // Simple MinHeap (by priority)
    public class MinHeap<T> where T : IComparable<T>
    {
        private readonly List<T> _elements = new List<T>();
        public int Count => _elements.Count;

        public void Add(T item)
        {
            _elements.Add(item);
            HeapifyUp(_elements.Count - 1);
        }

        public T Pop()
        {
            if (Count == 0) throw new InvalidOperationException("Empty heap");
            var result = _elements[0];
            _elements[0] = _elements[^1];
            _elements.RemoveAt(Count - 1);
            HeapifyDown(0);
            return result;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_elements[index].CompareTo(_elements[parent]) >= 0) break;
                (_elements[index], _elements[parent]) = (_elements[parent], _elements[index]);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            int last = Count - 1;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;
                if (left <= last && _elements[left].CompareTo(_elements[smallest]) < 0) smallest = left;
                if (right <= last && _elements[right].CompareTo(_elements[smallest]) < 0) smallest = right;
                if (smallest == index) break;
                (_elements[index], _elements[smallest]) = (_elements[smallest], _elements[index]);
                index = smallest;
            }
        }
    }

    // Simple Graph for dependencies
    public class Graph
    {
        public readonly Dictionary<int, List<int>> adjacency = new Dictionary<int, List<int>>();

        public void AddEdge(int from, int to)
        {
            if (!adjacency.ContainsKey(from)) adjacency[from] = new List<int>();
            adjacency[from].Add(to);
        }

        public List<int> GetDependencies(int id)
        {
            return adjacency.ContainsKey(id) ? adjacency[id] : new List<int>();
        }
    }
}
