namespace MyBlueprint.PapierMirror.Razor;

/// <summary>
/// What to do when a node type has neither a built-in renderer nor a registered override component.
/// </summary>
public enum UnknownNodeBehavior
{
    /// <summary>
    /// Render the node's children without any wrapping element.
    /// </summary>
    RenderChildren,

    /// <summary>
    /// Render nothing for the node or its children.
    /// </summary>
    Skip,

    /// <summary>
    /// Throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    Throw,
}

/// <summary>
/// Options controlling how PapierMirror documents are rendered by the Razor components.
/// </summary>
public sealed record PapierMirrorRenderOptions
{
    private static readonly IReadOnlyDictionary<Type, Type> EmptyMap = new Dictionary<Type, Type>();

    /// <summary>
    /// The default options.
    /// </summary>
    public static PapierMirrorRenderOptions Default { get; } = new();

    /// <summary>
    /// Override components keyed by <see cref="Node"/> type (exact type match). Each component must accept a
    /// <c>Node</c> parameter and a <c>ChildContent</c> <see cref="Microsoft.AspNetCore.Components.RenderFragment"/>;
    /// derive from <see cref="PapierMirrorNodeComponent{TNode}"/> for convenience.
    /// </summary>
    public IReadOnlyDictionary<Type, Type> NodeComponents { get; init; } = EmptyMap;

    /// <summary>
    /// Override components keyed by <see cref="Mark"/> type (exact type match). Each component must accept a
    /// <c>Mark</c> parameter and a <c>ChildContent</c> <see cref="Microsoft.AspNetCore.Components.RenderFragment"/>;
    /// derive from <see cref="PapierMirrorMarkComponent{TMark}"/> for convenience.
    /// </summary>
    public IReadOnlyDictionary<Type, Type> MarkComponents { get; init; } = EmptyMap;

    /// <summary>
    /// Applied to every <c>href</c> and <c>src</c> value. Return <see langword="null"/> to omit the attribute.
    /// Defaults to <see cref="PapierMirrorSanitizers.Url"/>.
    /// </summary>
    public Func<string?, string?> UrlSanitizer { get; init; } = PapierMirrorSanitizers.Url;

    /// <summary>
    /// Applied to every CSS value written to a <c>style</c> attribute. Return <see langword="null"/> to omit the declaration.
    /// Defaults to <see cref="PapierMirrorSanitizers.CssValue"/>.
    /// </summary>
    public Func<string?, string?> CssValueSanitizer { get; init; } = PapierMirrorSanitizers.CssValue;

    /// <summary>
    /// What to do with node types that have no renderer. Defaults to <see cref="UnknownNodeBehavior.RenderChildren"/>.
    /// </summary>
    public UnknownNodeBehavior UnknownNodeBehavior { get; init; } = UnknownNodeBehavior.RenderChildren;
}