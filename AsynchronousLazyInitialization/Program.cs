using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using static System.Console;

namespace AsynchronousLazyInitialization
{
    public class Stuff
    {
        public static int value;

        private readonly Lazy<Task<int>> AutoIncValue =
            new Lazy<Task<int>>(async () =>
            {
                await Task.Delay(1000).ConfigureAwait(false);
                return value++;
            });

        private readonly Lazy<Task<int>> AutoIncValue2 =
           new Lazy<Task<int>>(() => Task.Run(async () =>
           {
               await Task.Delay(1000);
               return value++;
           }));

        //Nito.AsyncEx
        public AsyncLazy<int> AutoIncValue3 =
         new AsyncLazy<int>(async () =>
         {
             await Task.Delay(1000);
             return value++;
         });

        //public async Task UseValue()
        //{
        //    int value = await AutoIncValue.Value;
        //}

        public async Task UseValue3()
        {
            var task = await AutoIncValue3.Task;
            int value = task;
        }


    }

    internal class Program
    {
        public static async Task Main(string[] args)
        {
            //Deneme yapıldı
            var stuff = new Stuff();
            await stuff.UseValue3();

            Console.WriteLine(Stuff.value.ToString());
        }
    }
}
