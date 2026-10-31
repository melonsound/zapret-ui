namespace ZapretUI.Domain.Entities;

public class BypassProcess
{
    public string ProcessName { get; }
    public bool IsRunning { get; private set; }

    public BypassProcess(string processName, bool isRunning)
    {
        ProcessName = processName;
        IsRunning = isRunning;
    }
}
