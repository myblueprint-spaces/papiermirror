using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// Unlined text mark. Represented as a &lt;u&gt; element.
/// </summary>
public class Underline : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Underline" /> class.
    /// </summary>
    public Underline()
        : base("underline") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Underline" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Underline(INode node)
        : this() { }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "u" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("u");
    }
}