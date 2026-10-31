using ZapretUI.Domain.Enums;

namespace ZapretUI.Domain.ValueObjects;

//TODO: Service state
public sealed record ServiceStatusSnapshot
{
    public required ServiceState ZapretState { get; init; }
    public string? InstalledStrategyName { get; init; }
    public required ServiceState WinDivertState { get; init; }
    public required bool HasWinDivertDriverFile { get; init; }
    public required bool IsWinwsRunning { get; init; }
    public required IReadOnlyList<ServiceConflict> Conflicts { get; init; }
    public required DateTimeOffset CapturedAt { get; init; }
}
