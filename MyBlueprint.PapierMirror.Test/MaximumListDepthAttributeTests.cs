using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;
[TestFixture]
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
            list.Content = new List<Node>(list.Content ?? Enumerable.Empty<Node>()) { normalItem };
            list = nestedList;
        }

        return root;
    }

    [Test]
    public void TestsValidListDepth()
    {
        var document = GetDocument(MaxDepth);

        var attribute = new MaximumListDepthAttribute(MaxDepth);

        var depth = attribute.MaxDepth(document);
        Assert.AreEqual(MaxDepth, depth);
        
        var result = ValidateNode(document);
        Assert.IsTrue(result);
    }

    [Test]
    public void TestsInvalidListDepth()
    {
        var document = GetDocument(MaxDepth + 1);

        var attribute = new MaximumListDepthAttribute(MaxDepth);

        var depth = attribute.MaxDepth(document);
        Assert.AreEqual(MaxDepth + 1, depth);

        var result = ValidateNode(document);
        Assert.IsFalse(result);
    }

    [Test]
    public void TestValidSiblingLists()
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

        Assert.AreEqual(MaxDepth, depth);

        var result = ValidateNode(document);
        Assert.IsTrue(result);
    }

    [Test]
    public void TestInvalidSiblingLists()
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

        Assert.AreEqual(MaxDepth * 2, depth);

        var result = ValidateNode(document);
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