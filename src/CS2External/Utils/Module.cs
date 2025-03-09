using System.Diagnostics;

namespace CS2External.Utils;

public class Module(System.Diagnostics.Process process, ProcessModule processModule) : IDisposable
{
    public System.Diagnostics.Process? Process { get; private set; } = process;
    public ProcessModule? ProcessModule { get; private set; } = processModule;
    
    public void Dispose()
    {
        Process?.Dispose();
        Process = null;

        ProcessModule?.Dispose();
        ProcessModule = null;
        GC.SuppressFinalize(this);
    }
}