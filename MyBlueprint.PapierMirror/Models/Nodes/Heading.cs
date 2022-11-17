using AngleSharp.Dom;
using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Header attributes.
/// </summary>
public class HeadingAttributes : NodeAttributes
{
    /// <summary>
    /// Header level.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Level { get; set; }

    /// <summary>
    /// Text align.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TextAlign { get; set; }
}

/// <summary>
/// Heading node. Represented as a &lt;h[X]&gt; element.
/// </summary>
public class Heading : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Heading" /> class.
    /// </summary>
    public Heading()
        : base("heading") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Heading" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Heading(IElement node)
        : this()
    {
        Attributes = new HeadingAttributes
        {
            Level = GetLevel(node.NodeName)
        };
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => new[] { "h1", "h2", "h3", "h4", "h5", "h6" };

    /// <inheritdoc/>
    protected internal override Type AttributeType => typeof(HeadingAttributes);

    /// <summary>
    /// Parse the header level from the tag.
    /// </summary>
    /// <param name="tagName">HTML tag name.</param>
    /// <returns>Header level, or null if it can't be parsed.</returns>
    public static int? GetLevel(string tagName)
    {
        var match = Regex.Match(tagName, "^h([1-6])$", RegexOptions.IgnoreCase);
        return match.Success ? Convert.ToInt32(match.Groups[1].Value) : null;
    }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var attrs = (HeadingAttributes?)Attributes;
        return document.CreateElement($"h{attrs?.Level}");
    }
}