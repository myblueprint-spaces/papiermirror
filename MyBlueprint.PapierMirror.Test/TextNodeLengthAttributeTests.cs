using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
internal class TextNodeLengthAttributeTests
{
    private class ValidationTarget
    {
        public ValidationTarget(Node? document)
        {
            Document = document;
        }

        [TextNodeLength(5)]
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
    public void SumsSimpleText()
    {
        var document = new Document { Content = new Node[] { new TextNode { Text = "1234" } } };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        Assert.That(textLength, Is.EqualTo(4));

        var result = ValidateNode(document);
        Assert.That(result, Is.True);
    }

    [Test]
    public void SumsNestedNodes()
    {
        var document = new Document
        {
            Content = new Node[] { new Paragraph { Content = new Node[] { new TextNode { Text = "123" }, new TextNode { Text = "45" } } } }
        };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        Assert.That(textLength, Is.EqualTo(5));

        var result = ValidateNode(document);
        Assert.That(result, Is.True);
    }

    [Test]
    public void RejectsTooManyCharacters()
    {
        var document = new Document
        {
            Content = new Node[] { new Paragraph { Content = new Node[] { new TextNode { Text = "123" }, new TextNode { Text = "45" } } }, new Paragraph { Content = new Node[] { new TextNode { Text = "789" } } } }
        };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        Assert.That(textLength, Is.EqualTo(9));

        var result = ValidateNode(document);
        Assert.That(result, Is.False);
    }
}