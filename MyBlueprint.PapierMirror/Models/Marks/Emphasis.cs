using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// An emphasis mark. Rendered as an &lt;em&gt; element.
/// </summary>
public class Emphasis : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Emphasis" /> class.
    /// </summary>
    public Emphasis()
        : base("em") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Emphasis" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Emphasis(INode node)
        : this() { }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["em"];

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("em");
    }
}