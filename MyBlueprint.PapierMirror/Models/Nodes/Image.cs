﻿using AngleSharp.Dom;
using System;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Image attributes.
/// </summary>
public record ImageAttributes : NodeAttributes
{
    /// <summary>
    /// Alternate text.
    /// </summary>
    public string? Alt { get; set; }

    /// <summary>
    /// Source link.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Src { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Image width.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Width { get; set; }

    /// <summary>
    /// Image height.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Height { get; set; }
}

/// <summary>
/// Image node. Represented as a &lt;img&gt; element.
/// </summary>
public class Image : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    public Image()
        : base("image") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="src">The image source.</param>
    /// <param name="alt">The alternate text.</param>
    /// <param name="title">The image title.</param>
    /// <param name="width">The image width.</param>
    /// <param name="height">The image height.</param>
    public Image(string? src = null, string? alt = null, string? title = null, int? width = null, int? height = null)
        : base("image")
    {
        Attributes = new ImageAttributes
        {
            Alt = alt,
            Src = src,
            Title = title,
            Width = width,
            Height = height
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Image" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Image(IElement node)
        : this()
    {
        Attributes = new ImageAttributes
        {
            Alt = node.GetAttribute("alt"),
            Src = node.GetAttribute("src"),
            Title = node.GetAttribute("title"),
            Width = int.TryParse(node.GetAttribute("width"), out var widthVal) ? widthVal : null,
            Height = int.TryParse(node.GetAttribute("height"), out var heightVal) ? heightVal : null
        };
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["img"];

    /// <inheritdoc/>
    protected internal override Type AttributeType => typeof(ImageAttributes);

    /// <inheritdoc />
    public override bool Equals(Node? node)
    {
        return base.Equals(node) && (node is not Image im || im.Attributes == Attributes);
    }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("img");
        var attrs = (ImageAttributes?)Attributes;
        if (attrs == null)
        {
            return node;
        }

        if (!string.IsNullOrEmpty(attrs.Src))
        {
            node.SetAttribute("src", attrs.Src);
        }

        if (!string.IsNullOrEmpty(attrs.Alt))
        {
            node.SetAttribute("src", attrs.Alt);
        }

        if (!string.IsNullOrEmpty(attrs.Title))
        {
            node.SetAttribute("src", attrs.Title);
        }

        if (attrs.Width.HasValue)
        {
            node.SetAttribute("width", attrs.Width.Value.ToString());
        }

        if (attrs.Height.HasValue)
        {
            node.SetAttribute("height", attrs.Height.Value.ToString());
        }

        return node;
    }
}