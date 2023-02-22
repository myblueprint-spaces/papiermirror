using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;

namespace MyBlueprint.PapierMirror.Test;
[TestFixture]
internal class MaximumListDepthAttributeTests
{
    private static Node GetDocument(int depth)
    {
        var document = new Document { Content = new List<Node> { GetList(depth) }};
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
            list.Content = new List<Node>(list.Content ?? Enumerable.Empty<Node>()) { normalItem };
            list = nestedList;
        }

        return root;
    }

    [Test]
    public void TestsValidListDepth()
    {
        var maxDepth = 4;
        var document = GetDocument(maxDepth);

        var attribute = new MaximumListDepthAttribute(maxDepth);

        var depth = attribute.MaxDepth(document);
        Assert.IsTrue(depth <= maxDepth);

        var result = attribute.IsValid(document);

        Assert.IsTrue(result);
    }

    [Test]
    public void TestsInvalidListDepth()
    {
        var maxDepth = 4;
        var document = GetDocument(maxDepth + 1);
        var attribute = new MaximumListDepthAttribute(maxDepth);

        var depth = attribute.MaxDepth(document);
        Assert.AreEqual(maxDepth + 1, depth);

        var result = attribute.IsValid(document);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestValidSiblingLists()
    {
        const int maxDepth = 5;

        var document = new Document
        {
            Content = new List<Node>
            {
                GetList(maxDepth),
                GetList(1)
            }
        };

        var attribute = new MaximumListDepthAttribute(maxDepth);
        var depth = attribute.MaxDepth(document);

        Assert.AreEqual(maxDepth, depth);

        var result = attribute.IsValid(document);
        Assert.IsTrue(result);
    }

    [Test]
    public void TestInvalidSiblingLists()
    {
        const int maxDepth = 5;

        var document = new Document
        {
            Content = new List<Node>
            {
                GetList(maxDepth),
                GetList(maxDepth * 2)
            }
        };

        var attribute = new MaximumListDepthAttribute(maxDepth);
        var depth = attribute.MaxDepth(document);

        Assert.AreEqual(maxDepth * 2, depth);

        var result = attribute.IsValid(document);
        Assert.IsFalse(result);
    }
}