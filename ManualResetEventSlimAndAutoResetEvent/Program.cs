
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventSlimAndAutoResetEvent
{
    class ResetEvents
    {
        public static void Manual()
        {
            var evt = new ManualResetEventSlim();
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Boiling water...");
                for (int i = 0; i < 30; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }
                Console.WriteLine("Water is ready.");
                evt.Set();
            }, token);

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                evt.Wait(5000, token);
                Console.WriteLine("Here is your tea!");
                Console.WriteLine($"Is the event set? {evt.IsSet}");

                evt.Reset();
                evt.Wait(1000, token); // already set!
                    Console.WriteLine("That was a nice cup of tea!");
            }, token);

            makeTea.Wait(token);
        }

        static void Main(string[] args)
        {
            //Manual();
            Automatic();
        }

        private static void Automatic()
        {
            // try switching between manual and auto :)
            var evt = new AutoResetEvent(false);

            evt.Set(); // ok, it's set

            evt.WaitOne(); // this is ok but, in auto, it causes a reset

            if (evt.WaitOne(1000))
            {
                Console.WriteLine("Succeeded");
            }
            else
            {
                Console.WriteLine("Timed out");
            }
        }
    }
}