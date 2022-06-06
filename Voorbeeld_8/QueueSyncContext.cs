using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voorbeeld_8
{
    public sealed class QueueSyncContext : SynchronizationContext, IDisposable
    {
        private readonly BlockingQueue<SendOrPostCallbackItem> _messageQueue = new BlockingQueue<SendOrPostCallbackItem>();

        public override SynchronizationContext CreateCopy() => this;

        public override void Post(SendOrPostCallback d, object state)
        {
            _messageQueue.Enqueue(new SendOrPostCallbackItem(ExecutionType.Post, d, state, null));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            using (var handle = new ManualResetEventSlim())
            {
                var callbackItem = new SendOrPostCallbackItem(ExecutionType.Send, d, state, handle);
                _messageQueue.Enqueue(callbackItem);
                handle.Wait();
                if (callbackItem.Exception != null)
                    throw callbackItem.Exception;
            }
        }

        public SendOrPostCallbackItem Receive()
        {
            var message = _messageQueue.Dequeue();
            if (message == null)
                throw new ThreadInterruptedException("Message queue was unblocked.");
            return message;
        }

        public void Unblock() => _messageQueue.Enqueue(null);

        public void Unblock(int count)
        {
            for (; count > 0; count--)
                _messageQueue.Enqueue(null);
        }

        public void Dispose() => _messageQueue.Dispose();
    }


    public class SendOrPostCallbackItem
    {
        public SendOrPostCallbackItem(ExecutionType executionType, SendOrPostCallback callback, object state, ManualResetEventSlim signalComplete)
        {
            ExecutionType = executionType;
            Callback = callback;
            State = state;
            SignalComplete = signalComplete;
        }

        private ExecutionType ExecutionType { get; }
        private SendOrPostCallback Callback { get; }
        private object State { get; }
        private ManualResetEventSlim SignalComplete { get; }
        public Exception Exception { get; private set; }

        public void Execute()
        {
            switch (ExecutionType)
            {
                case ExecutionType.Post:
                    Callback(State);
                    break;
                case ExecutionType.Send:
                    try
                    {
                        Callback(State);
                    }
                    catch (Exception e)
                    {
                        Exception = e;
                    }
                    SignalComplete.Set();
                    break;
                default:
                    throw new ArgumentException($"{nameof(ExecutionType)} is not a valid value.");
            }
        }
    }

    public enum ExecutionType
    {
        Post,
        Send,
    }
}
