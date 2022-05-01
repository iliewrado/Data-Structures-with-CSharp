using System;
using System.Collections.Generic;
using System.Linq;

namespace WordCruncher
{
    class Program
    {
        static void Main()
        {
            string[] syllables = Console.ReadLine()
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string targetWord = Console.ReadLine();

            Cruncher cruncher = new Cruncher(syllables, targetWord);

            foreach (var path in cruncher.GetPaths())
            {
                Console.WriteLine(path);
            }
        }
    }

    class Cruncher
    {
        private class Node
        {
            public Node(string  syllable, List<Node> nextSyllables)
            {
                this.Syllable = syllable;
                this.NextSyllables = nextSyllables;
            }

            public string Syllable { get; set; }
            public List<Node> NextSyllables { get; set; }
        }

        private List<Node> syllableGrups;

        public Cruncher(string[] syllables, string targetWord)
        {
            this.syllableGrups = this.GenerateSyllableGrups(syllables, targetWord);
        }

        private List<Node> GenerateSyllableGrups(string[] syllables, string targetWord)
        {
            if (string.IsNullOrEmpty(targetWord) || syllables.Length == 0)
                return null;

            List<Node> result = new List<Node>();

            for (int i = 0; i < syllables.Length; i++)
            {
                string syllable = syllables[i];

                if (targetWord.StartsWith(syllable))
                {
                    List<Node> nextSyllables = GenerateSyllableGrups(
                        syllables.Where((_, x) => x != i).ToArray(),
                        targetWord.Substring(syllable.Length));

                    result.Add(new Node(syllable, nextSyllables));
                }
            }

            return result;
        }

        internal IEnumerable<string> GetPaths()
        {
            List<List<string>> allPaths = new List<List<string>>();
            
            this.GeneratePaths(this.syllableGrups, new List<string>(), allPaths);
            
            return new HashSet<string>(allPaths.Select(x=>string.Join(" ", x)));
        }

        private void GeneratePaths(List<Node> syllableGrups, List<string> current, List<List<string>> allPaths)
        {
            if (syllableGrups == null)
            {
                allPaths.Add(new List<string>(current));
                return;
            }

            foreach (var node in syllableGrups)
            {
                current.Add(node.Syllable);

                this.GeneratePaths(node.NextSyllables, current, allPaths);

                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
