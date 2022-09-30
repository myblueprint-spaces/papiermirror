using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Marks;

/// <summary>
/// Code font mark. Represented as a &lt;code&gt; element.
/// </summary>
public class Code : Mark
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Code" /> class.
    /// </summary>
    public Code()
        : base("code") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Code" /> class.
    /// </summary>
    /// <param name="node">The HTML Element to initialize from.</param>
    public Code(INode node)
        : this() { }

    /// <inheritdoc/>
    internal override string[] Tags => new[] { "code" };

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("code");
    }
}