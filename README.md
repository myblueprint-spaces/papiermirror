# myBlueprint PapierMirror - a .NET ProseMirror library
PapierMirror is a  [ProseMirror](https://github.com/ProseMirror/prosemirror) library written for .NET. This package is intended for server applications written in .NET to interface with frontend applications that use ProseMirror. The name comes from Papier which is the myBlueprint design language we use at myBlueprint/SpacesEDU. 

## Requirements
.NET 10.

## Rendering with Razor
`MyBlueprint.PapierMirror.Razor` renders a PapierMirror `Node` tree with Razor components. It needs no AngleSharp DOM and no `@Html.Raw`, and Razor HTML-encodes all text and attribute values. A `doc` node renders only its children, so the output is a fragment you can drop into any page.

**Blazor**
```razor
<PapierMirrorContent Node="@document" />
```

**MVC / Razor Pages (.cshtml)**
```cshtml
<component type="typeof(PapierMirrorContent)" render-mode="Static" param-Node="Model.Body" />
```

**Outside a request (emails, PDFs, background jobs)**
```csharp
var html = await PapierMirrorRazorRenderer.RenderAsync(document);
```

### Options
Pass a `PapierMirrorRenderOptions` as the `Options` parameter, or as the second argument to `RenderAsync`:

- `NodeComponents` / `MarkComponents` replace the markup for a node or mark type, including custom types in your `Schema`. Derive from `PapierMirrorNodeComponent<TNode>` or `PapierMirrorMarkComponent<TMark>`. The component receives the node or mark and a `ChildContent` fragment.
- `UrlSanitizer` is applied to `href` and `src`. The default allows relative URLs plus `http`, `https`, `mailto` and `tel`.
- `CssValueSanitizer` is applied to values written into `style`. The default rejects values that could end a declaration or load a resource.
- `UnknownNodeBehavior` controls node types with no renderer: `RenderChildren` (the default), `Skip` or `Throw`.

```razor
<PapierMirrorContent Node="@document" Options="@(new PapierMirrorRenderOptions
{
    NodeComponents = new Dictionary<Type, Type> { [typeof(Image)] = typeof(LazyImage) },
})" />
```
