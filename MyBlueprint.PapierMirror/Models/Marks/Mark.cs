using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// Marked or Highlighted mark. Represented as a &lt;mark&gt; element.
/// </summary>
public class Marked : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Marked" /> class.
    /// </summary>
    public Marked()
        : base("mark") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Marked" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Marked(INode node)
        : this() { }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "mark" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("mark");
    }
}