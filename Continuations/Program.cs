﻿using System;
using System.Threading.Tasks;

namespace Continuations
    {
        class CreatingContinuationsDemo
        {
            static void Main(string[] args)
            {
                SimpleContinuation();
                ContinueWhen();
            }

            private static void ContinueWhen()
            {
                var task = Task.Factory.StartNew(() => "Task 1");
                var task2 = Task.Factory.StartNew(() => "Task 2");

                // also ContinueWhenAny
                var task3 = Task.Factory.ContinueWhenAll(new[] { task, task2 },
                  tasks =>
                  {
                      Console.WriteLine("Tasks completed:");
                      foreach (var t in tasks)
                          Console.WriteLine(" - " + t.Result);
                      Console.WriteLine("All tasks done");
                  });

                task3.Wait();
            }

            private static void SimpleContinuation()
            {
                var task = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Boil water (task {Task.CurrentId}, then...");
                    throw null;
                });

                var task2 = task.ContinueWith(t =>
                {
                    // alternatively can also rethrow exceptions
                    if (t.IsFaulted)
                        throw t.Exception.InnerException;

                    Console.WriteLine($"{t.Id} is {t.Status}, so pour into cup  {Task.CurrentId})");
                }/*, TaskContinuationOptions.NotOnFaulted*/);

                try
                {
                    task2.Wait();
                }
                catch (AggregateException ae)
                {
                    ae.Handle(e =>
                    {
                        Console.WriteLine("Exception: " + e);
                        return true;
                    });
                }
            }
        }
    }