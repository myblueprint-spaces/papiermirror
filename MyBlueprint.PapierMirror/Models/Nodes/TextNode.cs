using AngleSharp.Dom;
using MyBlueprint.PapierMirror.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Text. Represented as a plain text.
/// </summary>
public class TextNode : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextNode" /> class.
    /// </summary>
    public TextNode()
        : base("text") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextNode" /> class.
    /// </summary>
    /// <param name="node">The HTML node to initialize from.</param>
    public TextNode(INode node)
        : this()
    {
        Text = WebUtility.HtmlDecode(node.TextContent.TrimStart('\n'));
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => new[] { "#text" };

    /// <summary>
    /// The node's text content.
    /// </summary>
    [JsonConverter(typeof(DefaultStringConverter))]
    public string? Text { get; set; }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateTextNode(Text ?? string.Empty);
        return node;
    }
}