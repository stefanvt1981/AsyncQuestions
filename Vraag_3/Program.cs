// See https://aka.ms/new-console-template for more information

var w = new Worker();

Parallel.Invoke(async () => await w.DoSomething(), () => w.DoSomethingElse().Wait(), () => w.DoSomethingMore());

Console.ReadKey();

internal class Worker
{
    public async Task DoSomething()
    {
        await Task.Delay(3000);
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