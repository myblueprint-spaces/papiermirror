using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Block quote node. Represented as a &lt;blockquote&gt; element.
/// </summary>
public class BlockQuote : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockQuote" /> class.
    /// </summary>
    public BlockQuote()
        : base("blockquote") { }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "blockquote" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("blockquote");
    }
}