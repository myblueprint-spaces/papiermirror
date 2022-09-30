using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Document = MyBlueprint.PapierMirror.Models.Nodes.Document;
using Node = MyBlueprint.PapierMirror.Node;

namespace MyBlueprint.PapierMirror.Html;

internal class HtmlSerializer
{
    private Dictionary<string, Type> TagMap { get; } = new(StringComparer.OrdinalIgnoreCase);

    private Dictionary<string, Type> MarkMap { get; } = new(StringComparer.OrdinalIgnoreCase);

    private readonly IBrowsingContext _context = BrowsingContext.New(Configuration.Default);

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlSerializer"/> class.
    /// </summary>
    /// <param name="schema"></param>
    public HtmlSerializer(Schema schema)
    {
        foreach (var node in schema.Nodes)
        {
            foreach (var tag in node.Tags)
            {
                TagMap[tag] = node.GetType();
            }
        }

        foreach (var mark in schema.Marks)
        {
            foreach (var tag in mark.Tags)
            {
                MarkMap[tag] = mark.GetType();
            }
        }
    }

    /// <summary>
    /// Convert the ProseMirror <see cref="PapierMirror.Node"/> to HTML.
    /// </summary>
    /// <param name="node"></param>
    /// <returns>A <see cref="Task{String}"/> representing the result of the asynchronous operation.</returns>
    public async Task<string> ConvertToHtmlAsync(Node node)
    {
        var document = await _context.OpenNewAsync();
        return RenderNode(document, node).ToHtml();
    }

    /// <summary>
    /// Convert the HTML to a ProseMirror document.
    /// </summary>
    /// <param name="html"></param>
    /// <returns>A <see cref="Task{Node}"/> representing the result of the asynchronous operation.</returns>
    public async Task<Node> ConvertFromHtmlAsync(string html)
    {
        var document = await _context.OpenAsync(req => req.Content(html));

        var rootNode = document.Body!;

        var pmDoc = new Models.Nodes.Document
        {
            Content = ParseNode(rootNode)
        };

        return pmDoc;
    }

    private readonly List<Mark> _marks = new();

    private IReadOnlyCollection<Node> ParseNode(INode htmlNode)
    {
        var nodes = new List<Node>();

        foreach (var child in htmlNode.ChildNodes)
        {
            Type? markType = null;
            if (!TagMap.TryGetValue(child.NodeName, out var nodeType) && !MarkMap.TryGetValue(child.NodeName, out markType))
            {
                throw new InvalidOperationException($"Tag:{child.NodeName} not registered in schema");
            }

            if (nodeType != null)
            {
                var node = (Node?)Activator.CreateInstance(nodeType,
                    BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.OptionalParamBinding, binder: null, args: new object[] { child }, culture: null);

                if (node == null)
                {
                    if (child.HasChildNodes)
                    {
                        nodes.AddRange(ParseNode(child));
                    }

                    continue;
                }

                if (child.HasChildNodes)
                {
                    node.Content = ParseNode(child);
                }

                if (_marks.Count > 0)
                {
                    node.Marks = _marks.ToList();
                }

                nodes.Add(node);
            }
            else if (markType != null)
            {
                var mark = (Mark?)Activator.CreateInstance(markType,
                    BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.OptionalParamBinding, binder: null, args: new object[] { child }, culture: null);
                if (mark != null)
                {
                    _marks.Add(mark);

                    if (child.HasChildNodes)
                    {
                        nodes.AddRange(ParseNode(child));
                    }

                    if (_marks.Count > 0)
                    {
                        _marks.RemoveAt(_marks.Count - 1);
                    }
                }
                else if (child.HasChildNodes)
                {
                    nodes.AddRange(ParseNode(child));
                }
            }
        }

        return nodes;
    }

    private INode RenderNode(IDocument document, Node node)
    {
        var tag = GetHtmlNode(document, node);

        foreach (var child in node.Content ?? Enumerable.Empty<Node>())
        {
            tag.AppendChild(RenderNode(document, child));
        }

        return tag.ParentElement ?? tag;
    }

    private static INode GetHtmlNode(IDocument document, Node node)
    {
        var resultNode = node.GetHtmlNode(document);
        foreach (var mark in node.Marks?.Reverse() ?? Array.Empty<Mark>())
        {
            var markHtmlNode = mark.GetHtmlNode(document);
            resultNode = markHtmlNode.AppendChild(resultNode).ParentElement!;
        }

        return resultNode;
    }
}