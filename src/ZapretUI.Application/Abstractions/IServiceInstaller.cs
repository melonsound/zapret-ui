namespace ZapretUI.Application.Abstractions;

public interface IServiceInstaller
{
    Task<(bool Success, string? Error)> InstallAsync(string strategyName, CancellationToken ct = default);
}
