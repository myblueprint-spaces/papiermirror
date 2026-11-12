using MyBlueprint.PapierMirror.Models.Marks;
using System.Text;

namespace MyBlueprint.PapierMirror.Razor;

internal static class RenderHelpers
{
    public static string? NullIfEmpty(string? value) => string.IsNullOrEmpty(value) ? null : value;

    public static string? TextAlignStyle(string? textAlign, PapierMirrorRenderOptions options) =>
        options.CssValueSanitizer(textAlign) is { } value ? $"text-align:{value}" : null;

    public static string? TextStyleStyle(TextStyleAttributes? attrs, PapierMirrorRenderOptions options)
    {
        if (attrs is null)
        {
            return null;
        }

        var styles = new StringBuilder();
        Append(styles, "color", attrs.Color, options);
        Append(styles, "font-size", attrs.FontSize, options);
        Append(styles, "background", attrs.Background, options);
        return styles.Length > 0 ? styles.ToString() : null;
    }

    private static void Append(StringBuilder styles, string property, string? value, PapierMirrorRenderOptions options)
    {
        if (options.CssValueSanitizer(value) is { } safe)
        {
            styles.Append(property).Append(':').Append(safe).Append(';');
        }
    }
}