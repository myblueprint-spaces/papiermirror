using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// List item node. Represented as a &lt;li&gt; element.
/// </summary>
public class ListItem : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListItem" /> class.
    /// </summary>
    public ListItem()
        : base("listItem")
    {
    }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "li" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("li");
    }
}