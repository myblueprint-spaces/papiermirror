using MyBlueprint.PapierMirror.Json;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test
{
    internal static class TestDocument
    {
        public const string JsonString =
            "{\"type\":\"doc\",\"content\":[{\"type\":\"heading\",\"attrs\":{\"level\":3},\"content\":[{\"text\":\"Hello ProseMirror\",\"type\":\"text\"}]},{\"type\":\"paragraph\",\"content\":[{\"text\":\"This is editable text. You can focus it and start typing.\",\"type\":\"text\"}]},{\"type\":\"paragraph\",\"content\":[{\"text\":\"To apply styling, you can select a piece of text and manipulate its styling from the menu. The basic schema supports \",\"type\":\"text\"},{\"text\":\"emphasis\",\"type\":\"text\",\"marks\":[{\"type\":\"em\"}]},{\"text\":\", \",\"type\":\"text\"},{\"text\":\"strong text\",\"type\":\"text\",\"marks\":[{\"type\":\"strong\"}]},{\"text\":\", \",\"type\":\"text\"},{\"text\":\"links\",\"type\":\"text\",\"marks\":[{\"type\":\"link\",\"attrs\":{\"href\":\"http://marijnhaverbeke.nl/blog\",\"title\":null}}]},{\"text\":\", \",\"type\":\"text\"},{\"text\":\"code font\",\"type\":\"text\",\"marks\":[{\"type\":\"code\"}]},{\"text\":\", and \",\"type\":\"text\"},{\"type\":\"image\",\"attrs\":{\"alt\":null,\"src\":\"/img/smiley.png\",\"title\":null}},{\"text\":\" images.\",\"type\":\"text\"}]},{\"type\":\"paragraph\",\"content\":[{\"text\":\"Block-level structure can be manipulated with key bindings (try ctrl-shift-2 to create a level 2 heading, or enter in an empty textblock to exit the parent block), or through the menu.\",\"type\":\"text\"}]},{\"type\":\"paragraph\",\"content\":[{\"text\":\"Try using the list item in the menu to wrap this paragraph in a numbered list.\",\"type\":\"text\"}]}]}";

        public static readonly Node Document =
            JsonSerializer.Deserialize<Node>(JsonString, PapierMirrorJson.JsonOptions(Schema.All)) ??
            throw new InvalidOperationException("Invalid document");

        public const string HtmlString =
            "<html><body><h3>Hello ProseMirror</h3><p>This is editable text. You can focus it and start typing.</p><p>To apply styling, you can select a piece of text and manipulate its styling from the menu. The basic schema supports <em>emphasis</em>, <strong>strong text</strong>, <a href=\"http://marijnhaverbeke.nl/blog\">links</a>, <code>code font</code>, and <img src=\"/img/smiley.png\"> images.</p><p>Block-level structure can be manipulated with key bindings (try ctrl-shift-2 to create a level 2 heading, or enter in an empty textblock to exit the parent block), or through the menu.</p><p>Try using the list item in the menu to wrap this paragraph in a numbered list.</p></body></html>";
    }
}