using AngleSharp.Dom;
using System;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror;

/// <summary>
/// An object holding the attributes of the mark.
/// </summary>
public class MarkAttributes
{
}

/// <summary>
/// A mark is a piece of information that can be attached to a node, such as it being emphasized, in code font, or a link. It has a type and optionally a set of attributes that provide further information (such as the target of the link).
/// </summary>
public abstract class Mark
{
    /// <summary>
    /// The type of this mark.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// The attributes associated with this mark.
    /// </summary>
    [JsonPropertyName("attrs"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MarkAttributes? Attributes { get; set; }

    /// <summary>
    /// An array of supported HTML tags for this mark.
    /// </summary>
    public abstract string[] Tags { get; }

    /// <summary>
    /// The <see cref="System.Type"/> of the attribute.
    /// </summary>
    public virtual Type AttributeType { get; } = typeof(MarkAttributes);

    /// <summary>
    /// Initializes a new instance of the <see cref="Mark" /> class.
    /// </summary>
    /// <param name="type">The mark type.</param>
    protected Mark(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Create a new HTML node representing this mark.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public abstract INode GetHtmlNode(IDocument document);
}