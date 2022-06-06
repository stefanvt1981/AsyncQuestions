var threadId = Thread.CurrentThread.ManagedThreadId;
Console.WriteLine($"threadid: {threadId}");

var w = new Worker();

w.DoSomething().Wait();

w.DoSomethingElse().GetAwaiter().GetResult();


internal class Worker
{
    public Task DoSomething()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"Doing Something... {threadId}");
        return Task.CompletedTask;
    }

    public async Task DoSomethingElse()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        await Task.Delay(500);
        Console.WriteLine($"Doing Something Else... {threadId}");
    }
}