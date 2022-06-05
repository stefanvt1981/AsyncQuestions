using System;
using System.Threading.Tasks;

namespace Vraag_1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var w = new Worker();

            await w.DoSomething();

            await w.DoSomethingElse();
        }        
    }

    internal class Worker
    {
        public Task DoSomething()
        {
            Console.WriteLine("Doing Something...");
            return Task.CompletedTask;
        }

        public async Task DoSomethingElse()
        {
            await Task.Delay(500);
            Console.WriteLine("Doing Something Else...");
        }
    }
}
