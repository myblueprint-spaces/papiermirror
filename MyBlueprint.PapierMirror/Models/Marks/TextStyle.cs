using AngleSharp.Dom;
using System;
using System.Linq;
using System.Text;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// Text style attributes.
/// </summary>
public class TextStyleAttributes : MarkAttributes
{
    /// <summary>
    /// Font color.
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Font size.
    /// </summary>
    public string? FontSize { get; set; }

    /// <summary>
    /// Background color.
    /// </summary>
    public string? Background { get; set; }
}

/// <summary>
/// A text style mark for applying custom styling. Rendered as a &lt;span&gt; element.
/// </summary>
public class TextStyle : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextStyle" /> class.
    /// </summary>
    public TextStyle()
        : base("textStyle") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextStyle" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public TextStyle(IElement node)
        : this()
    {
        Attributes = GetAttrs(node);
    }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "span" };

    /// <inheritdoc/>
    public override Type AttributeType => typeof(TextStyleAttributes);

    private const string Color = "color:", FontSize = "font-size:", Background = "background:";

    /// <summary>
    /// Parses the attributes from the HTML element.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static TextStyleAttributes? GetAttrs(IElement node)
    {
        var attributes = new TextStyleAttributes();
        var styleAttribute = node.GetAttribute("style");

        if (styleAttribute == null)
        {
            return attributes;
        }

        foreach (var style in styleAttribute.Split(';').Select(style => style.Replace(" ", string.Empty)))
        {
            if (style.StartsWith(Color))
            {
                attributes.Color = style[Color.Length..];
            }
            else if (style.StartsWith(FontSize))
            {
                attributes.FontSize = style[FontSize.Length..];
            }
            else if (style.StartsWith(Background))
            {
                attributes.FontSize = style[Background.Length..];
            }
        }

        return attributes;
    }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("span");

        var attrs = (TextStyleAttributes?)Attributes;
        if (attrs == null)
        {
            return node;
        }

        var styles = new StringBuilder();

        if (!string.IsNullOrEmpty(attrs.Color))
        {
            styles.Append($"{Color}{attrs.Color};");
        }

        if (!string.IsNullOrEmpty(attrs.FontSize))
        {
            styles.Append($"{FontSize}{attrs.FontSize};");
        }

        if (!string.IsNullOrEmpty(attrs.Background))
        {
            styles.Append($"{Background}{attrs.Background};");
        }

        var styleString = styles.ToString();
        if (!string.IsNullOrEmpty(styleString))
        {
            node.SetAttribute("style", styleString);
        }

        return node;
    }
}