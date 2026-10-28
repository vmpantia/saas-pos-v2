using Microsoft.AspNetCore.Components;

namespace Pos.Web.Components.UI;

public enum ButtonVariant { Default, Destructive, Outline, Secondary, Ghost, Link }

public enum ButtonSize { Default, Sm, Lg, Icon }

public enum BadgeVariant { Default, Secondary, Destructive, Outline, Success }

public enum AlertVariant { Default, Destructive }

public enum TextAlign { Left, Center, Right }

/// <summary>Shared styling for form controls so Input, Select and NumberInput look identical.</summary>
public static class FieldStyles
{
    public const string Control =
        "flex h-9 w-full min-w-0 rounded-md border border-input bg-transparent px-3 py-1 text-base shadow-xs transition-[color,box-shadow] outline-none " +
        "placeholder:text-muted-foreground disabled:cursor-not-allowed disabled:opacity-50 md:text-sm " +
        "focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50 " +
        "[&.invalid]:border-destructive [&.invalid]:ring-destructive/20";
}

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
