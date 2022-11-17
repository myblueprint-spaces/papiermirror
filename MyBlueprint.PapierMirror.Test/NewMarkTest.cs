using AngleSharp.Dom;
using MyBlueprint.PapierMirror.Json;
using NUnit.Framework;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test
{
    public class CustomMark : Node
    {
        public CustomMark() : base("custom") { }

        public CustomMark(INode node) : this() { }

        protected override string[] Tags => new[] { "custom" };

        public override INode GetHtmlNode(IDocument document)
        {
            return document.CreateElement("custom");
        }
    }

    [TestFixture]
    public class NewMarkTest
    {
        [Test]
        public void SerializesCustomNode()
        {
            var nodeString = "{\"type\":\"custom\"}";

            var options = PapierMirrorJson.JsonOptions(new Schema(new[] { new CustomMark()}));
            var node = JsonSerializer.Deserialize<Node>(nodeString, options) ?? throw new InvalidOperationException("Failed");

            Assert.That(node, Is.Not.Null);

            var serialized = JsonSerializer.Serialize(node, options);
            Assert.AreEqual(nodeString, serialized);
        }
    }
}