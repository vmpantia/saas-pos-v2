using Microsoft.AspNetCore.Components;

namespace Pos.Web.Components.UI;

public enum ButtonVariant { Default, Destructive, Outline, Secondary, Ghost, Link }

public enum ButtonSize { Default, Sm, Lg, Icon }

public enum BadgeVariant { Default, Secondary, Destructive, Outline, Success }

public enum AlertVariant { Default, Destructive }

public enum TextAlign { Left, Center, Right }

/// <summary>Base for UI primitives: merges a caller <see cref="Class"/> and forwards unmatched attributes.</summary>
public abstract class UiComponent : ComponentBase
{
    [Parameter] public string? Class { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    // Plain concatenation (no tailwind-merge): avoid passing classes that conflict with a component's defaults.
    protected static string Cn(params string?[] classes) =>
        string.Join(' ', classes.Where(c => !string.IsNullOrWhiteSpace(c)));

    protected static string AlignClass(TextAlign align) => align switch
    {
        TextAlign.Right => "text-right",
        TextAlign.Center => "text-center",
        _ => "text-left"
    };
}
