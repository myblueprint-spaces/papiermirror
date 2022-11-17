using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Hard break node. Represented as a &lt;br&gt; element.
/// </summary>
public class HardBreak : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HardBreak" /> class.
    /// </summary>
    public HardBreak()
        : base("hardBreak") { }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "br" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("br");
    }
}