using AngleSharp.Dom;
using System;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// Link attributes.
/// </summary>
public class LinkAttributes : MarkAttributes
{
    /// <summary>
    /// Link target.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Target { get; set; }

    /// <summary>
    /// Hypertext reference.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Href { get; set; }

    /// <summary>
    /// Link title.
    /// </summary>
    public string? Title { get; set; }
}

/// <summary>
/// A link.
/// </summary>
public class Link : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Link" /> class.
    /// </summary>
    public Link()
        : base("link") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Link" /> class.
    /// </summary>
    /// <param name="node">The HTML node to initialize from.</param>
    public Link(IElement node)
        : this()
    {
        Attributes = new LinkAttributes
        {
            Target = node.GetAttribute("target"),
            Href = node.GetAttribute("href"),
            Title = node.GetAttribute("title")
        };
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["a"];

    /// <inheritdoc/>
    protected internal override Type AttributeType => typeof(LinkAttributes);

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("a");
        var attrs = (LinkAttributes?)Attributes;
        if (attrs == null)
        {
            return node;
        }

        if (!string.IsNullOrEmpty(attrs.Href))
        {
            node.SetAttribute("href", attrs.Href);
        }

        if (!string.IsNullOrEmpty(attrs.Title))
        {
            node.SetAttribute("title", attrs.Title);
        }

        if (!string.IsNullOrEmpty(attrs.Target))
        {
            node.SetAttribute("target", attrs.Target);
        }

        return node;
    }
}