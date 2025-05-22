using MyBlueprint.PapierMirror.Html;

namespace MyBlueprint.PapierMirror.Test;

internal class HtmlSerializerTests
{
    /// <summary>
    /// Tests the serialization of a PapierMirror document to an HTML document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Test]
    public async Task SerializeToHtmlAsync()
    {
        var result = await PapierMirrorHtmlSerializer.SerializeToHtmlAsync(Schema.All, TestDocument.Document);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Length).IsGreaterThan(0);
    }

    /// <summary>
    /// Tests the deserialization of an HTML document to a PapierMirror document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Test]
    public async Task SerializeFromHtmlAsync()
    {
        var result = await PapierMirrorHtmlSerializer.SerializeFromHtmlAsync(Schema.All, TestDocument.HtmlString);

        using var _ = Assert.Multiple();
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Content).IsNotNull();
        await Assert.That(result.Content!.Count()).IsEqualTo(5);
    }

    /// <summary>
    /// Tests the serialization between HTML to PapierMirror can happen reversibly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Test]
    public async Task SerializeReversiblyAsync()
    {
        var node = await PapierMirrorHtmlSerializer.SerializeFromHtmlAsync(Schema.All, TestDocument.HtmlString);

        var document = await PapierMirrorHtmlSerializer.SerializeToHtmlAsync(Schema.All, node);

        await Assert.That(TestDocument.HtmlString).IsEqualTo(document);
    }
}