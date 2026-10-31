using ZapretUI.Application.Abstractions;
using ZapretUI.Application.Models;
using ZapretUI.Domain.Entities;

namespace ZapretUI.Application.Services;

public class InstallServiceUseCase
{
    private readonly IServiceStatusProvider _statusProvider;
    private readonly IServiceInstaller _installer;

    public InstallServiceUseCase(IServiceStatusProvider statusProvider, IServiceInstaller installer)
    {
        _statusProvider = statusProvider;
        _installer = installer;
    }

    public async Task<InstallResult> ExecuteAsync(string strategyName, CancellationToken ct = default)
    {
        var snapshot = await _statusProvider.GetStatusAsync(ct);

        var zapretService = new ZapretService("zapret", snapshot.ZapretState, snapshot.InstalledStrategyName);

        if (zapretService.IsActive)
        {
            return new InstallResult(InstallStatus.Rejected,
                "Служба уже запущена. Сначала удалите текущую службу (Remove Services).");
        }

        var (success, error) = await _installer.InstallAsync(strategyName, ct);

        return success
            ? new InstallResult(InstallStatus.Success)
            : new InstallResult(InstallStatus.Failed, error);
    }
}
