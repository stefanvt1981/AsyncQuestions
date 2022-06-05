// Queue the task.
ThreadPool.QueueUserWorkItem(ThreadProc);
Console.WriteLine("Main thread does some work, then sleeps.");
Thread.Sleep(1000);

Console.WriteLine("Main thread exits.");


// This thread procedure performs the task.
void ThreadProc(Object stateInfo)
{
    // No state object was passed to QueueUserWorkItem, so stateInfo is null.
    Console.WriteLine("Hello from the thread pool.");
}