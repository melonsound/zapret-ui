namespace ZapretUI.Application.Abstractions;

public interface IServiceRemover
{
    Task<(bool Success, string? Error)> RemoveAsync(CancellationToken ct = default);
}
