using System.Diagnostics;

Stopwatch sw = new Stopwatch();

sw.Start();
var w = new Worker();

Task[] tasks = new Task[3];
tasks[0] = w.DoSomething();
tasks[1] = w.DoSomethingElse();
tasks[2] = w.DoSomethingMore();

Parallel.ForEach(tasks, async t => await t);
sw.Stop();

Console.WriteLine($"Runtime: {sw.Elapsed}");
Console.ReadKey();

internal class Worker
{
    public async Task DoSomething()
    {
        await Task.Delay(300);
        Console.WriteLine("Doing Something...");
        return;
    }

    public async Task DoSomethingElse()
    {
        await Task.Delay(750);
        Console.WriteLine("Doing Something Else...");
    }

    public async Task DoSomethingMore()
    {
        await Task.Delay(500);
        Console.WriteLine("Doing Something More...");
    }
}
