using MyBlueprint.PapierMirror.Models.Marks;
using MyBlueprint.PapierMirror.Models.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace MyBlueprint.PapierMirror;

/// <summary>
/// ProseMirror schema. This class should be initialized once and the instance of the schema shared between usages of the serialization/deserialization methods.
/// </summary>
public class Schema
{
    /// <summary>
    /// Supported nodes.
    /// </summary>
    public IReadOnlyCollection<Node> Nodes { get; set; }

    /// <summary>
    /// Supported marks.
    /// </summary>
    public IReadOnlyCollection<Mark> Marks { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Schema"/> class.
    /// </summary>
    /// <param name="nodes"></param>
    public Schema(IEnumerable<Node> nodes)
        : this(nodes, new List<Mark>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Schema"/> class.
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="marks"></param>
    public Schema(IEnumerable<Node> nodes, IEnumerable<Mark> marks)
    {
        Nodes = nodes.ToList();
        Marks = marks.ToList();
    }

    /// <summary>
    /// Default schema.
    /// </summary>
    public static readonly Schema All = new(new Node[]
    {
        new BlockQuote(),
        new BulletList(),
        new CodeBlock(),
        new Document(),
        new HardBreak(),
        new HorizontalRule(),
        new Image(),
        new ListItem(),
        new OrderedList(),
        new Paragraph(),
        new Heading(),
        new TextNode()
    }, new Mark[]
    {
        new Strong(),
        new Code(),
        new Link(),
        new Strike(),
        new Subscript(),
        new Superscript(),
        new TextStyle(),
        new Underline(),
        new Emphasis(),
        new Marked()
    });
}