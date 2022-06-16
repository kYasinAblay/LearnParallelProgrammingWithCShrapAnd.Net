using System;
using System.Threading.Tasks;

namespace AsyncFactoryMethod
{
    class Foo
    {
        private Foo()
        {

        }
        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }
        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Foo x = await Foo.CreateAsync();
        }
    }
}
