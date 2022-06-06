var w = new Worker();

w.DoSomething();

Console.ReadKey();


internal class Worker
{
    public async Task DoSomething()
    {
        Console.WriteLine("Doing Something...");
        await Task.Delay(1000);

        throw new ArgumentException("Bla");
    }    
}
