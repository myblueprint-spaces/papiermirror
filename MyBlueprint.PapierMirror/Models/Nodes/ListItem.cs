using AngleSharp.Dom;
using System;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// List item attributes.
/// </summary>
public record ListItemAttributes : NodeAttributes
{
    /// <summary>
    /// Whether the list item is closed (ProseMirror-specific).
    /// </summary>
    public bool Closed { get; set; }

    /// <summary>
    /// Whether the list item is nested.
    /// </summary>
    public bool Nested { get; set; }
}

/// <summary>
/// List item node. Represented as a &lt;li&gt; element.
/// </summary>
public class ListItem : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ListItem" /> class.
    /// </summary>
    public ListItem()
        : base("listItem")
    {
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["li"];

    /// <inheritdoc/>
    protected internal override Type AttributeType => typeof(ListItemAttributes);

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        return document.CreateElement("li");
    }
}