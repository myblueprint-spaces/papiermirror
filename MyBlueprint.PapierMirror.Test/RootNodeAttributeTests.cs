using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Validation;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
internal class RootNodeAttributeTests
{
    [Test]
    public void ValidatesCorrectRootNode()
    {
        var document = new Document();

        var attribute = new RootNodeAttribute(typeof(Document));

        var result = attribute.IsValid(document);

        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Null values should be handled by the <see cref="RequiredAttribute"/>.
    /// </summary>
    [Test]
    public void ValidatesNullNode()
    {
        Node? document = null;

        var attribute = new RootNodeAttribute(typeof(Document));

        var result = attribute.IsValid(document);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ValidatesIncorrectRootNode()
    {
        Node textNode = new TextNode { Text = "Test"};

        var attribute = new RootNodeAttribute(typeof(Document));

        var result = attribute.IsValid(textNode);

        Assert.That(result, Is.False);
    }
}