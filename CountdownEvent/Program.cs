using System;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEvent_App
{
    class CountDownEventDemo
    {
        private static int taskCount = 5;
        static CountdownEvent cte = new CountdownEvent(taskCount);
        static Random random = new Random();
        static void Main(string[] args)
        {
            var tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}.");
                    Thread.Sleep(random.Next(3000));
                    cte.Signal(); // also takes a signalcount
                                  //cte.CurrentCount/InitialCount
                    Console.WriteLine($"Exiting task {Task.CurrentId}.");
                });
            }

            var finalTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Waiting for other tasks in task {Task.CurrentId}");
                cte.Wait();
                Console.WriteLine("All tasks completed.");
            });

            finalTask.Wait();
        }
    }
}
