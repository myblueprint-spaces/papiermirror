using MyBlueprint.PapierMirror.Models.Marks;
using MyBlueprint.PapierMirror.Models.Nodes;

namespace MyBlueprint.PapierMirror.Test;

internal class EqualityTests
{
    /// <summary>
    /// Tests the equality of two documents with different text.
    /// </summary>
    [Test]
    public async Task NotEqualDocuments()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "B" }] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task EqualDocuments()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsTrue();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task EqualDocumentsWithMarks()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }], Marks = [new Strong()] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }], Marks = [new Emphasis()] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsFalse();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }

    /// <summary>
    /// Tests the equality of two documents with different structures.
    /// </summary>
    [Test]
    public async Task NotEqualStructures()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new TextNode { Text = "A" }] }, new HardBreak(), new Paragraph { Content = [new TextNode { Text = "B"}] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsFalse();
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task DifferentHeaders()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new Heading(1), new TextNode { Text = "A" }] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new Heading(2), new TextNode { Text = "A" }] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsFalse();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task SameHeaders()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new Heading(1), new TextNode { Text = "A" }] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new Heading(1), new TextNode { Text = "A" }] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsTrue();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task DifferentImages()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new Image("https://google.com/")] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new Image("https://reddit.com/")] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsFalse();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }

    /// <summary>
    /// Tests the equality of two documents with the same text.
    /// </summary>
    [Test]
    public async Task SameImages()
    {
        var documentA = new Document { Content = [new Paragraph { Content = [new Image("https://google.com/")] }] };
        var documentB = new Document { Content = [new Paragraph { Content = [new Image("https://google.com/")] }] };

        var result = documentA.Equals(documentB);

        await Assert.That(result).IsTrue();
        await Assert.That(documentA).IsNotSameReferenceAs(documentB);
    }
}