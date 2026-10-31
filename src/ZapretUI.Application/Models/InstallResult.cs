namespace ZapretUI.Application.Models;

public enum InstallStatus
{
    Success,
    /// <summary>
    /// Service already running
    /// </summary>
    Rejected,
    /// <summary>
    /// Infrastructure error
    /// </summary>
    Failed
}

public sealed record InstallResult(InstallStatus Status, string? ErrorMessage = null);
