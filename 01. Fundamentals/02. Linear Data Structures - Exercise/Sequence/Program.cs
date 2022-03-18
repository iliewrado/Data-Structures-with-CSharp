using System;
using System.Collections.Generic;
using System.Linq;

namespace Sequence
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] input = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            int n = input[0];
            int m = input[1];

            Queue<(int value, Node<int> node)> queue = new Queue<(int value, Node<int> node)>();
            Node<int> node = new Node<int>(n);
            queue.Enqueue((n, node));

            while (queue.Count != 0)
            {
                (int value, Node<int> node) e = queue.Dequeue();
                node = e.node;
                if (e.value < m)
                {
                    Node<int> newNode = new Node<int>(e.value + 1);
                    newNode.Previous = node;
                    queue.Enqueue((e.value + 1, newNode));
                    newNode = new Node<int>(e.value + 2);
                    newNode.Previous = node;
                    queue.Enqueue((e.value + 2, newNode));
                    newNode = new Node<int>(e.value * 2);
                    newNode.Previous = node;
                    queue.Enqueue((e.value * 2, newNode));
                    
                }

                if (e.value == m)
                {
                    List<int> result = new List<int>();
                    while (e.node != null)
                    {
                        result.Add(e.value);
                        
                        if (e.node.Previous == null)
                        {
                            break;
                        }

                        e = (e.node.Previous.Value, e.node.Previous);
                    }
                    result.Reverse();
                    Console.WriteLine(string.Join(" ", result));
                    break;
                }
            }
        }
        public class Node<T>
        {
            public Node(T element)
            {
                this.Value = element;
            }

            public T Value { get; set; }
            public Node<T> Previous { get; set; }
        }
    }
}
