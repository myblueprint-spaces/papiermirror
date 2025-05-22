using AngleSharp.Dom;
using MyBlueprint.PapierMirror.Json;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test
{
    public class CustomMark : Node
    {
        public CustomMark() : base("custom") { }

        public CustomMark(INode node) : this() { }

        protected override string[] Tags => ["custom"];

        public override INode GetHtmlNode(IDocument document)
        {
            return document.CreateElement("custom");
        }
    }

    public class NewMarkTest
    {
        [Test]
        public async Task SerializesCustomNode()
        {
            var nodeString = "{\"type\":\"custom\"}";

            var options = PapierMirrorJson.JsonOptions(new Schema([new CustomMark()]));
            var node = JsonSerializer.Deserialize<Node>(nodeString, options) ?? throw new InvalidOperationException("Failed");

            await Assert.That(node).IsNotNull();

            var serialized = JsonSerializer.Serialize(node, options);
            await Assert.That(nodeString).IsEqualTo(serialized);
        }
    }
}