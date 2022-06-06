/* This example shows how to yield from tasks in order to let others be scheduled 
 * on that thread and not hog all resources on a non blocking but long computation. 
 * To simulate a cpu constrained machine, I use ConcurrentExclusiveInterleave scheduler 
 * which piles all tasks in a single thread. 
 * This same class can be handy for implementing read/writer tasks.
 * 
 * https://candide-guevara.github.io/cs_related/2017/06/10/c-sharp-adv-async.html
 */


using System.Diagnostics;
using System.Runtime.CompilerServices;

public class YieldTest
{
    public static Stopwatch s_sw = Stopwatch.StartNew();

    public static void Main()
    {
        var tasks = new List<Task>();
        // We use a task scheduler that runs all tasks sequentially to simulate a bottleneck
        var sched = new ConcurrentExclusiveSchedulerPair();
        // By starting the task using the factory we are setting the exclusive TaskScheduler 
        // to be used by await on all sub tasks
        var facto = new TaskFactory(sched.ExclusiveScheduler);

        for (int i = 0; i < 4; i++)
        {
            // We need to copy i otherwise the lambda would capture a reference to the SAME loop variable
            int capture_i = i;
            // We need to use Unwrap() otherwise we would return a Task<Task> and the program would exit
            // once all lambdas would have finished without waiting for long_run_operation
            tasks.Add(facto.StartNew(() => long_run_operation(capture_i)).Unwrap());
        }

        Task.WhenAll(tasks).Wait();
    }

    public static async Task long_run_operation(int index)
    {
        write_message(index);
        await Task.Yield();

        for (uint i = 1; i < (1 << 24); i++)
        {
            if (i % (1 << 22) == 0)
            {
                write_message(index, 100 * i / (1 << 24));
                // By yielding here we requeue the rest of this task to the end of the queue
                // allowing other tasks to make progress
                await Task.Yield();
            }
        }
    }

    public static void write_message(int? index = null, uint progress = 0,
                                      [CallerLineNumber] int line = 0, [CallerMemberName] string func = "")
    {
        Console.WriteLine("{4}:{5,-3} at={1,-5}, tid={2,-3}, idx={0,-3}, prog={3,-3}",
             index, s_sw.ElapsedMilliseconds, Thread.CurrentThread.ManagedThreadId, progress, func, line);
    }
} // YieldTest