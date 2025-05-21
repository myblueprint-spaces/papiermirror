using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

internal class MaximumListDepthAttributeTests
{
    private const int MaxDepth = 5;
    private class ValidationTarget
    {
        public ValidationTarget(Node? document)
        {
            Document = document;
        }

        [MaximumListDepth(MaxDepth)]
        public Node? Document { get; }
    }

    private static bool ValidateNode(Node? node)
    {
        var target = new ValidationTarget(node);
        var context = new ValidationContext(target);
        var results = new List<ValidationResult>();

        return Validator.TryValidateObject(target, context, results, true);
    }

    private static Node GetDocument(int depth)
    {
        var document = new Document { Content = new List<Node> { GetList(depth) } };
        return document;
    }

    private static Node GetList(int depth)
    {
        Node list;
        var root = list = new OrderedList();
        for (var i = 0; i < depth - 1; i++)
        {
            var nestedList = new OrderedList();
            var normalItem = new ListItem { Content = new List<Node> { new Paragraph { Content = new List<Node> { new TextNode { Text = $"Text At Depth {i}" } } }, nestedList } };
            list.Content = new List<Node>(list.Content ?? []) { normalItem };
            list = nestedList;
        }

        return root;
    }

    [Test]
    public async Task TestsValidListDepth()
    {
        var document = GetDocument(MaxDepth);

        var attribute = new MaximumListDepthAttribute(MaxDepth);

        var depth = attribute.MaxDepth(document);
        await Assert.That(depth).IsEqualTo(MaxDepth);

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TestsInvalidListDepth()
    {
        var document = GetDocument(MaxDepth + 1);

        var attribute = new MaximumListDepthAttribute(MaxDepth);

        var depth = attribute.MaxDepth(document);
        await Assert.That(depth).IsEqualTo(MaxDepth + 1);

        var result = ValidateNode(document);
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TestValidSiblingLists()
    {
        var document = new Document
        {
            Content = new List<Node>
        {
            GetList(MaxDepth),
            GetList(1)
        }
        };

        var attribute = new MaximumListDepthAttribute(MaxDepth);
        var depth = attribute.MaxDepth(document);

        await Assert.That(depth).IsEqualTo(MaxDepth);

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task TestInvalidSiblingLists()
    {
        var document = new Document
        {
            Content = new List<Node>
            {
                GetList(MaxDepth),
                GetList(MaxDepth * 2)
            }
        };

        var attribute = new MaximumListDepthAttribute(MaxDepth);
        var depth = attribute.MaxDepth(document);

        await Assert.That(depth).IsEqualTo(MaxDepth * 2);

        var result = ValidateNode(document);
        await Assert.That(result).IsFalse();
    }
}