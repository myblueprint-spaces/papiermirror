using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// A strong mark. Rendered as a &lt;strong&gt; element.
/// </summary>
public class Strong : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Strong" /> class.
    /// </summary>
    public Strong()
        : base("strong") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Strong" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Strong(INode node)
        : this() { }

    /// <inheritdoc/>
    protected override string[] Tags => new[] { "strong" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("strong");
    }
}