using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
internal class TextNodeLengthAttributeTests
{
    [Test]
    public void SumsSimpleText()
    {
        var document = new Document { Content = new List<Node> { new TextNode { Text = "1234" } } };

        var textLength = TextNodeLengthAttribute.SumTextLength(document);
        Assert.That(textLength, Is.EqualTo(4));

        var attribute = new TextNodeLengthAttribute(5);
        var result = attribute.IsValid(document);

        Assert.That(result, Is.True);
    }

    [Test]
    public void SumsNestedNodes()
    {
        var document = new Document()
        {
            Content = new List<Node> { new Paragraph { Content = new List<Node> { new TextNode { Text = "1234567890" } } } }
        };

        var textLength = TextNodeLengthAttribute.SumTextLength(document);
        Assert.That(textLength, Is.EqualTo(10));

        var attribute = new TextNodeLengthAttribute(10);
        var result = attribute.IsValid(document);

        Assert.That(result, Is.True);
    }
}