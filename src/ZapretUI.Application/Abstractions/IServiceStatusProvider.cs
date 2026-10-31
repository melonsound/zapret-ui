using ZapretUI.Domain.ValueObjects;

namespace ZapretUI.Application.Abstractions;

public interface IServiceStatusProvider
{
    Task<ServiceStatusSnapshot> GetStatusAsync(CancellationToken ct = default);
}
