﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voorbeeld_8
{
    public class TaskThreadPool : TaskScheduler, IDisposable
    {
        private readonly BlockingQueue<Task> _queue = new BlockingQueue<Task>();

        private Thread[] _threads;
        private bool _disposed;
        private readonly object _lock = new object();

        public int ThreadCount { get; }


        public TaskThreadPool(int threadCount, bool isBackground = false)
        {
            if (threadCount < 1)
                throw new ArgumentOutOfRangeException(nameof(threadCount), "Must be at least 1");
            ThreadCount = threadCount;
            _threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                _threads[i] = new Thread(ExecuteTasks)
                {
                    IsBackground = isBackground
                };
                _threads[i].Start();
            }
        }

        public Task Run(Action action) => Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, this);

        private void ExecuteTasks()
        {
            while (true)
            {
                var task = _queue.Dequeue();
                if (task == null)
                    return;
                TryExecuteTask(task);
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks() => _queue.ToArray();

        protected override void QueueTask(Task task)
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(TaskThreadPool).FullName);
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                _queue.Enqueue(task);
            }
            catch (ObjectDisposedException e)
            {
                throw new ObjectDisposedException(typeof(TaskThreadPool).FullName, e);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(TaskThreadPool).FullName);
            return !taskWasPreviouslyQueued && TryExecuteTask(task);
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed)
                    return;
                _disposed = true;
            }

            for (int i = 0; i < _threads.Length; i++)
                _queue.Enqueue(null);

            foreach (var thread in _threads)
                thread.Join();
            _threads = null;
            _queue.Dispose();
        }
    }
}
