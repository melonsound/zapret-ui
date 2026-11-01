using System.ServiceProcess;
using System.Text;
using ZapretUI.Application.Abstractions;
using ZapretUI.Infrastructure.Common;

namespace ZapretUI.Infrastructure.Windows;

public class WindowsServiceInstaller : IServiceInstaller
{
    public async Task<(bool Success, string? Error)> InstallAsync(string strategyName, CancellationToken ct = default)
    {
        string argumentsForWinws = await GetArgumentsFromBatFileAsync(strategyName, ct);
        if (string.IsNullOrWhiteSpace(argumentsForWinws))
        {
            return (false, $"Не удалось прочитать аргументы из стратегии {strategyName}");
        }

        var binPath = Path.Combine(AppContext.BaseDirectory, "ZapretCore", "bin", "winws.exe");

        string scArgs = $"create zapret binPath= \"\\\"{binPath}\\\" {argumentsForWinws}\" DisplayName= \"zapret\" start= auto";

        var result = await ProcessRunner.RunAsync("sc.exe", scArgs, ct);

        if (result.ExitCode != 0)
        {
            return (false, $"Ошибка sc create: {result.Error}");
        }
        string regArgs = $"add \"HKLM\\System\\CurrentControlSet\\Services\\zapret\" /v zapret-discord-youtube /t REG_SZ /d \"{strategyName}\" /f";
        await ProcessRunner.RunAsync("reg.exe", regArgs, ct);

        // Run service
        var startResult = await ProcessRunner.RunAsync("sc.exe", "start zapret", ct);
        if (startResult.ExitCode != 0)
        {
             return (false, $"Служба создана, но не запустилась: {startResult.Error}");
        }

        return (true, null);
    }

    private async Task<string> GetArgumentsFromBatFileAsync(string strategyName, CancellationToken ct)
    {
        var batPath = Path.Combine(AppContext.BaseDirectory, "ZapretCore", strategyName);
        if (!File.Exists(batPath)) return string.Empty;

        var lines = await File.ReadAllLinesAsync(batPath, ct);

        var startIndex = Array.FindIndex(lines, l => l.Contains("winws.exe"));
        if (startIndex == -1) return string.Empty;

        var argsBuilder = new StringBuilder();

        for (int i = startIndex; i < lines.Length; i++)
        {
            var line = lines[i].Trim();

            if (i == startIndex)
            {
                var idx = line.IndexOf("winws.exe") + "winws.exe".Length;
                line = line.Substring(idx).Trim();
            }

            if (line.EndsWith("^"))
            {
                line = line.Substring(0, line.Length - 1).Trim();
                argsBuilder.Append(line).Append(" ");
            }
            else
            {
                argsBuilder.Append(line);
                break;
            }
        }

        var rawArgs = argsBuilder.ToString();

        var binPath = Path.Combine(AppContext.BaseDirectory, "ZapretCore", "bin");
        var listsPath = Path.Combine(AppContext.BaseDirectory, "ZapretCore", "lists");

        rawArgs = rawArgs.Replace("%BIN%", binPath + "\\").Replace("%LISTS%", listsPath + "\\");

        //TODO: use gamefilter ui
        rawArgs = rawArgs.Replace("%GameFilterTCP%", "1024-65535")
                          .Replace("%GameFilterUDP%", "1024-65535");

        return rawArgs;
    }
}
