using System;
using System.Threading;
using System.Threading.Tasks;

namespace Barrier_TaskCoordination
{
    class BarrierDemo
    {
        static Barrier barrier = new Barrier(2, b =>
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished.");
                //b.ParticipantCount
                //b.ParticipantsRemaining

                // add/remove participants
            });

        public static void Water()
        {
            Console.WriteLine("Putting the kettle on (takes a bit longer).");
            Thread.Sleep(2000);
            barrier.SignalAndWait(); // signaling and waiting fused
            Console.WriteLine("Putting water into cup.");
            barrier.SignalAndWait();
            Console.WriteLine("Putting the kettle away.");

        }

        public static void Cup()
        {
            Console.WriteLine("Finding the nicest tea cup (only takes a second).");
            barrier.SignalAndWait();
            Console.WriteLine("Adding tea.");
            barrier.SignalAndWait();
            Console.WriteLine("Adding sugar");
        }

        static void Main(string[] args)
        {
            var water = Task.Factory.StartNew(Water);
            var cup = Task.Factory.StartNew(Cup);

            var tea = Task.Factory.ContinueWhenAll(new[] { water, cup }, tasks =>
            {
                Console.WriteLine("Enjoy your cup of tea.");
            });

            tea.Wait();
        }
    }
}