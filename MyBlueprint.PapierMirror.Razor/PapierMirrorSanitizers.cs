using System.Text.RegularExpressions;

namespace MyBlueprint.PapierMirror.Razor;

/// <summary>
/// Default sanitizers for attribute values. Razor already HTML-encodes text and attribute values; these guard
/// against values that are dangerous even when correctly encoded (for example <c>javascript:</c> URLs).
/// </summary>
public static partial class PapierMirrorSanitizers
{
    private static readonly HashSet<string> AllowedSchemes = new(StringComparer.OrdinalIgnoreCase)
    {
        "http", "https", "mailto", "tel",
    };

    /// <summary>
    /// Allows relative URLs and the <c>http</c>, <c>https</c>, <c>mailto</c> and <c>tel</c> schemes.
    /// </summary>
    /// <param name="url">The URL to check.</param>
    /// <returns>The URL, or <see langword="null"/> if it is empty or uses a disallowed scheme.</returns>
    public static string? Url(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        // Browsers ignore control characters and whitespace when parsing the scheme (e.g. "java\tscript:").
        var probe = new string(url.Where(c => c > ' ' && !char.IsControl(c)).ToArray());
        var colon = probe.IndexOf(':');
        var separator = probe.IndexOfAny(['/', '?', '#']);
        if (colon < 0 || (separator >= 0 && separator < colon))
        {
            // No scheme: a relative URL.
            return url;
        }

        return AllowedSchemes.Contains(probe[..colon]) ? url : null;
    }

    /// <summary>
    /// Allows simple CSS values such as keywords, lengths, colours and <c>rgb()</c>/<c>hsl()</c> functions.
    /// Rejects anything that could terminate the declaration or load a resource.
    /// </summary>
    /// <param name="value">The CSS value to check.</param>
    /// <returns>The trimmed value, or <see langword="null"/> if it is empty or unsafe.</returns>
    public static string? CssValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        value = value.Trim();
        if (!SafeCssValue().IsMatch(value) ||
            value.Contains("url(", StringComparison.OrdinalIgnoreCase) ||
            value.Contains("expression(", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return value;
    }

    [GeneratedRegex(@"^[\w\s#%.,()/+-]+$")]
    private static partial Regex SafeCssValue();
}