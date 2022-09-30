using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// A superscript mark. Rendered as a &lt;sup&gt; element.
/// </summary>
public class Superscript : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Superscript" /> class.
    /// </summary>
    public Superscript()
        : base("superscript") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Superscript" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Superscript(INode node)
        : this() { }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "sup" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("sup");
    }
}