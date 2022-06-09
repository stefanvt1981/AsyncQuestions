// See https://aka.ms/new-console-template for more information

//Performance voorbeeld, async met en zonder configure await en synchroon

using System.Diagnostics;

var doasync = new DoAsync();
var doasync2 = new DoAsync2();
var dosync = new DoSync();

Stopwatch sw = new Stopwatch();

List<long> list = new List<long>();

for (int i = 0; i < 10; i++)
{
    sw.Start();
    await doasync.DoSomething();
    sw.Stop();
    list.Add(sw.ElapsedMilliseconds);
    //Console.WriteLine($"Elapsed: {sw.Elapsed.ToString()}");
    sw.Reset();
}
Console.WriteLine($"Async Median Elapsed: {list.Average()}");

for (int i = 0; i < 10; i++)
{
    sw.Start();
    await doasync2.DoSomething();
    sw.Stop();
    list.Add(sw.ElapsedMilliseconds);
    //Console.WriteLine($"Elapsed: {sw.Elapsed.ToString()}");
    sw.Reset();
}
Console.WriteLine($"Async ConfigureAwait Median Elapsed: {list.Average()}");

for (int i = 0; i < 10; i++)
{
    sw.Start();
    dosync.DoSomething();
    sw.Stop();
    list.Add(sw.ElapsedMilliseconds);
    //Console.WriteLine($"Elapsed: {sw.Elapsed.ToString()}");
    sw.Reset();
}
Console.WriteLine($"Sync Median Elapsed: {list.Average()}");


class DoAsync
{
    public async Task DoSomething()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething1 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            await Task.Run(DoSomething2);
        }
    }

    public async Task DoSomething2()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething2 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            await Task.Run(DoSomething3);
        }
    }

    public async Task DoSomething3()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        ///Console.WriteLine($"DoSomething3 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            await Task.Run(DoSomething4);
        }
    }

    public Task DoSomething4()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething4 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
        }

        return Task.CompletedTask;
    }
}

class DoAsync2
{
    public async Task DoSomething()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething1 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
            await Task.Run(DoSomething2).ConfigureAwait(false);
        }
    }

    public async Task DoSomething2()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething2 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
            await Task.Run(DoSomething3).ConfigureAwait(false);
        }
    }

    public async Task DoSomething3()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething3 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
            await Task.Run(DoSomething4).ConfigureAwait(false);
        }
    }

    public Task DoSomething4()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething4 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
        }

        return Task.CompletedTask;
    }
}

class DoSync
{
    public void DoSomething()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething1 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            DoSomething2();
        }
    }

    public void DoSomething2()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething2 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            DoSomething3();
        }
    }

    public void DoSomething3()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething3 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;

            DoSomething4();
        }
    }

    public void DoSomething4()
    {
        var threadId = Thread.CurrentThread.ManagedThreadId;
        //Console.WriteLine($"DoSomething4 treadid: {threadId}");

        for (int i = 0; i < 100; i++)
        {
            var b = i * i;
        }
    }
}
