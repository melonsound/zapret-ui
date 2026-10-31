using ZapretUI.Application.Abstractions;
using ZapretUI.Domain.ValueObjects;

namespace ZapretUI.Application.Services;

public class GetServiceStatusUseCase
{
    private readonly IServiceStatusProvider _statusProvider;

    public GetServiceStatusUseCase(IServiceStatusProvider statusProvider)
    {
        _statusProvider = statusProvider;
    }

    public Task<ServiceStatusSnapshot> ExecuteAsync(CancellationToken ct = default)
    {
        return _statusProvider.GetStatusAsync(ct);
    }
}
