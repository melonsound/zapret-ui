using ZapretUI.Domain.Enums;

namespace ZapretUI.Domain.Entities;

public class ZapretService
{
    public string Name { get; }
    public ServiceState State { get; private set; }
    public string? InstalledStrategyName { get; private set; }

    public ZapretService(string name, ServiceState state, string? installedStrategyName = null)
    {
        Name = name;
        State = state;
        InstalledStrategyName = installedStrategyName;
    }

    //Business rules
    public bool IsInstalled => State != ServiceState.NotInstalled;
    public bool IsActive => State is ServiceState.Running or ServiceState.StopPending;

    public void MarkAsRunning(string strategyName)
    {
        State = ServiceState.Running;
        InstalledStrategyName = strategyName;
    }
}
