using System.Threading.Tasks;

namespace MyBlueprint.PapierMirror.Html;

/// <summary>
/// ProseMirror HTML serialization methods.
/// </summary>
public static class PapierMirrorHtmlSerializer
{
    /// <summary>
    /// Serialize a ProseMirror document to HTML.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="root"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public static Task<string> SerializeToHtmlAsync(Schema schema, Node root)
    {
        var serializer = new HtmlSerializer(schema);

        return serializer.ConvertToHtmlAsync(root);
    }

    /// <summary>
    /// Serialize a ProseMirror document from HTML.
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="html"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public static Task<Node> SerializeFromHtmlAsync(Schema schema, string html)
    {
        var serializer = new HtmlSerializer(schema);

        return serializer.ConvertFromHtmlAsync(html);
    }
}