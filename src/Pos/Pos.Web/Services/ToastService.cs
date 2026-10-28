namespace Pos.Web.Services;

public enum ToastKind { Success, Error, Info }

public sealed record ToastMessage(Guid Id, string Title, string? Description, ToastKind Kind);

/// <summary>Raises transient notifications rendered by the Toaster component.</summary>
public sealed class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void Show(string title, string? description = null, ToastKind kind = ToastKind.Info) =>
        OnShow?.Invoke(new ToastMessage(Guid.NewGuid(), title, description, kind));

    public void Success(string title, string? description = null) => Show(title, description, ToastKind.Success);

    public void Error(string title, string? description = null) => Show(title, description, ToastKind.Error);
}
