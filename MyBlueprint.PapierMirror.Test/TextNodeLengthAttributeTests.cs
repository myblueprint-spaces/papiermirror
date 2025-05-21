using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

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
    public async Task SumsSimpleText()
    {
        var document = new Document { Content = [new TextNode { Text = "1234" }] };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        await Assert.That(textLength).IsEqualTo(4);

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task SumsNestedNodes()
    {
        var document = new Document
        {
            Content = [new Paragraph { Content = [new TextNode { Text = "123" }, new TextNode { Text = "45" }] }
            ]
        };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        await Assert.That(textLength).IsEqualTo(5);

        var result = ValidateNode(document);
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task RejectsTooManyCharacters()
    {
        var document = new Document
        {
            Content = [new Paragraph { Content = [new TextNode { Text = "123" }, new TextNode { Text = "45" }] }, new Paragraph { Content =
                [new TextNode { Text = "678" }]
                }
            ]
        };

        var attribute = new TextNodeLengthAttribute(5);
        var textLength = attribute.SumTextLength(document);
        await Assert.That(textLength).IsEqualTo(8);

        var result = ValidateNode(document);
        await Assert.That(result).IsFalse();
    }
}