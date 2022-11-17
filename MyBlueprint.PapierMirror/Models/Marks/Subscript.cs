using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// A subscript mark. Rendered as a &lt;sub&gt; mark.
/// </summary>
public class Subscript : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Subscript" /> class.
    /// </summary>
    public Subscript()
        : base("subscript") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subscript" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Subscript(INode node)
        : this() { }

    /// <inheritdoc/>
    public override string[] Tags => new[] { "sub" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("sub");
    }
}