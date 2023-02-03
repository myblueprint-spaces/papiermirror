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

        var result = TextNodeLengthAttribute.SumTextLength(document);

        Assert.That(result, Is.EqualTo(4));
    }
}