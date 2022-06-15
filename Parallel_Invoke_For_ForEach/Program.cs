using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_Invoke_For_ForEach
{
    class ParallelLoopsDemo
    {
        public static IEnumerable<int> Range(int start, int end, int step)
        {
            for (int i = start; i < end; i += step)
            {
                yield return i;
            }
        }

        static void Main(string[] args)
        {
            var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

            Parallel.Invoke(a, b, c);
            // these are blocking operations; wait on all tasks

            Parallel.For(1, 11, x =>
            {
                Console.Write($"{x * x}\t");
            });
            Console.WriteLine();

            // has a step strictly equal to 1
            // if you want something else...
            Parallel.ForEach(Range(1, 20, 3), Console.WriteLine);

            string[] letters = { "oh", "what", "a", "night" };
            var po = new ParallelOptions();
            po.MaxDegreeOfParallelism = 2;
            Parallel.ForEach(letters, po, letter =>
            {
                Console.WriteLine($"{letter} has length {letter.Length} (task {Task.CurrentId})");
            });
        }
    }
}