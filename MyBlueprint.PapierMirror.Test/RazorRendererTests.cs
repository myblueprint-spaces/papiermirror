using AngleSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MyBlueprint.PapierMirror.Html;
using MyBlueprint.PapierMirror.Json;
using MyBlueprint.PapierMirror.Models.Nodes;
using MyBlueprint.PapierMirror.Razor;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test;

internal class RazorRendererTests
{
    private static readonly JsonSerializerOptions JsonOptions = PapierMirrorJson.JsonOptions(Schema.All);

    /// <summary>
    /// The Razor renderer produces the same markup as the AngleSharp serializer for the JSON test document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MatchesHtmlSerializerForJsonDocument()
    {
        await AssertParityAsync(TestDocument.Document);
    }

    /// <summary>
    /// The Razor renderer produces the same markup as the AngleSharp serializer for the list test document.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MatchesHtmlSerializerForListDocument()
    {
        await AssertParityAsync(Parse(TestDocument.ListString));
    }

    /// <summary>
    /// The Razor renderer produces the same markup as the AngleSharp serializer for a document parsed from HTML.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MatchesHtmlSerializerForParsedHtml()
    {
        var node = await PapierMirrorHtmlSerializer.SerializeFromHtmlAsync(Schema.All, TestDocument.HtmlString);
        await AssertParityAsync(node);
    }

    /// <summary>
    /// Node attributes are rendered, and defaults are omitted.
    /// </summary>
    /// <param name="json">The document content JSON.</param>
    /// <param name="expected">The expected HTML.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("""{"type":"heading","attrs":{"level":2,"textAlign":"center"},"content":[{"type":"text","text":"Hi"}]}""", """<h2 style="text-align:center">Hi</h2>""")]
    [Arguments("""{"type":"heading","content":[{"type":"text","text":"Hi"}]}""", "<h1>Hi</h1>")]
    [Arguments("""{"type":"paragraph","attrs":{"textAlign":"right"},"content":[{"type":"text","text":"a"}]}""", """<p style="text-align:right">a</p>""")]
    [Arguments("""{"type":"orderedList","attrs":{"order":3},"content":[{"type":"listItem","content":[{"type":"paragraph","content":[{"type":"text","text":"x"}]}]}]}""", """<ol start="3"><li><p>x</p></li></ol>""")]
    [Arguments("""{"type":"orderedList","attrs":{"order":1},"content":[]}""", "<ol></ol>")]
    [Arguments("""{"type":"bulletList","content":[{"type":"listItem","content":[]}]}""", "<ul><li></li></ul>")]
    [Arguments("""{"type":"image","attrs":{"src":"/a.png","alt":"A","title":"T","width":10,"height":20}}""", """<img src="/a.png" alt="A" title="T" width="10" height="20">""")]
    [Arguments("""{"type":"paragraph","content":[{"type":"text","text":"a"},{"type":"hardBreak"},{"type":"text","text":"b"}]}""", "<p>a<br>b</p>")]
    [Arguments("""{"type":"blockquote","content":[{"type":"horizontalRule"}]}""", "<blockquote><hr></blockquote>")]
    [Arguments("""{"type":"codeBlock","content":[{"type":"text","text":"x < y"}]}""", "<pre>x &lt; y</pre>")]
    public async Task RendersNodes(string json, string expected)
    {
        var html = await RenderContentAsync(json);
        await Assert.That(html).IsEqualTo(await NormalizeAsync(expected));
    }

    /// <summary>
    /// Marks are nested with the first mark outermost, and mark attributes are rendered.
    /// </summary>
    /// <param name="marksJson">The marks JSON array.</param>
    /// <param name="expected">The expected HTML.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("""[{"type":"strong"},{"type":"link","attrs":{"href":"https://x.test","title":"X","target":"_blank"}}]""", """<strong><a href="https://x.test" title="X" target="_blank">t</a></strong>""")]
    [Arguments("""[{"type":"em"},{"type":"code"},{"type":"strike"},{"type":"underline"}]""", "<em><code><s><u>t</u></s></code></em>")]
    [Arguments("""[{"type":"subscript"},{"type":"superscript"},{"type":"mark"}]""", "<sub><sup><mark>t</mark></sup></sub>")]
    [Arguments("""[{"type":"textStyle","attrs":{"color":"#ff0000","fontSize":"12px","background":"rgb(0, 0, 0)"}}]""", """<span style="color:#ff0000;font-size:12px;background:rgb(0, 0, 0);">t</span>""")]
    public async Task RendersMarks(string marksJson, string expected)
    {
        var html = await RenderContentAsync($$"""{"type":"text","text":"t","marks":{{marksJson}}}""");
        await Assert.That(html).IsEqualTo(await NormalizeAsync(expected));
    }

    /// <summary>
    /// Text content is HTML-encoded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task EncodesText()
    {
        var html = await PapierMirrorRazorRenderer.RenderAsync(new TextNode { Text = "<script>alert(1)</script>" });
        await Assert.That(html).IsEqualTo("&lt;script&gt;alert(1)&lt;/script&gt;");
    }

    /// <summary>
    /// Unsafe URLs and CSS values are dropped.
    /// </summary>
    /// <param name="marksJson">The marks JSON array.</param>
    /// <param name="expected">The expected HTML.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("""[{"type":"link","attrs":{"href":"javascript:alert(1)"}}]""", "<a>t</a>")]
    [Arguments("""[{"type":"link","attrs":{"href":" JaVa\tScript:alert(1)"}}]""", "<a>t</a>")]
    [Arguments("""[{"type":"link","attrs":{"href":"data:text/html,x"}}]""", "<a>t</a>")]
    [Arguments("""[{"type":"textStyle","attrs":{"color":"red;position:fixed"}}]""", "<span>t</span>")]
    [Arguments("""[{"type":"textStyle","attrs":{"background":"url(/track)"}}]""", "<span>t</span>")]
    public async Task DropsUnsafeValues(string marksJson, string expected)
    {
        var html = await RenderContentAsync($$"""{"type":"text","text":"t","marks":{{marksJson}}}""");
        await Assert.That(html).IsEqualTo(expected);
    }

    /// <summary>
    /// The default URL sanitizer keeps relative URLs and allowed schemes.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="allowed">Whether the URL should be kept.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments("/path?a=b:c", true)]
    [Arguments("page.html#x:y", true)]
    [Arguments("https://example.test", true)]
    [Arguments("MAILTO:a@example.test", true)]
    [Arguments("tel:+15555555555", true)]
    [Arguments("javascript:void(0)", false)]
    [Arguments("vbscript:x", false)]
    [Arguments("", false)]
    public async Task UrlSanitizer(string url, bool allowed)
    {
        await Assert.That(PapierMirrorSanitizers.Url(url) is not null).IsEqualTo(allowed);
    }

    /// <summary>
    /// A registered node component replaces the built-in markup and receives the rendered children.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UsesNodeOverride()
    {
        var options = new PapierMirrorRenderOptions
        {
            NodeComponents = new Dictionary<Type, Type> { [typeof(Paragraph)] = typeof(FancyParagraph) },
        };

        var html = await PapierMirrorRazorRenderer.RenderAsync(
            Parse("""{"type":"doc","content":[{"type":"paragraph","attrs":{"textAlign":"left"},"content":[{"type":"text","text":"a","marks":[{"type":"strong"}]}]}]}"""),
            options);

        await Assert.That(html).IsEqualTo("""<div class="fancy" data-align="left"><strong>a</strong></div>""");
    }

    /// <summary>
    /// Unknown node types follow <see cref="PapierMirrorRenderOptions.UnknownNodeBehavior"/>.
    /// </summary>
    /// <param name="behavior">The behavior.</param>
    /// <param name="expected">The expected HTML, or <see langword="null"/> if rendering should throw.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    [Arguments(UnknownNodeBehavior.RenderChildren, "inner")]
    [Arguments(UnknownNodeBehavior.Skip, "")]
    [Arguments(UnknownNodeBehavior.Throw, null)]
    public async Task HandlesUnknownNodes(UnknownNodeBehavior behavior, string? expected)
    {
        var node = new CustomMark { Content = [new TextNode { Text = "inner" }] };
        var options = new PapierMirrorRenderOptions { UnknownNodeBehavior = behavior };

        if (expected is null)
        {
            await Assert.That(() => PapierMirrorRazorRenderer.RenderAsync(node, options)).Throws<InvalidOperationException>();
            return;
        }

        await Assert.That(await PapierMirrorRazorRenderer.RenderAsync(node, options)).IsEqualTo(expected);
    }

    private static Node Parse(string json) =>
        JsonSerializer.Deserialize<Node>(json, JsonOptions) ?? throw new InvalidOperationException("Invalid document");

    private static async Task<string> RenderContentAsync(string contentJson) =>
        await NormalizeAsync(await PapierMirrorRazorRenderer.RenderAsync(Parse($$"""{"type":"doc","content":[{{contentJson}}]}""")));

    private static async Task AssertParityAsync(Node node)
    {
        var expected = await NormalizeAsync(await PapierMirrorHtmlSerializer.SerializeToHtmlAsync(Schema.All, node));
        var actual = await NormalizeAsync(await PapierMirrorRazorRenderer.RenderAsync(node));

        await Assert.That(actual).IsEqualTo(expected);
    }

    // Re-serializes through AngleSharp so void-element syntax and entity choices don't affect comparisons.
    private static async Task<string> NormalizeAsync(string html)
    {
        using var context = BrowsingContext.New(Configuration.Default);
        using var document = await context.OpenAsync(req => req.Content(html));
        return document.Body!.InnerHtml;
    }

    private sealed class FancyParagraph : PapierMirrorNodeComponent<Paragraph>
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "fancy");
            builder.AddAttribute(2, "data-align", (Node.Attributes as ParagraphAttributes)?.TextAlign);
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }
    }
}