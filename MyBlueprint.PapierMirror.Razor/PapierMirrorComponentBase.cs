using Microsoft.AspNetCore.Components;

namespace MyBlueprint.PapierMirror.Razor;

/// <summary>
/// Convenience base class for components registered in <see cref="PapierMirrorRenderOptions.NodeComponents"/>.
/// </summary>
/// <typeparam name="TNode">The node type this component renders.</typeparam>
public abstract class PapierMirrorNodeComponent<TNode> : ComponentBase
    where TNode : Node
{
    /// <summary>
    /// The node being rendered.
    /// </summary>
    [Parameter, EditorRequired]
    public TNode Node { get; set; } = default!;

    /// <summary>
    /// The rendered children of <see cref="Node"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// Convenience base class for components registered in <see cref="PapierMirrorRenderOptions.MarkComponents"/>.
/// </summary>
/// <typeparam name="TMark">The mark type this component renders.</typeparam>
public abstract class PapierMirrorMarkComponent<TMark> : ComponentBase
    where TMark : Mark
{
    /// <summary>
    /// The mark being rendered.
    /// </summary>
    [Parameter, EditorRequired]
    public TMark Mark { get; set; } = default!;

    /// <summary>
    /// The content the mark applies to.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}