using MyBlueprint.PapierMirror.Html;
using NUnit.Framework;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
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

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.GreaterThan(0));
    }

    /// <summary>
    /// Tests the deserialization of an HTML document to a PapierMirror document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Test]
    public async Task SerializeFromHtmlAsync()
    {
        var result = await PapierMirrorHtmlSerializer.SerializeFromHtmlAsync(Schema.All, TestDocument.HtmlString);

        Assert.NotNull(result);
        Assert.NotNull(result.Content);
        Assert.That(result.Content!.Count(), Is.EqualTo(5));
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

        Assert.AreEqual(TestDocument.HtmlString, document);
    }
}