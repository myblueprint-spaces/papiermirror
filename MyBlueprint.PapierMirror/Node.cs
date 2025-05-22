using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror;

/// <summary>
/// Default node attributes.
/// </summary>
public record NodeAttributes
{
}

/// <summary>
/// A node describes the type of the content, and holds a fragment containing its children.
/// </summary>
public abstract class Node : IEquatable<Node>
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
    protected internal abstract string[] Tags { get; }

    /// <summary>
    /// The <see cref="System.Type"/> of the attribute.
    /// </summary>
    protected internal virtual Type AttributeType { get; } = typeof(NodeAttributes);

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

    /// <inheritdoc />
    public virtual bool Equals(Node? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Type != other.Type) return false;

        if ((Content == null || other.Content == null) && !ReferenceEquals(Content, other.Content)) return false;
        if (Content != null && other.Content != null)
        {
            if (!CompareEnumerators(Content, other.Content))
            {
                return false;
            }
        }

        if ((Marks == null || other.Marks == null) && !ReferenceEquals(Marks, other.Marks)) return false;
        if (Marks != null && other.Marks != null)
        {
            if (!CompareEnumerators(Marks, other.Marks))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    private static bool CompareEnumerators<T>(IEnumerable<T> a, IEnumerable<T> b)
    {
        using var leftEnumerator = a.GetEnumerator();
        using var rightEnumerator = b.GetEnumerator();

        var lHas = leftEnumerator.MoveNext();
        var rHas = rightEnumerator.MoveNext();
        do
        {
            var c = leftEnumerator.Current;
            var o = rightEnumerator.Current;

            if (!c.Equals(o))
            {
                return false;
            }

            lHas = leftEnumerator.MoveNext();
            rHas = rightEnumerator.MoveNext();
            if (lHas != rHas)
            {
                return false;
            }
        } while (lHas && rHas);

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Node)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Attributes, Content, Marks);
    }
}