/* Writing a Custom Task Scheduler with Dotnet 
 * Dotnet developers are well afforded with easy-to-use multithreading libraries within the .NET Framework. 
 * This is especially true in recent versions, most notably with the addition of async/await in C# 5.0. However, 
 * it is sometimes necessary to write bespoke logic to handle task scheduling in some exceptional cases.
 * 
 * Rather than writing our own logic from scratch, we can utilise the existing infrastructure for scheduling tasks. 
 * This approach will allow us to continue to make use of C# and .NET Task Pool functionality, such as async/await, 
 * but with our custom code handling the scheduling work in the background. This may sound complex, 
 * but it’s actually pretty straightforward to implement and makes the scheduler far more convenient.
 * 
 * Why bother?
 * In most cases, it won’t be necessary to implement your own scheduler. It’s normally far more practical to use functionality 
 * already built into .NET than to write your own bespoke implementation to achieve the same thing. However, in some 
 * cases custom scheduling logic is needed, especially for handling semi-critical tasks and complex task 
 * prioritisation. In these cases and others, the builtin scheduling logic simply won’t do, rendering it necessary to
 * write custom code to do the job.
 * 
 * This can also be considered a useful educational exercise for those with some time on their hands, 
 * since it can teach you a lot about the internal workings of a scheduler. In this vein, we will cover writing a 
 * basic scheduler, leaving any more complex bespoke code required for specific cases as an exercise for the reader.
 *  
 * https://samuelcollins.dev/posts/dotnet-custom-scheduler/
 */

using System.Text;
using Voorbeeld_8;

var scheduler = new SchedulingSyncContext();
SynchronizationContext.SetSynchronizationContext(scheduler.SynchronizationContext);
var builder = new StringBuilder();
WriteTest();
Thread.Sleep(1000);
Console.WriteLine(builder.ToString());
scheduler.Dispose();

async void WriteTest()
{
    var t1 = Write1();
    var t2 = Write2();
    builder.AppendLine("WriteTest");
    await Task.WhenAll(t1, t2);
    await Write3();
}
async Task Write1()
{
    // Yield the thread (runs the remainder of the async code as a callback on the SchedulingSyncContext thread)
    await Task.Yield();
    builder.AppendLine("Test1");
}
async Task Write2()
{
    // Wait for 10 milliseconds before continuing (uses callbacks)
    await Task.Delay(10);
    builder.AppendLine("Test2");
}
async Task Write3()
{
    // Run these two tasks on the thread pool (could come out in any order)
    var tA = Task.Run(() => builder.AppendLine("Write3 - 1"));
    var tB = Task.Run(() => builder.AppendLine("Write3 - 2"));
    // Wait for them to finish before continuing (uses callbacks)
    await Task.WhenAll(tA, tB);
}