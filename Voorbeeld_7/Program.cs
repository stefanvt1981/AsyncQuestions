/* This example installs a noisy sync ctx to trace when it is called to schedule continuations. 
 * There are also special rules on how the sync ctx is propagated from task to task. 
 * I use AsyncLocal objects to keep track of the logical execution stack even across 
 * the different threads where the task DAG nodes are run.
 * 
 * https://candide-guevara.github.io/cs_related/2017/06/10/c-sharp-adv-async.html
 * */

using System.Runtime.CompilerServices;

public class SyncCtxTest
{
    // The value of this variable is captured before awaiting and restored in the continuation
    public static AsyncLocal<string> s_lid = new AsyncLocal<string>();

    public static void Main()
    {
        s_lid.Value = "main";
        foreach (var lid in new String[] { "1", "2" })
            LongRunOperation(lid).Wait();
    }

    public static async Task LongRunOperation(string lid)
    {
        s_lid.Value = "ou_" + lid;                                               // [0] run
        write_message();                                                         // [0] run
        SynchronizationContext.SetSynchronizationContext(new NoisySyncCtx());  // [0] run
                                                                               // ConfigureAwait=false forbids calling the sync ctx to schedule
                                                                               // the continuation => it does not inherit the sync ctx instance
        await Task.Delay(200).ConfigureAwait(false);                             // [0] push 1 + ctx_switch
        write_message();                                                         // [1] pop
                                                                                 // This await will not yield, the continuation 1 will run the beginning
                                                                                 // of LongRunInnerOp method until it finds a real yield point
        await LongRunInnerOp(lid).ConfigureAwait(false);                         // [1] push 2
        write_message();                                                         // [2] pop
    }

    public static async Task LongRunInnerOp(string lid)
    {
        s_lid.Value = "in_" + lid;                                               // [1] run
        write_message();                                                         // [1] run
                                                                                 // Set again the sync ctx, it will be used to schedule the continuation
        SynchronizationContext.SetSynchronizationContext(new NoisySyncCtx());  // [1] run
        await Task.Delay(200).ConfigureAwait(true);                              // [1] push 3, 2 + ctx_switch
                                                                                 // Task.Delay ended, call NoisySyncCtx to schedule the continuation
        write_message();                                                         // [3] pop  2
    }

    public static void write_message([CallerLineNumber] int line = 0, [CallerMemberName] string func = "")
    {
        Console.WriteLine("{0,-16}:{1,-3} tid={2,-3}, lid={3,-3}", func, line, Thread.CurrentThread.ManagedThreadId, s_lid.Value);
    }
} // SyncCtxTest

public class NoisySyncCtx : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object state)
    {
        SyncCtxTest.write_message();
        base.Post(d, state);
    }
    public override void Send(SendOrPostCallback d, object state)
    {
        SyncCtxTest.write_message();
        base.Send(d, state);
    }
} // NoisySyncCtx