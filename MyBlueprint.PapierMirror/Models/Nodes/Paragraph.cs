using AngleSharp.Dom;
using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Paragraph attributes.
/// </summary>
public class ParagraphAttributes : NodeAttributes
{
    /// <summary>
    /// Text alignment.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TextAlign { get; set; }
}

/// <summary>
/// Paragraph node. Represented as a &lt;p&gt; element.
/// </summary>
public class Paragraph : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Paragraph" /> class.
    /// </summary>
    public Paragraph()
        : base("paragraph") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Paragraph" /> class.
    /// </summary>
    /// <param name="node"></param>
    public Paragraph(IElement node)
        : this()
    {
        Attributes = GetAttrs(node);
    }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "p" };

    /// <inheritdoc/>
    public override Type AttributeType => typeof(ParagraphAttributes);

    private static ParagraphAttributes GetAttrs(IElement node)
    {
        var attributes = new ParagraphAttributes();
        var styleAttribute = node.GetAttribute("style");

        if (styleAttribute == null)
        {
            return attributes;
        }

        foreach (var style in styleAttribute.Split(';').Select(style => style.Replace(" ", string.Empty)))
        {
            const string textAlign = "text-align:";
            if (style.StartsWith(textAlign))
            {
                attributes.TextAlign = style[textAlign.Length..];
            }
        }

        return attributes;
    }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("p");
        var attrs = (ParagraphAttributes?)Attributes;
        if (attrs == null)
        {
            return node;
        }

        if (!string.IsNullOrEmpty(attrs.TextAlign))
        {
            node.SetAttribute("style", $"text-align:{attrs.TextAlign}");
        }

        return node;
    }
}