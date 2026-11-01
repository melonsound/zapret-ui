using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Win32;
using ZapretUI.Application.Abstractions;
using ZapretUI.Domain.Enums;
using ZapretUI.Domain.ValueObjects;

namespace ZapretUI.Infrastructure.Windows;

public class WindowsServiceStatusProvider : IServiceStatusProvider
{
    public async Task<ServiceStatusSnapshot> GetStatusAsync(CancellationToken ct = default)
    {
        var zapretState = await GetServiceStateAsync("zapret", ct);

        var winDivertState = await GetServiceStateAsync("WinDivert", ct);

        var isWinwsRunning = await IsProcessRunningAsync("winws", ct);

        string? installedStrategyName = null;
        if (zapretState != ServiceState.NotInstalled)
        {
            installedStrategyName = await GetInstalledStrategyNameAsync(ct);
        }

        bool hasDriverFile = CheckWinDivertDriverFile();

        // TODO: Coflict lists
        return new ServiceStatusSnapshot
        {
            ZapretState = zapretState,
            WinDivertState = winDivertState,
            IsWinwsRunning = isWinwsRunning,
            InstalledStrategyName = installedStrategyName,
            HasWinDivertDriverFile = hasDriverFile,
            Conflicts = new List<ServiceConflict>(), // Empty
            CapturedAt = DateTimeOffset.Now
        };
    }

    private async Task<ServiceState> GetServiceStateAsync(string serviceName, CancellationToken ct)
    {
        var service = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
        if (service == null) return ServiceState.NotInstalled;

        return service.Status switch
        {
            ServiceControllerStatus.Running => ServiceState.Running,
            ServiceControllerStatus.Stopped => ServiceState.Stopped,
            ServiceControllerStatus.StopPending => ServiceState.StopPending,
            _ => ServiceState.NotInstalled
        };
    }

    private async Task<bool> IsProcessRunningAsync(string processName, CancellationToken ct)
    {
        try
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length > 0)
                return true;

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    private async Task<string?> GetInstalledStrategyNameAsync(CancellationToken ct)
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\zapret");
            var value = key?.GetValue("zapret-discord-youtube") as string;
            return value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private bool CheckWinDivertDriverFile()
    {
        var baseDir = AppContext.BaseDirectory;
        var driverPath = Path.Combine(baseDir, "ZapretCore", "bin", "WinDivert64.sys");

        return File.Exists(driverPath);
    }
}
