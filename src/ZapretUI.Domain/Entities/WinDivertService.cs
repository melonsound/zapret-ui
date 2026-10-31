using ZapretUI.Domain.Enums;

namespace ZapretUI.Domain.Entities;

public class WinDivertService
{
    public string Name { get; }
    public ServiceState State { get; private set; }
    public bool HasDriverFileOnDisk { get; private set; }

    public WinDivertService(string name, ServiceState state, bool hasDriverFileOnDisk)
    {
        Name = name;
        State = state;
        HasDriverFileOnDisk = hasDriverFileOnDisk;
    }

    public bool IsActive => State is ServiceState.Running or ServiceState.StopPending;
}
