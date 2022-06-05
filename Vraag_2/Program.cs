﻿var w = new Worker();

w.DoSomething().Wait();

w.DoSomethingElse().GetAwaiter().GetResult();


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