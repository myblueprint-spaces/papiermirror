using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

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
    public async Task ValidatesCorrectRootNode()
    {
        var document = new Document();

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    /// <summary>
    /// Null values should be handled by the <see cref="RequiredAttribute"/>.
    /// </summary>
    [Test]
    public async Task ValidatesNullNode()
    {
        Node? document = null;

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task ValidatesIncorrectRootNode()
    {
        Node textNode = new TextNode { Text = "Test"};

        var result = ValidateNode(textNode);
        await Assert.That(result).IsFalse();
    }
}