// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

var threadId = Thread.CurrentThread.ManagedThreadId;
Console.WriteLine($"threadid: {threadId}");

Stopwatch sw = new Stopwatch();

sw.Start();
var w = new Worker();

Parallel.Invoke(async () => await w.DoSomething(), () => w.DoSomethingElse().Wait(), () => w.DoSomethingMore());

sw.Stop();

Console.WriteLine($"Runtime: {sw.Elapsed}");
Console.ReadKey();

internal class Worker
{
    public async Task DoSomething()
    {
        await Task.Delay(3000);
        var threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"Doing Something... {threadId}");
        return;
    }

    public async Task DoSomethingElse()
    {
        await Task.Delay(750);
        var threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"Doing Something Else... {threadId}");
    }

    public async Task DoSomethingMore()
    {
        await Task.Delay(500);
        var threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"Doing Something More... {threadId}");
    }
}