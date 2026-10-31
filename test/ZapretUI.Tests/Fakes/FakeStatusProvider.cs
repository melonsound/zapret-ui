using ZapretUI.Application.Abstractions;
using ZapretUI.Domain.ValueObjects;

namespace ZapretUI.Tests.Fakes;

public class FakeStatusProvider : IServiceStatusProvider
{
    private readonly ServiceStatusSnapshot _snapshotToReturn;

    public FakeStatusProvider(ServiceStatusSnapshot snapshotToReturn)
    {
        _snapshotToReturn = snapshotToReturn;
    }

    public Task<ServiceStatusSnapshot> GetStatusAsync(CancellationToken ct = default)
        => Task.FromResult(_snapshotToReturn);
}
