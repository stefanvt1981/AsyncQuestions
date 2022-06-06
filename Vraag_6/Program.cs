using Nito.AsyncEx;

var m = new MyClass();
await m.DelayAndIncrementAsync();

class MyClass
{
    // This lock protects the _value field.
    private readonly AsyncLock _mutex = new AsyncLock();

    private int _value;

    public async Task DelayAndIncrementAsync()
    {
        using (await _mutex.LockAsync())
        {
            int oldValue = _value;
        }
    }
}

