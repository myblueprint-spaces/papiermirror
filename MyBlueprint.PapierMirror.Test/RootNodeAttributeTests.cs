using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
internal class RootNodeAttributeTests
{
    private class ValidationTarget
    {
        public ValidationTarget(Node? document)
        {
            Document = document;
        }

        [RootNode(typeof(Document))]
        public Node? Document { get; }
    }

    private static bool ValidateNode(Node? node)
    {
        var target = new ValidationTarget(node);
        var context = new ValidationContext(target);
        var results = new List<ValidationResult>();

        return Validator.TryValidateObject(target, context, results, true);
    }

    [Test]
    public void ValidatesCorrectRootNode()
    {
        var document = new Document();

        var result = ValidateNode(document);
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Null values should be handled by the <see cref="RequiredAttribute"/>.
    /// </summary>
    [Test]
    public void ValidatesNullNode()
    {
        Node? document = null;

        var result = ValidateNode(document);
        Assert.That(result, Is.True);
    }

    [Test]
    public void ValidatesIncorrectRootNode()
    {
        Node textNode = new TextNode { Text = "Test"};

        var result = ValidateNode(textNode);
        Assert.That(result, Is.False);
    }
}