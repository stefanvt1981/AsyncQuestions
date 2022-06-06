using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vraag_6
{
    class MyClass
    {
        // This lock protects the _value field.
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        private int _value;

        public async Task DelayAndIncrementAsync()
        {
            await _mutex.WaitAsync();
            try
            {
                int oldValue = _value;
                await Task.Delay(TimeSpan.FromSeconds(oldValue));
                _value = oldValue + 1;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }

    class MyClass2
    {        
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);
        private int _value;
        public async Task DelayAndIncrementAsync()
        {
            await _mutex.WaitAsync();
            try
            {
                int oldValue = _value;
                await Task.Delay(TimeSpan.FromSeconds(oldValue));
                _value = oldValue + 1;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}
