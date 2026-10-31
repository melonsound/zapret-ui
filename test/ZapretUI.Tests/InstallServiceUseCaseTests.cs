using ZapretUI.Application.Models;
using ZapretUI.Application.Services;
using ZapretUI.Domain.Enums;
using ZapretUI.Domain.ValueObjects;
using ZapretUI.Tests.Fakes;

namespace ZapretUI.Tests;

public class InstallServiceUseCaseTests
{
    [Fact]
    public async Task Install_WhenServiceIsActive_ReturnsRejected()
    {
        //ARRANGE
        var fakeSnapshot = new ServiceStatusSnapshot
        {
            ZapretState = ServiceState.Running,
            WinDivertState = ServiceState.Stopped,
            HasWinDivertDriverFile = true,
            IsWinwsRunning = true,
            InstalledStrategyName = "general.bat",
            CapturedAt = DateTime.UtcNow,
            Conflicts = []
        };


        var fakeProvider = new FakeStatusProvider(fakeSnapshot);
        var fakeInstaller = new FakeInstaller();
        var useCase = new InstallServiceUseCase(fakeProvider, fakeInstaller);

        // ACT
        var result = await useCase.ExecuteAsync("general.bat");

        // ASSERT
        Assert.Equal(InstallStatus.Rejected, result.Status);
        Assert.False(fakeInstaller.WasCalled);
    }
}
