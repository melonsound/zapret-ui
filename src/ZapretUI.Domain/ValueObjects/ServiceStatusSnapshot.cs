using ZapretUI.Domain.Enums;

namespace ZapretUI.Domain.ValueObjects;

public sealed record ServiceStatusSnapshot
{
    public required ServiceState ZapretState { get; init; }
    public required ServiceState WinDivertState { get; init; }
    public required bool IsWinwsRunning { get; init; }
    public string? InstalledStrategyName { get; init; }
    public required bool HasWinDivertDriverFile { get; init; }
    public required IReadOnlyList<ServiceConflict> Conflicts { get; init; }
    public required DateTimeOffset CapturedAt { get; init; }

    //Domain rules
    public bool IsZapretInstalled => ZapretState != ServiceState.NotInstalled;
    public bool IsZapretActive => ZapretState is ServiceState.Running or ServiceState.StopPending;
    public bool IsBypassActive => ZapretState == ServiceState.Running || IsWinwsRunning;
    public bool CanInstallService => !IsZapretActive;
    public bool CanRemoveService => true;
    public bool RequiresWinDivertCleanup =>
        !IsWinwsRunning && WinDivertState is ServiceState.Running or ServiceState.StopPending;
    public bool HasConflicts => Conflicts.Count > 0;
}
