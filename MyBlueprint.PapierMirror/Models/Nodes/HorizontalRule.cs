using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Horizontal rule node. Represented as a &lt;hr&gt; element.
/// </summary>
public class HorizontalRule : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HorizontalRule" /> class.
    /// </summary>
    public HorizontalRule()
        : base("horizontalRule") { }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["hr"];

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("hr");
    }
}