using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Data
{
    public class PriorityQueue<T>
        where T : IComparable<T>
    {
        private List<T> heap;

        public PriorityQueue()
        {
            heap = new List<T>();
        }

        public int Size => heap.Count;

        public List<T> ToList
        { 
            get 
            {
                return new List<T>(heap);
            } 
        }

        public T Dequeue()
        {
            this.ValidateEmpty();

            T first = heap[0];
            heap[0] = heap[Size - 1];
            heap.RemoveAt(Size - 1);
            HeapifyDown(0);

            return first;
        }

        public void Add(T element)
        {
            heap.Add(element);
            HeapifyUp(heap.Count - 1);
        }

        public T Peek()
        {
            this.ValidateEmpty();

            return heap[0];
        }

        private void HeapifyDown(int index)
        {
            int leftIndex = index * 2 + 1;
            int rightIndex = index * 2 + 2;
            int maxIndex = leftIndex;

            if (leftIndex >= Size) return;

            if ((rightIndex < Size) && heap[leftIndex].CompareTo(heap[rightIndex]) < 0)
                maxIndex = rightIndex;

            if (heap[index].CompareTo(heap[maxIndex]) < 0)
            {
                T temp = heap[index];
                heap[index] = heap[maxIndex];
                heap[maxIndex] = temp;
                HeapifyDown(maxIndex);
            }
        }

        private void HeapifyUp(int index)
        {
            if (index == 0) return;

            int parentIndex = (index - 1) / 2;

            if (heap[index].CompareTo(heap[parentIndex]) > 0)
            {
                T temp = heap[index];
                heap[index] = heap[parentIndex];
                heap[parentIndex] = temp;
                HeapifyUp(parentIndex);
            }
        }

        private void ValidateEmpty()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Operation on empty Data");
        }
    }
}
