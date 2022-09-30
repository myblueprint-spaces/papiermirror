using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror;

/// <summary>
/// Default node attributes.
/// </summary>
public class NodeAttributes
{
}

/// <summary>
/// A node describes the type of the content, and holds a fragment containing its children.
/// </summary>
public abstract class Node
{
    /// <summary>
    /// The type of this node.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The attributes associated with this node.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), JsonPropertyName("attrs")]
    public NodeAttributes? Attributes { get; internal set; }

    /// <summary>
    /// The child <see cref="Node"/>s.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Node>? Content { get; set; }

    /// <summary>
    /// The <see cref="Mark"/>s that are applied to this node.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<Mark>? Marks { get; set; }

    /// <summary>
    /// An array of supported HTML tags for this node.
    /// </summary>
    internal abstract string[] Tags { get; }

    /// <summary>
    /// The <see cref="System.Type"/> of the attribute.
    /// </summary>
    internal virtual Type AttributeType { get; } = typeof(NodeAttributes);

    /// <summary>
    /// Initializes a new instance of the <see cref="Node" /> class.
    /// </summary>
    /// <param name="type">The node type.</param>
    protected Node(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Create a new HTML node representing this node.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public abstract INode GetHtmlNode(IDocument document);
}