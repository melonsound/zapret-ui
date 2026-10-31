using ZapretUI.Application.Abstractions;

namespace ZapretUI.Tests.Fakes;

public class FakeInstaller : IServiceInstaller
{
    public bool WasCalled { get; private set; }

    public bool ResultToReturn { get; set; } = true;

    public Task<(bool Success, string? Error)> InstallAsync(string strategyName, CancellationToken ct = default)
    {
        WasCalled = true;
        return Task.FromResult((ResultToReturn, (string?)null));
    }
}
