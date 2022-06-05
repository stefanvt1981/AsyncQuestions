Thread thread1 = new Thread(ThreadWork.DoWork);

thread1.Start();

for (int i = 0; i < 3; i++)
{
    Console.WriteLine("In main.");
    Thread.Sleep(100);
}

public class ThreadWork
{
    public static void DoWork()
    {
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Working thread...");
            Thread.Sleep(100);
        }
    }
}