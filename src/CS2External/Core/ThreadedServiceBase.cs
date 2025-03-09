using System.Diagnostics;
using Serilog;

namespace CS2External.Core;

public abstract class ThreadedServiceBase : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Thread _thread;

    protected virtual string ThreadName => nameof(ThreadedServiceBase);
    protected virtual TimeSpan ThreadTimeout { get; set; } = new(0, 0, 0, 3);
    protected virtual TimeSpan ThreadFrameSleep { get; set; } = new(0, 0, 0, 0, 1);

    protected ThreadedServiceBase()
    {
        _thread = new Thread(ThreadStart)
        {
            Name = ThreadName
        };
    }

    public virtual void Dispose()
    {
        _thread.Interrupt();
        _cts.Cancel();
        if (!_thread.Join(ThreadTimeout)) 
            Log.Warning("Thread {ThreadName} did not exit gracefully within timeout", ThreadName);
    }

    public void Start()
    {
        Log.Information("Starting {ThreadName} thread", ThreadName);
        _thread.Start();
    }

    private void ThreadStart()
    {
        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                FrameAction();
                Thread.Sleep(ThreadFrameSleep);
            }
        }
        catch (NullReferenceException)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
                { FileName = "steam://rungameid/730", UseShellExecute = true });
        }
    }

    protected abstract void FrameAction();
}