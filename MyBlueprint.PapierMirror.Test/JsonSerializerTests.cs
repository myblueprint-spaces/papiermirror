using MyBlueprint.PapierMirror.Json;
using NUnit.Framework;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test;

[TestFixture]
public class JsonSerializerTests
{
    /// <summary>
    /// Tests the deserialization of a JSON document to a PapierMirror document.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public void SerializesFromJson()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Deserialize<Node>(TestDocument.JsonString, options) ?? throw new InvalidOperationException("Failed");

        Assert.NotNull(node);
        Assert.NotNull(node.Content);
        Assert.That(node.Content!.Count(), Is.EqualTo(5));
    }

    /// <summary>
    /// Tests the serialization of a PapierMirror document to a JSON document.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public void SerializesToJson()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Serialize(TestDocument.Document, options);

        Assert.That(node, Is.Not.Null);
    }

    /// <summary>
    /// Tests the serialization between JSON to PapierMirror can happen reversibly.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public void ShouldSerializeReversibly()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Deserialize<Node>(TestDocument.JsonString, options) ?? throw new InvalidOperationException("Failed");

        Assert.That(node, Is.Not.Null);

        var serialized = JsonSerializer.Serialize(node, options);
        Assert.AreEqual(TestDocument.JsonString, serialized);
    }
}