using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// A strike-through mark. Rendered as a &lt;s&gt; element.
/// </summary>
public class Strike : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Strike" /> class.
    /// </summary>
    public Strike()
        : base("strike") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Strike" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Strike(INode node)
        : this() { }

    /// <inheritdoc/>
    protected internal override string[] Tags => new[] { "s" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("s");
    }
}