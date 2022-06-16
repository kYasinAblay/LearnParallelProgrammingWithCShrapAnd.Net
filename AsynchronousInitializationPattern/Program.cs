using System;
using System.Threading.Tasks;

namespace AsynchronousInitializationPattern
{
    public interface IAsyncInit
    {
        Task InitTask { get; }
    }
    public class MyOtherClass : IAsyncInit
    {
        private readonly MyClass myClass;
        public MyOtherClass(MyClass myClass)
        {
            this.myClass = myClass;
            InitTask = InitAsync();
        }
        public Task InitTask { get; }
        private async Task InitAsync()
        {
            if (myClass is IAsyncInit ai)
                await ai.InitTask;

            await Task.Delay(1000);
        }
    }
    public class MyClass : IAsyncInit
    {
        public MyClass()
        {
            InitTask = InitAsync();
        }
        public Task InitTask { get; }
        private async Task InitAsync()
        {
            await Task.Delay(1000);
        }
    }
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            //var myClass1 = new MyClass();

            //if (myClass1 is IAsyncInit ai)
            //    await ai.InitTask;
            //}

            var myClass = new MyClass();
            var oc = new MyOtherClass(myClass);
            await oc.InitTask;
            
        }
    }
}
