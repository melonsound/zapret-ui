using ZapretUI.Application.Abstractions;
using ZapretUI.Application.Models;

namespace ZapretUI.Application.Services;

public class RemoveServicesUseCase
{
    private readonly IServiceRemover _remover;

    public RemoveServicesUseCase(IServiceRemover remover)
    {
        _remover = remover;
    }

    public async Task<RemoveResult> ExecuteAsync(CancellationToken ct = default)
    {
        // Удаление идемпотентно (можно вызывать, даже если ничего не установлено)
        var (success, error) = await _remover.RemoveAsync(ct);

        return success
            ? new RemoveResult(true)
            : new RemoveResult(false, error);
    }
}
