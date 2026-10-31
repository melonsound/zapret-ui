namespace ZapretUI.Domain.Enums;

public enum ServiceConflictKind
{
    ZapretStopPending,
    WinDivertActiveWithoutWinws,
    ConflictingBypassService,
    MissingWinDivertDriver
}
