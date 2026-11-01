using System.Diagnostics;
using ZapretUI.Application.Abstractions;
using ZapretUI.Infrastructure.Common;

namespace ZapretUI.Infrastructure.Windows;

public class WindowsServiceRemover : IServiceRemover
{
    public async Task<(bool Success, string? Error)> RemoveAsync(CancellationToken ct = default)
    {
        try
        {
            await ProcessRunner.RunAsync("sc.exe", "stop zapret", ct);
            await ProcessRunner.RunAsync("sc.exe", "delete zapret", ct);

            var winwsProcesses = Process.GetProcessesByName("winws");
            foreach (var p in winwsProcesses)
            {
                try { p.Kill(); } catch {  }
            }

            await ProcessRunner.RunAsync("sc.exe", "stop WinDivert", ct);
            await ProcessRunner.RunAsync("sc.exe", "delete WinDivert", ct);

            await ProcessRunner.RunAsync("sc.exe", "stop WinDivert14", ct);
            await ProcessRunner.RunAsync("sc.exe", "delete WinDivert14", ct);

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, $"Исключение при удалении: {ex.Message}");
        }
    }
}
