using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Code block node. Represented as a &lt;blockquote&gt; element.
/// </summary>
public class CodeBlock : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CodeBlock" /> class.
    /// </summary>
    public CodeBlock()
        : base("codeBlock") { }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["pre"];

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("pre");
    }
}