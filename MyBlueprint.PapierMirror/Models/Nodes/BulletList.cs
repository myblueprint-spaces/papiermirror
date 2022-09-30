using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Bullet list node. Represented as a &lt;ul&gt; element.
/// </summary>
public class BulletList : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BulletList" /> class.
    /// </summary>
    public BulletList()
        : base("bulletList") { }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "ul" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("ul");
    }
}