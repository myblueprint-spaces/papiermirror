﻿using AngleSharp.Dom;

namespace MyBlueprint.PapierMirror.Models.Nodes;

/// <summary>
/// Ordered list attributes.
/// </summary>
public record OrderedListAttributes : NodeAttributes
{
    /// <summary>
    /// An integer to start counting from for the list items.
    /// </summary>
    public int Start { get; set; } = OrderedList.DefaultStart;
}

/// <summary>
/// Ordered list node. Represented as a &lt;ol&gt; element.
/// </summary>
public class OrderedList : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderedList" /> class.
    /// </summary>
    public OrderedList()
        : base("orderedList")
    {
        Attributes = new OrderedListAttributes();
    }

    /// <inheritdoc/>
    protected internal override string[] Tags => ["ol"];

    internal const int DefaultStart = 1;

    /// <inheritdoc />
    public override bool Equals(Node? node)
    {
        return base.Equals(node) && (node is not OrderedList ol || ol.Attributes == Attributes);
    }

    /// <inheritdoc />
    public override INode GetHtmlNode(IDocument document)
    {
        var node = document.CreateElement("ol");

        var attrs = (OrderedListAttributes?)Attributes;
        if (attrs == null)
        {
            return node;
        }

        if (attrs.Start != DefaultStart)
        {
            node.SetAttribute("start", attrs.Start.ToString());
        }

        return node;
    }
}