using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Document node. Represented as a &lt;html&gt; and &lt;body&gt; element.
/// </summary>
public class Document : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Document" /> class.
    /// </summary>
    public Document()
        : base("doc") { }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "html", "body" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("html");
        var body = document.CreateElement("body");
        node.AppendChild(body);
        return body;
    }
}