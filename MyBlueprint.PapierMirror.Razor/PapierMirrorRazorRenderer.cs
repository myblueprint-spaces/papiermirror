using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MyBlueprint.PapierMirror.Razor;

/// <summary>
/// Renders PapierMirror documents to an HTML string with <see cref="HtmlRenderer"/>, for use outside a Razor
/// view or Blazor app (emails, PDFs, background jobs).
/// </summary>
public static class PapierMirrorRazorRenderer
{
    /// <summary>
    /// Render a PapierMirror document (or any node) to an HTML fragment.
    /// </summary>
    /// <param name="node">The node to render.</param>
    /// <param name="options">Rendering options. Defaults to <see cref="PapierMirrorRenderOptions.Default"/>.</param>
    /// <param name="services">
    /// Services used to activate components, required only if override components inject services.
    /// An <see cref="ILoggerFactory"/> is resolved from it when available.
    /// </param>
    /// <returns>A <see cref="Task{TResult}"/> producing the rendered HTML.</returns>
    public static async Task<string> RenderAsync(Node node, PapierMirrorRenderOptions? options = null, IServiceProvider? services = null)
    {
        ArgumentNullException.ThrowIfNull(node);

        services ??= EmptyServiceProvider.Instance;
        var loggerFactory = (ILoggerFactory?)services.GetService(typeof(ILoggerFactory)) ?? NullLoggerFactory.Instance;

        await using var renderer = new HtmlRenderer(services, loggerFactory);
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>
            {
                [nameof(PapierMirrorContent.Node)] = node,
                [nameof(PapierMirrorContent.Options)] = options,
            });

            var output = await renderer.RenderComponentAsync<PapierMirrorContent>(parameters);
            return output.ToHtmlString();
        });
    }

    private sealed class EmptyServiceProvider : IServiceProvider
    {
        public static readonly EmptyServiceProvider Instance = new();

        public object? GetService(Type serviceType) => null;
    }
}